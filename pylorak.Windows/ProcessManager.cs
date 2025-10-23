﻿
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace pylorak.Windows
{
    public readonly struct ProcessSnapshotEntry
    {
        public readonly string ImagePath;
        public readonly long CreationTime;
        public readonly uint ProcessId;
        public readonly uint ParentProcessId;

        public ProcessSnapshotEntry(string path, long creationTime, uint pid, uint parentPid)
        {
            ImagePath = path;
            CreationTime = creationTime;
            ProcessId = pid;
            ParentProcessId = parentPid;
        }
    }

    public static class ProcessManager
    {
        [SuppressUnmanagedCodeSecurity]
        protected static class NativeMethods
        {
            [DllImport("kernel32", SetLastError = true)]
            internal static extern SafeObjectHandle OpenProcess(ProcessAccessFlags dwDesiredAccess, bool bInheritHandle, uint dwProcessId);

            [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Unicode)]
            internal static extern bool QueryFullProcessImageName(SafeObjectHandle hProcess, QueryFullProcessImageNameFlags dwFlags, [Out] StringBuilder lpExeName, ref int size);

            [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Unicode)]
            internal static unsafe extern bool QueryFullProcessImageName(SafeObjectHandle hProcess, QueryFullProcessImageNameFlags dwFlags, [Out] char* lpExeName, ref int size);

            [DllImport("ntdll")]
            internal static extern int NtQueryInformationProcess(SafeObjectHandle hProcess, int processInformationClass, [Out] out PROCESS_BASIC_INFORMATION processInformation, int processInformationLength, out int returnLength);

            [DllImport("kernel32", SetLastError = true)]
            internal static extern SafeObjectHandle CreateToolhelp32Snapshot(SnapshotFlags flags, int id);
            [DllImport("kernel32", SetLastError = true)]
            internal static extern bool Process32First(SafeObjectHandle hSnapshot, [In, Out] ref PROCESSENTRY32 lppe);
            [DllImport("kernel32", SetLastError = true)]
            internal static extern bool Process32Next(SafeObjectHandle hSnapshot, [In, Out] ref PROCESSENTRY32 lppe);

            [DllImport("user32", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool PostThreadMessage(int threadId, uint msg, UIntPtr wParam, IntPtr lParam);

            [DllImport("kernel32", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool GetProcessTimes(SafeObjectHandle hProcess, out long lpCreationTime, out long lpExitTime, out long lpKernelTime, out long lpUserTime);

            [DllImport("advapi32", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool OpenProcessToken(
                SafeObjectHandle ProcessToken,
                TokenAccessLevels DesiredAccess,
                out SafeObjectHandle TokenHandle);

            [DllImport("advapi32", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool GetTokenInformation(
                SafeObjectHandle TokenHandle,
                TokenInformationClass TokenInformationClass,
                HeapSafeHandle TokenInformation,
                int TokenInformationLength,
                out int ReturnLength);
        }

        protected enum TokenInformationClass
        {
            TokenUser = 1,
            TokenGroups,
            TokenPrivileges,
            TokenOwner,
            TokenPrimaryGroup,
            TokenDefaultDacl,
            TokenSource,
            TokenType,
            TokenImpersonationLevel,
            TokenStatistics,
            TokenRestrictedSids,
            TokenSessionId,
            TokenGroupsAndPrivileges,
            TokenSessionReference,
            TokenSandBoxInert,
            TokenAuditPolicy,
            TokenOrigin,
            TokenElevationType,
            TokenLinkedToken,
            TokenElevation,
            TokenHasRestrictions,
            TokenAccessInformation,
            TokenVirtualisationAllowed,
            TokenVirtualisationEnabled,
            TokenIntegrityLevel,
            TokenUIAccess,
            TokenMandatoryPolicy,
            TokenLogonSid,
            TokenIsAppContainer,
            TokenCapabilities,
            TokenAppContainerSid,
            TokenAppContainerNumber,
            TokenUserClaimAttributes,
            TokenDeviceClaimAttributes,
            TokenRestrictedUserClaimAttributes,
            TokenRestrictedDeviceClaimAttributes,
            TokenDeviceGroups,
            TokenRestrictedDeviceGroups,
            TokenSecurityAttributes,
            TokenIsRestricted,
            TokenProcessTrustLevel,
            TokenPrivateNameSpace,
            TokenSingletonAttributes,
            TokenBnoIsolation,
            TokenChildProcessFlags,
            TokenIsLessPrivilegedAppContainer,
            TokenIsSandboxed,
            TokenOriginatingProcessTrustLevel,
            MaxTokenInfoClass
        }

        [StructLayout(LayoutKind.Sequential)]
        protected struct TOKEN_APPCONTAINER_INFORMATION
        {
            public IntPtr TokenAppContainer;
        };

        [Flags]
        internal enum TokenAccessLevels
        {
            AssignPrimary = 0x00000001,
            Duplicate = 0x00000002,
            Impersonate = 0x00000004,
            TokenQuery = 0x00000008,
            QuerySource = 0x00000010,
            AdjustPrivileges = 0x00000020,
            AdjustGroups = 0x00000040,
            AdjustDefault = 0x00000080,
            AdjustSessionId = 0x00000100,

            Read = 0x00020000 | TokenQuery,

            Write = 0x00020000 | AdjustPrivileges | AdjustGroups | AdjustDefault,

            AllAccess = 0x000F0000 |
                AssignPrimary |
                Duplicate |
                Impersonate |
                TokenQuery |
                QuerySource |
                AdjustPrivileges |
                AdjustGroups |
                AdjustDefault |
                AdjustSessionId,

            MaximumAllowed = 0x02000000
        }

        [StructLayout(LayoutKind.Sequential)]
        protected ref struct PROCESS_BASIC_INFORMATION
        {
            // Fore more info, see docs for NtQueryInformationProcess()
            public IntPtr Reserved1;
            public IntPtr PebBaseAddress;
            public IntPtr Reserved2_0;
            public IntPtr Reserved2_1;
            public IntPtr UniqueProcessId;
            public IntPtr InheritedFromUniqueProcessId;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal unsafe struct PROCESSENTRY32
        {
            public uint dwSize;
            public uint cntUsage;
            public uint th32ProcessID;
            public IntPtr th32DefaultHeapID;
            public uint th32ModuleID;
            public uint cntThreads;
            public uint th32ParentProcessID;
            public int pcPriClassBase;
            public uint dwFlags;
            public fixed char szExeFile[260];
        };

        [Flags]
        internal enum SnapshotFlags : uint
        {
            HeapList = 0x00000001,
            Process = 0x00000002,
            Thread = 0x00000004,
            Module = 0x00000008,
            Module32 = 0x00000010,
            All = (HeapList | Process | Thread | Module),
            Inherit = 0x80000000,
            NoHeaps = 0x40000000
        }

        [Flags]
        public enum ProcessAccessFlags
        {
            All = 0x001F0FFF,
            Terminate = 0x00000001,
            CreateThread = 0x00000002,
            VMOperation = 0x00000008,
            VMRead = 0x00000010,
            VMWrite = 0x00000020,
            DupHandle = 0x00000040,
            SetInformation = 0x00000200,
            QueryInformation = 0x00000400,
            Synchronise = 0x00100000,
            ReadControl = 0x00020000,
            QueryLimitedInformation = 0x00001000,
        }

        [Flags]
        public enum QueryFullProcessImageNameFlags
        {
            Win32Format = 0,
            NativeFormat = 1
        }

        private const int MAX_PATH_BUFF_CHARS = 1040;

        public static string ExecutablePath { get; } = GetCurrentExecutablePath();
        private static string GetCurrentExecutablePath()
        {
            using var proc = Process.GetCurrentProcess();
            var pid = unchecked((uint)proc.Id);
            return GetProcessPath(pid) ?? proc.MainModule.FileName;
        }
        public static string GetProcessPath(uint processId)
        {
            StringBuilder? buffer = null;
            return GetProcessPath(processId, ref buffer);
        }

        public static string GetProcessPath(uint processId, ref StringBuilder? buffer)
        {
            using var hProcess = NativeMethods.OpenProcess(ProcessAccessFlags.QueryLimitedInformation, false, processId);
            return GetProcessPath(hProcess, ref buffer);
        }

        public static string GetProcessPath(SafeObjectHandle hProcess, ref StringBuilder? buffer)
        {
            // This method needs Windows Vista or newer OS
            System.Diagnostics.Debug.Assert(Environment.OSVersion.Version.Major >= 6);

            if (hProcess.IsInvalid)
                return string.Empty;

            // First, try a smaller buffer on the stack.
            // This is more eficient both memory and performance-wise, and covers most cases.
            const int STACK_BUFF_BYTES = 1024;
            const int STACK_BUFF_CHARS = STACK_BUFF_BYTES / 2;
            int numChars = STACK_BUFF_CHARS;
            unsafe
            {
                var stack_buffer = stackalloc char[STACK_BUFF_CHARS];
                if (NativeMethods.QueryFullProcessImageName(hProcess, QueryFullProcessImageNameFlags.NativeFormat, stack_buffer, ref numChars))
                {
                    if (numChars == 0)
                        return string.Empty;

                    return PathMapper.Instance.ConvertPathIgnoreErrors(new ReadOnlySpan<char>(stack_buffer, numChars), PathFormat.Win32);
                }
            }

            // If the stack buffer wasn't big enough, allocate a larger buffer on the heap and try again
            const int ERROR_INSUFFICIENT_BUFFER = 122;
            if (ERROR_INSUFFICIENT_BUFFER == Marshal.GetLastWin32Error())
            {
                if (buffer is null)
                {
                    buffer = new StringBuilder(MAX_PATH_BUFF_CHARS);
                }
                else
                {
                    buffer.Clear();
                    buffer.EnsureCapacity(MAX_PATH_BUFF_CHARS);
                }
                numChars = buffer.Capacity;
                if (NativeMethods.QueryFullProcessImageName(hProcess, QueryFullProcessImageNameFlags.NativeFormat, buffer, ref numChars))
                {
                    if (numChars == 0)
                        return string.Empty;

                    return PathMapper.Instance.ConvertPathIgnoreErrors(buffer.ToString(), PathFormat.Win32);
                }
            }

            return string.Empty;
        }

        public static bool GetParentProcess(uint processId, ref uint parentPid)
        {
            using var hProcess = NativeMethods.OpenProcess(ProcessAccessFlags.QueryLimitedInformation, false, processId);
            if (hProcess.IsInvalid)
                return false;
            //throw new Exception($"Cannot open process Id {processId}.");

            if (VersionInfo.IsWow64Process)
            {
                return false;
                //throw new NotSupportedException("This method is not supported in 32-bit process on a 64-bit OS.");
            }
            else
            {
                var pbi = new PROCESS_BASIC_INFORMATION();
                var status = NativeMethods.NtQueryInformationProcess(hProcess, 0, out pbi, Marshal.SizeOf(typeof(PROCESS_BASIC_INFORMATION)), out int returnLength);
                if (status < 0)
                    throw new Exception($"NTSTATUS: {status}");

                parentPid = unchecked((uint)pbi.InheritedFromUniqueProcessId.ToInt32());

                // parentPid might have been reused and thus might not be the actual parent.
                // Check process creation times to figure it out.
                using var hParentProcess = NativeMethods.OpenProcess(ProcessAccessFlags.QueryLimitedInformation, false, parentPid);
                if (GetProcessCreationTime(hParentProcess, out long parentCreation) && GetProcessCreationTime(hProcess, out long childCreation))
                    return parentCreation <= childCreation;
                else
                    return false;
            }
        }

        private static bool GetProcessCreationTime(SafeObjectHandle hProcess, out long creationTime)
        {
            return NativeMethods.GetProcessTimes(hProcess, out creationTime, out _, out _, out _);
        }

        private static IEnumerable<PROCESSENTRY32> CreateToolhelp32Snapshot()
        {
            const int ERROR_NO_MORE_FILES = 18;

            var pe32 = new PROCESSENTRY32();
            pe32.dwSize = (uint)Marshal.SizeOf(typeof(PROCESSENTRY32));
            using var hSnapshot = NativeMethods.CreateToolhelp32Snapshot(SnapshotFlags.Process, 0);
            if (hSnapshot.IsInvalid)
                throw new Win32Exception(Marshal.GetLastWin32Error());

            if (!NativeMethods.Process32First(hSnapshot, ref pe32))
            {
                int errno = Marshal.GetLastWin32Error();
                if (errno == ERROR_NO_MORE_FILES)
                    yield break;
                throw new Win32Exception(errno);
            }
            do
            {
                yield return pe32;
            } while (NativeMethods.Process32Next(hSnapshot, ref pe32));
        }

        public static IEnumerable<ProcessSnapshotEntry> CreateToolhelp32SnapshotExtended()
        {
            StringBuilder? sbuilder = null;
            foreach (var p in CreateToolhelp32Snapshot())
            {
                using var hProcess = NativeMethods.OpenProcess(ProcessAccessFlags.QueryLimitedInformation, false, p.th32ProcessID);
                GetProcessCreationTime(hProcess, out long creationTime);
                yield return new ProcessSnapshotEntry(
                    (p.th32ProcessID != 0) ? GetProcessPath(hProcess, ref sbuilder) : "System",
                    creationTime,
                    p.th32ProcessID,
                    p.th32ParentProcessID
                );
            }
        }

        public static void WakeMessageQueues(Process p)
        {
            foreach (ProcessThread thread in p.Threads)
            {
                const uint WM_NULL = 0;
                NativeMethods.PostThreadMessage(thread.Id, WM_NULL, UIntPtr.Zero, IntPtr.Zero);
            }
        }

        public static void TerminateProcess(Process p, int timeoutMs)
        {
            if (p.MainWindowHandle == IntPtr.Zero)
            {
                foreach (ProcessThread thread in p.Threads)
                {
                    const uint WM_QUIT = 0x0012;
                    NativeMethods.PostThreadMessage(thread.Id, WM_QUIT, UIntPtr.Zero, IntPtr.Zero);
                }
            }
            else
            {
                p.CloseMainWindow();
            }
            if (!p.WaitForExit(timeoutMs))
            {
                p.Kill();
                p.WaitForExit(1000);
            }
        }

        public static string? GetAppContainerSid(uint pid)
        {
            using var hProcess = NativeMethods.OpenProcess(ProcessAccessFlags.QueryInformation, false, pid);
            if (!NativeMethods.OpenProcessToken(hProcess, TokenAccessLevels.TokenQuery, out SafeObjectHandle hToken))
                return null;

            try
            {
                const int hTokenInfoMemSize = 128;
                using var hTokenInfo = new HeapSafeHandle(hTokenInfoMemSize);
                if (!NativeMethods.GetTokenInformation(hToken, TokenInformationClass.TokenAppContainerSid, hTokenInfo, hTokenInfoMemSize, out _))
                    return null;

                var tokenAppContainerInfo = Marshal.PtrToStructure<TOKEN_APPCONTAINER_INFORMATION>(hTokenInfo.DangerousGetHandle());
                if (tokenAppContainerInfo.TokenAppContainer == IntPtr.Zero)
                    return null;

                return SafeSidHandle.ToStringSid(tokenAppContainerInfo.TokenAppContainer);
            }
            finally
            {
                hToken.Dispose();
            }
        }
    }
}
