﻿using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Sockets;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.AccessControl;

namespace pylorak.Windows.WFP
{
    public enum FieldMatchType
    {
        FWP_MATCH_EQUAL,
        FWP_MATCH_GREATER,
        FWP_MATCH_LESS,
        FWP_MATCH_GREATER_OR_EQUAL,
        FWP_MATCH_LESS_OR_EQUAL,
        FWP_MATCH_RANGE,
        FWP_MATCH_FLAGS_ALL_SET,
        FWP_MATCH_FLAGS_ANY_SET,
        FWP_MATCH_FLAGS_NONE_SET,
        FWP_MATCH_EQUAL_CASE_INSENSITIVE,
        FWP_MATCH_NOT_EQUAL,
        FWP_MATCH_TYPE_MAX
    }

    public enum FieldKeyNames
    {
        Unknown,
        FWPM_CONDITION_IP_LOCAL_ADDRESS,
        FWPM_CONDITION_IP_REMOTE_ADDRESS,
        FWPM_CONDITION_IP_LOCAL_PORT,
        FWPM_CONDITION_IP_REMOTE_PORT,
        FWPM_CONDITION_IP_PROTOCOL,
        FWPM_CONDITION_ALE_APP_ID,
        FWPM_CONDITION_ALE_ORIGINAL_APP_ID,
        FWPM_CONDITION_ICMP_TYPE,
        FWPM_CONDITION_ICMP_CODE,
        FWPM_CONDITION_ORIGINAL_ICMP_TYPE,
    }

    public class FilterCondition : IDisposable
    {
        protected Interop.FWPM_FILTER_CONDITION0 _nativeStruct;
        private int _referenceCount;

        public void AddRef()
        {
            ++_referenceCount;
        }

        public void RemoveRef()
        {
            Debug.Assert(_referenceCount > 0);
            --_referenceCount;
            if (0 == _referenceCount)
                Dispose();
        }

        public int ReferenceCount => _referenceCount;

        public Guid FieldKey
        {
            get { return _nativeStruct.fieldKey; }
        }

        protected FieldKeyNames? _fieldKeyName;
        public FieldKeyNames FieldKeyName
        {
            get
            {
                if (_fieldKeyName.HasValue)
                    return _fieldKeyName.Value;

                if (0 == _nativeStruct.fieldKey.CompareTo(ConditionKeys.FWPM_CONDITION_IP_LOCAL_ADDRESS))
                    _fieldKeyName = FieldKeyNames.FWPM_CONDITION_IP_LOCAL_ADDRESS;
                else if (0 == _nativeStruct.fieldKey.CompareTo(ConditionKeys.FWPM_CONDITION_IP_REMOTE_ADDRESS))
                    _fieldKeyName = FieldKeyNames.FWPM_CONDITION_IP_REMOTE_ADDRESS;
                else if (0 == _nativeStruct.fieldKey.CompareTo(ConditionKeys.FWPM_CONDITION_IP_LOCAL_PORT))
                    _fieldKeyName = FieldKeyNames.FWPM_CONDITION_IP_LOCAL_PORT;
                else if (0 == _nativeStruct.fieldKey.CompareTo(ConditionKeys.FWPM_CONDITION_IP_REMOTE_PORT))
                    _fieldKeyName = FieldKeyNames.FWPM_CONDITION_IP_REMOTE_PORT;
                else if (0 == _nativeStruct.fieldKey.CompareTo(ConditionKeys.FWPM_CONDITION_IP_PROTOCOL))
                    _fieldKeyName = FieldKeyNames.FWPM_CONDITION_IP_PROTOCOL;
                else if (0 == _nativeStruct.fieldKey.CompareTo(ConditionKeys.FWPM_CONDITION_ALE_APP_ID))
                    _fieldKeyName = FieldKeyNames.FWPM_CONDITION_ALE_APP_ID;
                else if (0 == _nativeStruct.fieldKey.CompareTo(ConditionKeys.FWPM_CONDITION_ALE_ORIGINAL_APP_ID))
                    _fieldKeyName = FieldKeyNames.FWPM_CONDITION_ALE_ORIGINAL_APP_ID;
                else if (0 == _nativeStruct.fieldKey.CompareTo(ConditionKeys.FWPM_CONDITION_ICMP_TYPE))
                    _fieldKeyName = FieldKeyNames.FWPM_CONDITION_ICMP_TYPE;
                else if (0 == _nativeStruct.fieldKey.CompareTo(ConditionKeys.FWPM_CONDITION_ICMP_CODE))
                    _fieldKeyName = FieldKeyNames.FWPM_CONDITION_ICMP_CODE;
                else if (0 == _nativeStruct.fieldKey.CompareTo(ConditionKeys.FWPM_CONDITION_ORIGINAL_ICMP_TYPE))
                    _fieldKeyName = FieldKeyNames.FWPM_CONDITION_ORIGINAL_ICMP_TYPE;
                else
                    _fieldKeyName = FieldKeyNames.Unknown;

                return _fieldKeyName.Value;
            }
        }

