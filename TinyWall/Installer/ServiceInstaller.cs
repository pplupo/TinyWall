﻿using pylorak.Windows.Services;
using System;
using System.Collections;
using System.Configuration.Install;
using System.ServiceProcess;

namespace pylorak.TinyWall
{
    internal class ServiceInstaller : Installer
    {
        // Service Account Information
        private readonly System.ServiceProcess.ServiceProcessInstaller serviceProcessInstaller = new();
        // Service Information
        private readonly System.ServiceProcess.ServiceInstaller serviceInstaller = new();

        internal ServiceInstaller()
        {
            try
            {
                serviceProcessInstaller.Account = ServiceAccount.LocalSystem;
                serviceProcessInstaller.Username = null;
                serviceProcessInstaller.Password = null;

                serviceInstaller.DisplayName = TinyWallService.ServiceDisplayName;
                serviceInstaller.StartType = ServiceStartMode.Automatic;
                // This must be identical to the WindowsService.ServiceBase name
                // set in the constructor of WindowsService.cs
                serviceInstaller.ServiceName = TinyWallService.SERVICE_NAME;
                // Depends on other services
                serviceInstaller.ServicesDependedOn = TinyWallService.SERVICE_DEPENDENCIES;

                this.Installers.Add(serviceProcessInstaller);
                this.Installers.Add(serviceInstaller);
            }
            catch (Exception e)
            {
                Utils.LogException(e, Utils.LOG_ID_INSTALLER);
            }
        }

        public override void Install(IDictionary stateSaver)
        {
            base.Install(stateSaver);

            try
            {
                using var scm = new ServiceControlManager();
                scm.SetLoadOrderGroup(TinyWallService.SERVICE_NAME, @"NetworkProvider");
            }
            catch { }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Release managed resources

                serviceInstaller.Dispose();
                serviceProcessInstaller.Dispose();
            }

            // Release unmanaged resources.
            // Set large fields to null.
            // Call Dispose on your base class.

            base.Dispose(disposing);
        }
    }
}
