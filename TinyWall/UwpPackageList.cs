﻿using pylorak.Windows;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using Windows.Management.Deployment;

namespace pylorak.TinyWall
{
    public class UwpPackageList : IReadOnlyList<UwpPackageList.Package>
    {
        public enum TamperedState
        {
            Unknown,
            No,
            Yes
        }

        public readonly struct Package : IEquatable<Package>
        {
            [SuppressUnmanagedCodeSecurity]
            private static class NativeMethods
            {
                [DllImport("Userenv", CharSet = CharSet.Unicode)]
                public static extern int DeriveAppContainerSidFromAppContainerName(string pszAppContainerName, out SafeSidHandle ppsidAppContainerSid);
            }

            public readonly string Name;
            public readonly string Publisher;
            public readonly string PublisherId;
            public readonly string Sid;
            public readonly TamperedState Tampered;

            public Package(global::Windows.ApplicationModel.Package p)
            {
                Name = p.Id.Name;
                Publisher = p.Id.Publisher;
                PublisherId = p.Id.PublisherId;
                Tampered = p.Status.Tampered ? TamperedState.Yes : TamperedState.No;

                SafeSidHandle? pSid = null;
                try
                {
                    if (0 != NativeMethods.DeriveAppContainerSidFromAppContainerName(p.Id.FamilyName, out pSid))
                        throw new ArgumentException("Cannot determine package SID.");

                    Sid = pSid.GetStringSid() ?? string.Empty;
                }
                finally
                {
                    pSid?.Dispose();
                }
            }

            public override int GetHashCode()
            {
                return Name.GetHashCode()
                    ^ Publisher.GetHashCode()
                    ^ PublisherId.GetHashCode()
                    ^ Sid.GetHashCode()
                    ^ Tampered.GetHashCode();
            }

            public override bool Equals(object? obj)
            {
                return obj is Package other && Equals(other);
            }

            public bool Equals(Package other)
            {
                return
                    (Name == other.Name)
                    && (Publisher == other.Publisher)
                    && (PublisherId == other.PublisherId)
                    && (Sid == other.Sid)
                    && (Tampered == other.Tampered);
            }

            public static bool operator ==(Package o1, Package o2)
            {
                return o1.Equals(o2);
            }

            public static bool operator !=(Package o1, Package o2)
            {
                return !o1.Equals(o2);
            }
        }

        private List<Package>? _packages;
        private List<Package> Packages
        {
            get
            {
                if (_packages is not null) return _packages;

                try
                {
                    _packages = CreatePackageList();
                }
                catch
                {
                    // Return an empty list if we cannot enumerate the packages on the system.
                    // This happens for exmaple when the AppXSVC service is disabled.
                    _packages = new List<Package>();
                }
                return _packages;
            }
        }

        public int Count => ((IReadOnlyCollection<Package>)Packages).Count;

        public Package this[int index] => ((IReadOnlyList<Package>)Packages)[index];

        private static List<Package> CreatePackageList()
        {
            var pm = new PackageManager();
            var packageList = pm.FindPackagesForUser(string.Empty);
            var resultList = new List<Package>();
            foreach (var p in packageList)
            {
                try
                {
                    resultList.Add(new Package(p));
                }
                catch
                {
                    // ignored
                }
            }

            return resultList;
        }

        public Package? FindPackage(string? sid)
        {
            if (string.IsNullOrEmpty(sid))
                return null;

            foreach (var package in Packages.Where(package => package.Sid.Equals(sid)))
            {
                return package;
            }

            return null;
        }

        public Package? FindPackageForProcess(uint pid)
        {
            return FindPackage(ProcessManager.GetAppContainerSid(pid));
        }

        public IEnumerator<Package> GetEnumerator()
        {
            return ((IEnumerable<Package>)Packages).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)Packages).GetEnumerator();
        }
    }
}