        public FieldMatchType MatchType
        {
            get { return _nativeStruct.matchType; }
            set
            {
                if ((FieldMatchType.FWP_MATCH_NOT_EQUAL == value) && !VersionInfo.Win7OrNewer)
                    throw new NotSupportedException("FWP_MATCH_NOT_EQUAL requires Windows 7 or newer.");

                _nativeStruct.matchType = value;
            }
        }

        public Interop.FWP_CONDITION_VALUE0 ConditionValue
        {
            get { return _nativeStruct.conditionValue; }
        }

        protected FilterCondition()
        {
            _nativeStruct = new Interop.FWPM_FILTER_CONDITION0();
        }

        internal FilterCondition(Interop.FWPM_FILTER_CONDITION0 cond0)
        {
            _nativeStruct = cond0;
        }

        public FilterCondition(Guid fieldKey, FieldMatchType matchType, Interop.FWP_CONDITION_VALUE0 conditionValue)
        {
            _nativeStruct.fieldKey = fieldKey;
            _nativeStruct.matchType = matchType;
            _nativeStruct.conditionValue = conditionValue;
        }

        internal Interop.FWPM_FILTER_CONDITION0 Marshal()
        {
            return _nativeStruct;
        }

        public void Dispose()
        {
            Debug.Assert(_referenceCount == 0);
            if (_referenceCount > 0)
                return;

            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing) { }
    }

    public enum RemoteOrLocal
    {
        Remote,
        Local
    }

    public sealed class IpFilterCondition : FilterCondition
    {
        private static readonly byte[] MaskByteBitsLookup = new byte[]
        { 0x00, 0x80, 0xC0, 0xE0, 0xF0, 0xF8, 0xFC, 0xFE, 0xFF };

        private SafeHGlobalHandle? nativeMem;

