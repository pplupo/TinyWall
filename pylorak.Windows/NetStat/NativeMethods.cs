﻿using System;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;

namespace pylorak.Windows.NetStat
{
    internal static class NativeMethods
    {
        internal const int AfInet = 2;
        internal const int AfInet6 = 23;

        private const string DllName = "iphlpapi.dll";

        [DllImport(DllName, SetLastError = true)]
        public static extern uint GetExtendedTcpTable(IntPtr tcpTable, ref int tcpTableLength, [MarshalAs(UnmanagedType.Bool)] bool sort, int ipVersion, TcpTableType tcpTableType, int reserved);

        [DllImport(DllName, SetLastError = true)]
        public static extern uint GetExtendedUdpTable(IntPtr udpTable, ref int udpTableLength, [MarshalAs(UnmanagedType.Bool)] bool sort, int ipVersion, UdpTableType udpTableType, int reserved);

        public enum TcpTableType
        {
            BasicListener,
            BasicConnections,
            BasicAll,
            OwnerPidListener,
            OwnerPidConnections,
            OwnerPidAll,
            OwnerModuleListener,
            OwnerModuleConnections,
            OwnerModuleAll,
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Tcp4Table
        {
            public uint length;
            public Tcp4Row row;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Tcp4Row
        {
            public TcpState state;
            public uint localAddr;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] localPort;
            public uint remoteAddr;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] remotePort;
            public uint owningPid;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Tcp6Table
        {
            public uint length;
            public Tcp6Row row;
        }


        [StructLayout(LayoutKind.Sequential)]
        public struct Tcp6Row
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public byte[] localAddr;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] localScopeId;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] localPort;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public byte[] remoteAddr;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] remoteScopeId;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] remotePort;

            public TcpState state;
            public uint owningPid;
        }

        public enum UdpTableType
        {
            Basic,
            OwnerPid,
            OwnerModule,
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Udp4Table
        {
            public uint length;
            public Udp4Row row;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Udp4Row
        {
            public uint localAddr;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] localPort;
            public uint owningPid;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Udp6Table
        {
            public uint length;
            public Udp6Row row;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Udp6Row
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public byte[] localAddr;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] localScopeId;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] localPort;

            public uint owningPid;
        }
    }
}
