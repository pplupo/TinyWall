﻿using System.ComponentModel;
using System.Configuration.Install;

namespace pylorak.TinyWall
{
    [RunInstaller(true)]
    public class TinyWallInstaller : Installer
    {
        public TinyWallInstaller()
        {
            this.Installers.Add(new ServiceInstaller());
        }
    }
}