        public IpFilterCondition(IPAddress addr, byte subnetLen, RemoteOrLocal peer)
        {
            if (((addr.AddressFamily == AddressFamily.InterNetwork) && (subnetLen > 32))
             || ((addr.AddressFamily == AddressFamily.InterNetworkV6) && (subnetLen > 128)))
                throw new ArgumentOutOfRangeException(nameof(subnetLen));

            _nativeStruct.matchType = FieldMatchType.FWP_MATCH_EQUAL;
            _nativeStruct.fieldKey = (peer == RemoteOrLocal.Local) ? ConditionKeys.FWPM_CONDITION_IP_LOCAL_ADDRESS : ConditionKeys.FWPM_CONDITION_IP_REMOTE_ADDRESS;

            byte[] addressBytes = addr.GetAddressBytes();

            switch (addr.AddressFamily)
            {
                case System.Net.Sockets.AddressFamily.InterNetwork:
                    IsIPv6 = false;
                    if (subnetLen == 32)
                    {
                        Array.Reverse(addressBytes);
                        _nativeStruct.conditionValue.type = Interop.FWP_DATA_TYPE.FWP_UINT32;
                        _nativeStruct.conditionValue.value.uint32 = BitConverter.ToUInt32(addressBytes, 0);
                    }
                    else
                    {
                        // Convert CIDR subnet length to byte array
                        uint maskBits = 0;
                        int prefix = subnetLen;
                        for (int i = 0; i < 4; ++i)
                        {
                            int s = (prefix < 8) ? prefix : 8;
                            maskBits = (maskBits << 8) | MaskByteBitsLookup[s];
                            prefix -= s;
                        }
                        Array.Reverse(addressBytes);

                        var addrAndMask4 = new Interop.FWP_V4_ADDR_AND_MASK();
                        addrAndMask4.addr = BitConverter.ToUInt32(addressBytes, 0);
                        addrAndMask4.mask = maskBits;
                        nativeMem = SafeHGlobalHandle.FromStruct(addrAndMask4);

                        _nativeStruct.conditionValue.type = Interop.FWP_DATA_TYPE.FWP_V4_ADDR_MASK;
                        _nativeStruct.conditionValue.value.v4AddrMask = nativeMem.DangerousGetHandle();
                    }
                    break;
                case System.Net.Sockets.AddressFamily.InterNetworkV6:
                    IsIPv6 = true;
                    if (subnetLen == 128)
                    {
                        nativeMem = SafeHGlobalHandle.Alloc(16);
                        IntPtr ptr = nativeMem.DangerousGetHandle();
                        System.Runtime.InteropServices.Marshal.Copy(addressBytes, 0, ptr, 16);

                        _nativeStruct.conditionValue.type = Interop.FWP_DATA_TYPE.FWP_BYTE_ARRAY16_TYPE;
                        _nativeStruct.conditionValue.value.byteArray16 = ptr;
                    }
                    else
                    {
                        var addrAndMask6 = new Interop.FWP_V6_ADDR_AND_MASK();
                        unsafe
                        {
                            fixed (byte* addrSrcPtr = addressBytes)
                            {
                                Buffer.MemoryCopy(addrSrcPtr, addrAndMask6.addr, 16, 16);
                            }
                        }
                        addrAndMask6.prefixLength = subnetLen;
                        nativeMem = SafeHGlobalHandle.FromStruct(addrAndMask6);

                        _nativeStruct.conditionValue.type = Interop.FWP_DATA_TYPE.FWP_V6_ADDR_MASK;
                        _nativeStruct.conditionValue.value.v6AddrMask = nativeMem.DangerousGetHandle();
                    }
                    break;
                default:
                    throw new NotSupportedException("Only the IPv4 and IPv6 address families are supported.");
            }
        }

        public bool IsIPv6 { get; }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                nativeMem?.Dispose();
                nativeMem = null;
            }

            base.Dispose(disposing);
        }
    }

    public sealed class PortFilterCondition : FilterCondition
    {
        private SafeHGlobalHandle? rangeNativeMem;

        private PortFilterCondition(RemoteOrLocal peer)
        {
            _nativeStruct.fieldKey = (peer == RemoteOrLocal.Local) ? ConditionKeys.FWPM_CONDITION_IP_LOCAL_PORT : ConditionKeys.FWPM_CONDITION_IP_REMOTE_PORT;
        }

        public PortFilterCondition(ushort portNumber, RemoteOrLocal peer)
            : this(peer)
        {
            Init(portNumber);
        }

        public PortFilterCondition(ushort minPort, ushort maxPort, RemoteOrLocal peer)
            : this(peer)
        {
            if (minPort == maxPort)
                Init(minPort);
            else
                Init(minPort, maxPort);
        }

        public PortFilterCondition(string portOrRange, RemoteOrLocal peer)
            : this(peer)
        {
            bool isRange = (-1 != portOrRange.IndexOf('-'));
            if (isRange)
            {
                string[] minmax = portOrRange.Split('-');
                ushort min = ushort.Parse(minmax[0], System.Globalization.CultureInfo.InvariantCulture);
                ushort max = ushort.Parse(minmax[1], System.Globalization.CultureInfo.InvariantCulture);
                Init(min, max);
            }
            else
            {
                ushort portNumber = ushort.Parse(portOrRange, System.Globalization.CultureInfo.InvariantCulture);
                Init(portNumber);
            }
        }

        private void Init(ushort portNumber)
        {
            _nativeStruct.matchType = FieldMatchType.FWP_MATCH_EQUAL;
            _nativeStruct.conditionValue.type = Interop.FWP_DATA_TYPE.FWP_UINT16;
            _nativeStruct.conditionValue.value.uint16 = portNumber;
        }

        private void Init(ushort minPort, ushort maxPort)
        {
            _nativeStruct.matchType = FieldMatchType.FWP_MATCH_RANGE;

            var range = new Interop.FWP_RANGE0();
            range.valueLow.type = Interop.FWP_DATA_TYPE.FWP_UINT16;
            range.valueLow.value.uint16 = minPort;
            range.valueHigh.type = Interop.FWP_DATA_TYPE.FWP_UINT16;
            range.valueHigh.value.uint16 = maxPort;

            rangeNativeMem = SafeHGlobalHandle.FromStruct(range);
            _nativeStruct.conditionValue.type = Interop.FWP_DATA_TYPE.FWP_RANGE_TYPE;
            _nativeStruct.conditionValue.value.rangeValue = rangeNativeMem.DangerousGetHandle();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                rangeNativeMem?.Dispose();
                rangeNativeMem = null;
            }

            base.Dispose(disposing);
        }
    }

    // RFC 1700
    public enum IpProtocol : byte
    {
        HOPOPT = 0,
        ICMPv4 = 1,
        IGMP = 2,
        TCP = 6,
        UDP = 17,
        GRE = 47,
        ESP = 50,
        AH = 51,
        ICMPv6 = 58
    }

    public sealed class ProtocolFilterCondition : FilterCondition
    {
        public ProtocolFilterCondition(byte proto)
        {
            Init(proto);
        }

        public ProtocolFilterCondition(IpProtocol proto)
        {
            Init((byte)proto);
        }

        private void Init(byte proto)
        {
            _nativeStruct.matchType = FieldMatchType.FWP_MATCH_EQUAL;
            _nativeStruct.fieldKey = ConditionKeys.FWPM_CONDITION_IP_PROTOCOL;
            _nativeStruct.conditionValue.type = Interop.FWP_DATA_TYPE.FWP_UINT8;
            _nativeStruct.conditionValue.value.uint8 = proto;
        }
    }

    public sealed class AppIdFilterCondition : FilterCondition
    {
        [SuppressUnmanagedCodeSecurity]
        internal static class NativeMethods
        {
            [DllImport("FWPUClnt.dll", EntryPoint = "FwpmGetAppIdFromFileName0")]
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
            internal static extern uint FwpmGetAppIdFromFileName0(
                [MarshalAs(UnmanagedType.LPWStr), In] string fileName,
                [Out] out FwpmMemorySafeHandle appId);
        }

        private readonly SafeHandle NativeMem;

        public AppIdFilterCondition(string filePath, bool bBeforeProxying = false, bool alreadyKernelFormat = false)
        {
            if (bBeforeProxying && !VersionInfo.Win8OrNewer)
                throw new NotSupportedException("FWPM_CONDITION_ALE_ORIGINAL_APP_ID (set by bBeforeProxying) requires Windows 8 or newer.");

            if ((!alreadyKernelFormat && System.IO.File.Exists(filePath)) || filePath.Equals("System", StringComparison.OrdinalIgnoreCase))
            {
                // NOTE: FwpmGetAppIdFromFileName0() will sometimes create invalid AppIds depending on
                //       international characters and casing. To avoid this, it is recommended to
                //       avoid calling FwpmGetAppIdFromFileName0() completely by passing in paths already
                //       in kernel format.
                uint err = NativeMethods.FwpmGetAppIdFromFileName0(filePath, out FwpmMemorySafeHandle tmpHandle);
                NativeMem = tmpHandle;
                if (0 != err)
                    throw new WfpException(err, "FwpmGetAppIdFromFileName0");
            }
            else
            {
                unsafe
                {
                    fixed (char* src = filePath.ToLowerInvariant()) // WFP will only match if lowercase
                        NativeMem = PInvokeHelper.CreateWfpBlob((IntPtr)src, filePath.Length * 2, true);
                }
            }

            _nativeStruct.matchType = FieldMatchType.FWP_MATCH_EQUAL;
            _nativeStruct.fieldKey = bBeforeProxying ? ConditionKeys.FWPM_CONDITION_ALE_ORIGINAL_APP_ID : ConditionKeys.FWPM_CONDITION_ALE_APP_ID;
            _nativeStruct.conditionValue.type = Interop.FWP_DATA_TYPE.FWP_BYTE_BLOB_TYPE;
            _nativeStruct.conditionValue.value.byteBlob = NativeMem.DangerousGetHandle();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                NativeMem.Dispose();
            }

            base.Dispose(disposing);
        }
    }

    public abstract class SecurityDescriptorFilterCondition : FilterCondition
    {
        private static class NativeMethods
        {
            [DllImport("advapi32", SetLastError = true, CharSet = CharSet.Unicode)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool ConvertStringSecurityDescriptorToSecurityDescriptor(
                string stringSd,
                uint stringSdRevision,
                out IntPtr resultSd,
                ref int resultSdLength);

            [DllImport("kernel32")]
            public static extern IntPtr LocalFree(IntPtr hMem);
        }

        private SafeHandle NativeMem;

        public SecurityDescriptorFilterCondition(Guid fieldKey, FieldMatchType matchType, string sddl)
        {
            Init(fieldKey, matchType, sddl);
        }

        [MemberNotNull(nameof(NativeMem))]
        protected void Init(Guid fieldKey, FieldMatchType matchType, string sddl)
        {
            // Get SDDL in binary form
            IntPtr nativeArray = IntPtr.Zero;
            try
            {
                int byteArraySize = 0;
                if (!NativeMethods.ConvertStringSecurityDescriptorToSecurityDescriptor(sddl, 1, out nativeArray, ref byteArraySize))
                    throw new Win32Exception();

                Init(fieldKey, matchType, nativeArray, byteArraySize);
            }
            finally
            {
                NativeMethods.LocalFree(nativeArray);
            }
        }


        [MemberNotNull(nameof(NativeMem))]
        protected void Init(Guid fieldKey, FieldMatchType matchType, RawSecurityDescriptor sd)
        {
            byte[] sdBinaryForm = new byte[sd.BinaryLength];
            sd.GetBinaryForm(sdBinaryForm, 0);
            Init(fieldKey, matchType, sdBinaryForm);
        }

        [MemberNotNull(nameof(NativeMem))]
        protected unsafe void Init(Guid fieldKey, FieldMatchType matchType, byte[] sdBinaryForm)
        {
            unsafe
            {
                fixed (byte* src = sdBinaryForm)
                    Init(fieldKey, matchType, (IntPtr)src, sdBinaryForm.Length);
            }
        }

        [MemberNotNull(nameof(NativeMem))]
        protected void Init(Guid fieldKey, FieldMatchType matchType, IntPtr sdBinaryFormPtr, int sdBinaryFormLength)
        {
            NativeMem = PInvokeHelper.CreateWfpBlob(sdBinaryFormPtr, sdBinaryFormLength);
            _nativeStruct.matchType = matchType;
            _nativeStruct.fieldKey = fieldKey;
            _nativeStruct.conditionValue.type = Interop.FWP_DATA_TYPE.FWP_SECURITY_DESCRIPTOR_TYPE;
            _nativeStruct.conditionValue.value.sd = NativeMem.DangerousGetHandle();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                NativeMem.Dispose();
            }

            base.Dispose(disposing);
        }
    }

    public sealed class ServiceNameFilterCondition : SecurityDescriptorFilterCondition
    {
        public ServiceNameFilterCondition(string serviceName)
            : base(ConditionKeys.FWPM_CONDITION_ALE_USER_ID, FieldMatchType.FWP_MATCH_EQUAL, $"O:SYG:SYD:(A;;CCRC;;;{GetServiceSidFromName(serviceName)})")
        {
        }

        private static string GetServiceSidFromName(string serviceName)
        {
#if false
            /*
             * This piece of code is the "standard" solution, but it only
             * allows retrieval of service SIDs for already installed services.
             * Also, it is about 8x slower.
             */
            NTAccount f = new NTAccount(@"NT SERVICE\" + serviceName);
            SecurityIdentifier sid = (SecurityIdentifier)f.Translate(typeof(SecurityIdentifier));
            return sid.ToString();
#endif

            // For the steps of converting a service name to a service SID, see:
            // https://pcsxcetrasupport3.wordpress.com/2013/09/08/how-do-you-get-a-service-sid-from-a-service-name/

            // 1: Input service same.
            // haha

            // 2: Convert service name to upper case.
            serviceName = serviceName.ToUpperInvariant();

            // 3: Get the Unicode bytes()  from the upper case service name.
            byte[] unicode = System.Text.UnicodeEncoding.Unicode.GetBytes(serviceName);

            // 4: Run bytes() thru the sha1 hash function.
            using var hasher = new System.Security.Cryptography.SHA1Managed();
            var sha1 = hasher.ComputeHash(unicode);

            // 5: Reverse the byte() string  returned from the SHA1 hash function(on Little Endian systems Not tested on Big Endian systems)
            // Optimised away by reversing array order in steps 7 and 10.

            // 6: Split the reversed string into 5 blocks of 4 bytes each.
            unsafe
            {
                var dec = stackalloc uint[5];
                for (int i = 0; i < 5; ++i)
                {
                    // 7: Convert each block of hex bytes() to Decimal
                    dec[i] =
                        ((uint)sha1[i * 4 + 3] << 24) +
                        ((uint)sha1[i * 4 + 2] << 16) +
                        ((uint)sha1[i * 4 + 1] << 8) +
                        ((uint)sha1[i * 4 + 0] << 0);
                }

                // 8: Reverse the Position of the blocks.
                // 9: Create the first part of the SID "S-1-5-80-"
                // 10: Tack on each block of Decimal strings with a "-" in between each block that was converted and reversed.
                // 11: Finally out put the complete SID for the service.
                return $"S-1-5-80-{dec[0].ToString()}-{dec[1].ToString()}-{dec[2].ToString()}-{dec[3].ToString()}-{dec[4].ToString()}";
            }
        }
    }

    public sealed class UserIdFilterCondition : SecurityDescriptorFilterCondition
    {
        public UserIdFilterCondition(string sid, RemoteOrLocal peer)
            : base((RemoteOrLocal.Local == peer) ? ConditionKeys.FWPM_CONDITION_ALE_USER_ID : ConditionKeys.FWPM_CONDITION_ALE_REMOTE_USER_ID,
                  FieldMatchType.FWP_MATCH_EQUAL,
                  $"O:LSD:(A;;CC;;;{sid}))")
        {
        }
    }

    public sealed class PackageIdFilterCondition : FilterCondition
    {
        [SuppressUnmanagedCodeSecurity]
        internal static class NativeMethods
        {
            [DllImport("advapi32", SetLastError = true, CharSet = CharSet.Unicode)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool ConvertStringSidToSid(string stringSid, out AllocHLocalSafeHandle ptrSid);

            [DllImport("userenv", SetLastError = false, CharSet = CharSet.Unicode)]
            internal static extern int DeriveAppContainerSidFromAppContainerName(string appContainerName, out SidSafeHandle sid);
        }

        private SafeHandle sidNativeMem;

        public PackageIdFilterCondition(IntPtr sid)
        {
            if (!VersionInfo.Win8OrNewer)
                throw new NotSupportedException("FWPM_CONDITION_ALE_PACKAGE_ID requires Windows 8 or newer.");

            Init(PInvokeHelper.CopyNativeSid(sid));
        }

        public PackageIdFilterCondition(string sid)
        {
            if (!VersionInfo.Win8OrNewer)
                throw new NotSupportedException("FWPM_CONDITION_ALE_PACKAGE_ID requires Windows 8 or newer.");

            if (!NativeMethods.ConvertStringSidToSid(sid, out AllocHLocalSafeHandle tmpHndl))
                throw new Win32Exception(System.Runtime.InteropServices.Marshal.GetLastWin32Error());

            Init(tmpHndl);
        }

        private PackageIdFilterCondition(SafeHandle sid)
        {
            if (!VersionInfo.Win8OrNewer)
                throw new NotSupportedException("FWPM_CONDITION_ALE_PACKAGE_ID requires Windows 8 or newer.");

            Init(sid);
        }

        [MemberNotNull(nameof(sidNativeMem))]
        private void Init(SafeHandle sidHandle)
        {
            sidNativeMem = sidHandle;

            _nativeStruct.matchType = FieldMatchType.FWP_MATCH_EQUAL;
            _nativeStruct.fieldKey = ConditionKeys.FWPM_CONDITION_ALE_PACKAGE_ID;
            _nativeStruct.conditionValue.type = Interop.FWP_DATA_TYPE.FWP_SID;
            _nativeStruct.conditionValue.value.sd = sidNativeMem.DangerousGetHandle();
        }

        public static PackageIdFilterCondition FromPackageFamilyName(string packageFamilyName)
        {
            if (!VersionInfo.Win8OrNewer)
                throw new NotSupportedException("FWPM_CONDITION_ALE_PACKAGE_ID requires Windows 8 or newer.");

            var err = NativeMethods.DeriveAppContainerSidFromAppContainerName(packageFamilyName, out SidSafeHandle tmpHndl);
            if (0 != err)
                throw new ArgumentException($"DeriveAppContainerSidFromAppContainerName() returned {err}.");

            return new PackageIdFilterCondition(tmpHndl);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                sidNativeMem.Dispose();
            }

            base.Dispose(disposing);
        }
    }

    public sealed class IcmpTypeFilterCondition : FilterCondition
    {
        public IcmpTypeFilterCondition(ushort icmpType)
        {
            _nativeStruct.matchType = FieldMatchType.FWP_MATCH_EQUAL;
            _nativeStruct.fieldKey = ConditionKeys.FWPM_CONDITION_ORIGINAL_ICMP_TYPE;
            _nativeStruct.conditionValue.type = Interop.FWP_DATA_TYPE.FWP_UINT16;
            _nativeStruct.conditionValue.value.uint16 = icmpType;
        }
    }
    public sealed class IcmpErrorTypeFilterCondition : FilterCondition
    {
        public IcmpErrorTypeFilterCondition(ushort icmpType)
        {
            _nativeStruct.matchType = FieldMatchType.FWP_MATCH_EQUAL;
            _nativeStruct.fieldKey = ConditionKeys.FWPM_CONDITION_ICMP_TYPE;
            _nativeStruct.conditionValue.type = Interop.FWP_DATA_TYPE.FWP_UINT16;
            _nativeStruct.conditionValue.value.uint16 = icmpType;
        }
    }
    public sealed class IcmpErrorCodeFilterCondition : FilterCondition
    {
        public IcmpErrorCodeFilterCondition(ushort icmpCode)
        {
            _nativeStruct.matchType = FieldMatchType.FWP_MATCH_EQUAL;
            _nativeStruct.fieldKey = ConditionKeys.FWPM_CONDITION_ICMP_CODE;
            _nativeStruct.conditionValue.type = Interop.FWP_DATA_TYPE.FWP_UINT16;
            _nativeStruct.conditionValue.value.uint16 = icmpCode;
        }
    }


    [Flags]
    public enum ConditionFlags : uint
    {
        FWP_CONDITION_FLAG_IS_LOOPBACK = 0x00000001,
        FWP_CONDITION_FLAG_IS_IPSEC_SECURED = 0x00000002,
        FWP_CONDITION_FLAG_IS_REAUTHORISE = 0x00000004,
        FWP_CONDITION_FLAG_IS_WILDCARD_BIND = 0x00000008,
        FWP_CONDITION_FLAG_IS_RAW_ENDPOINT = 0x00000010,
        FWP_CONDITION_FLAG_IS_FRAGMENT = 0x00000020,
        FWP_CONDITION_FLAG_IS_FRAGMENT_GROUP = 0x00000040,
        FWP_CONDITION_FLAG_IS_IPSEC_NATT_RECLASSIFY = 0x00000080,
        FWP_CONDITION_FLAG_REQUIRES_ALE_CLASSIFY = 0x00000100,
        FWP_CONDITION_FLAG_IS_IMPLICIT_BIND = 0x00000200,
        FWP_CONDITION_FLAG_IS_REASSEMBLED = 0x00000400,
        FWP_CONDITION_FLAG_IS_NAME_APP_SPECIFIED = 0x00004000,
        FWP_CONDITION_FLAG_IS_PROMISCUOUS = 0x00008000,
        FWP_CONDITION_FLAG_IS_AUTH_FW = 0x00010000,
        FWP_CONDITION_FLAG_IS_RECLASSIFY = 0x00020000,
        FWP_CONDITION_FLAG_IS_OUTBOUND_PASS_THRU = 0x00040000,
        FWP_CONDITION_FLAG_IS_INBOUND_PASS_THRU = 0x00080000,
        FWP_CONDITION_FLAG_IS_CONNECTION_REDIRECTED = 0x00100000,
        FWP_CONDITION_FLAG_IS_PROXY_CONNECTION = 0x00200000,
        FWP_CONDITION_FLAG_IS_APPCONTAINER_LOOPBACK = 0x00400000,
        FWP_CONDITION_FLAG_IS_NON_APPCONTAINER_LOOPBACK = 0x00800000,
        FWP_CONDITION_FLAG_IS_RESERVED = 0x01000000,
        FWP_CONDITION_FLAG_IS_HONOURING_POLICY_AUTHORISE = 0x02000000
    }

    public sealed class FlagsFilterCondition : FilterCondition
    {
        public FlagsFilterCondition(ConditionFlags flags, FieldMatchType matchType)
        {
            _nativeStruct.matchType = matchType;
            _nativeStruct.fieldKey = ConditionKeys.FWPM_CONDITION_FLAGS;
            _nativeStruct.conditionValue.type = Interop.FWP_DATA_TYPE.FWP_UINT32;
            _nativeStruct.conditionValue.value.uint32 = (uint)flags;
        }
    }

    public sealed class LocalInterfaceCondition : FilterCondition
    {
        [SuppressUnmanagedCodeSecurity]
        internal static class NativeMethods
        {
            [DllImport("Iphlpapi", SetLastError = false, CharSet = CharSet.Unicode)]
            internal static extern int ConvertInterfaceAliasToLuid(string stringSid, [Out] out ulong InterfaceLuid);
        }

        public static bool InterfaceAliasExists(string ifAlias)
        {
            const int NO_ERROR = 0;
            const int ERROR_INVALID_PARAMETER = 87;

            int err = NativeMethods.ConvertInterfaceAliasToLuid(ifAlias, out ulong _);
            return err switch
            {
                NO_ERROR => true,
                ERROR_INVALID_PARAMETER => false,
                _ => throw new Win32Exception(err)
            };
        }

        private readonly SafeHGlobalHandle NativeMem;

        public LocalInterfaceCondition(string ifAlias)
        {
            int err = NativeMethods.ConvertInterfaceAliasToLuid(ifAlias, out ulong luid);
            if (0 != err)
                throw new Win32Exception(err);

            NativeMem = SafeHGlobalHandle.FromStruct(luid);

            _nativeStruct.matchType = FieldMatchType.FWP_MATCH_EQUAL;
            _nativeStruct.fieldKey = ConditionKeys.FWPM_CONDITION_IP_LOCAL_INTERFACE;
            _nativeStruct.conditionValue.type = Interop.FWP_DATA_TYPE.FWP_UINT64;
            _nativeStruct.conditionValue.value.uint64 = NativeMem.DangerousGetHandle();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                NativeMem.Dispose();
            }

            base.Dispose(disposing);
        }
    }

}