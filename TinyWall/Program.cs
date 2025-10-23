﻿using Microsoft.Extensions.DependencyInjection;
using pylorak.Utilities;
using System;
using System.IO;
using System.Net;
using System.Threading;

namespace pylorak.TinyWall
{
    static class Program
    {
        internal static bool RestartOnQuit { get; set; }

        internal static System.Globalization.CultureInfo? DefaultOsCulture { get; set; }

        private static int StartDevelTool()
        {
            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
            System.Windows.Forms.Application.Run(new DevelToolForm());
            return 0;
        }

        private static void StartService(TinyWallService tw)
        {
#if DEBUG
            if (!Utils.RunningAsAdmin())
            {
                Console.WriteLine(@"Error: Not started as an admin process.");
                return;
            }
#endif

            using var singleInstanceMutex = new Mutex(true, @"Global\TinyWallService", out bool mutexok);

            if (!mutexok)
            {
                return;
            }

#if DEBUG
            tw.Start(Array.Empty<string>());
            tw.StartedEvent.WaitOne();
#else
            pylorak.Windows.Services.ServiceBase.Run(tw);
#endif
        }

        private static int StartController(CmdLineArgs opts)
        {
            // Start controller application
            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);

            do
            {
                RestartOnQuit = false;
                System.Windows.Forms.Application.Run(new TinyWallController(opts));
            } while (RestartOnQuit);

            return 0;
        }

        private static int InstallService()
        {
            return TinyWallDoctor.EnsureServiceInstalledAndRunning(Utils.LOG_ID_INSTALLER, true) ? 0 : -1;
        }

        private static int UninstallService()
        {
            return TinyWallDoctor.Uninstall();
        }

        public static IServiceProvider? ServiceProvider { get; set; }

        private static void ConfigureServices()
        {
            var services = new ServiceCollection();

            //Begin add services to the collection
            //services.AddScoped<IUserService, UserService>();
            //End adding services to the collection

            ServiceProvider = services.BuildServiceProvider();
        }

        public static T? GetService<T>() where T : class
        {
            return (T?)ServiceProvider?.GetService(typeof(T));
        }

        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static int Main(string[] args)
        {
            HierarchicalStopwatch.Enable = File.Exists(Path.Combine(Utils.AppDataPath, "enable-timings"));
            HierarchicalStopwatch.LogFileBase = Path.Combine(Utils.AppDataPath, @"logs\timings");

            DefaultOsCulture ??= Thread.CurrentThread.CurrentUICulture;

            // WerAddExcludedApplication will fail every time we are not running as admin,
            // so wrap it around a try-catch.
            try
            {
                // Prevent Windows Error Reporting running for us
                if (File.Exists(Utils.ExecutablePath))
                    Utils.SafeNativeMethods.WerAddExcludedApplication(Utils.ExecutablePath, true);

            }
            catch
            {
                // ignored
            }

            // Setup TLS 1.2 & 1.3 support, if supported
            if (ServicePointManager.SecurityProtocol != 0)
            {
                try { ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12; }
                catch
                {
                    // ignored
                }

                try { ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls13; }
                catch
                {
                    // ignored
                }
            }

            // Parse comman-line options
            var opts = new CmdLineArgs();
            if (!Environment.UserInteractive || Utils.StringArrayContains(args, "/service"))
                opts.ProgramMode = StartUpMode.Service;
            if (Utils.StringArrayContains(args, "/selfhosted"))
                opts.ProgramMode = StartUpMode.SelfHosted;
            if (Utils.StringArrayContains(args, "/develtool"))
                opts.ProgramMode = StartUpMode.DevelTool;
            if (Utils.StringArrayContains(args, "/install"))
                opts.ProgramMode = StartUpMode.Install;
            if (Utils.StringArrayContains(args, "/uninstall"))
                opts.ProgramMode = StartUpMode.Uninstall;

            if (opts.ProgramMode == StartUpMode.Invalid)
                opts.ProgramMode = StartUpMode.Controller;

            opts.autowhitelist = Utils.StringArrayContains(args, "/autowhitelist");
            opts.updatenow = Utils.StringArrayContains(args, "/updatenow");
            opts.startup = Utils.StringArrayContains(args, "/startup");

#if !DEBUG
            // Register an unhandled exception handler - lol

            void UnhandledException_Gui(object sender, UnhandledExceptionEventArgs e)
            {
                Utils.LogException((Exception)e.ExceptionObject, Utils.LOG_ID_GUI);
            }
            void UnhandledException_Service(object sender, UnhandledExceptionEventArgs e)
            {
                Utils.LogException((Exception)e.ExceptionObject, Utils.LOG_ID_SERVICE);
            }
            void UnhandledException_Installer(object sender, UnhandledExceptionEventArgs e)
            {
                Utils.LogException((Exception)e.ExceptionObject, Utils.LOG_ID_INSTALLER);
            }

            switch (opts.ProgramMode)
            {
                case StartUpMode.Install:
                    AppDomain.CurrentDomain.UnhandledException += UnhandledException_Installer;
                    break;
                case StartUpMode.Uninstall:
                    AppDomain.CurrentDomain.UnhandledException += UnhandledException_Installer;
                    break;
                case StartUpMode.Controller:
                    AppDomain.CurrentDomain.UnhandledException += UnhandledException_Gui;
                    break;
                case StartUpMode.DevelTool:
                    AppDomain.CurrentDomain.UnhandledException += UnhandledException_Gui;
                    break;
                case StartUpMode.SelfHosted:
                    AppDomain.CurrentDomain.UnhandledException += UnhandledException_Gui;
                    AppDomain.CurrentDomain.UnhandledException += UnhandledException_Service;
                    break;
                case StartUpMode.Service:
                    AppDomain.CurrentDomain.UnhandledException += UnhandledException_Service;
                    break;
            }
#endif


            switch (opts.ProgramMode)
            {
                case StartUpMode.Install:
                    return InstallService();
                case StartUpMode.Uninstall:
                    return UninstallService();
                case StartUpMode.Controller:
                    return StartController(opts);
                case StartUpMode.DevelTool:
                    return StartDevelTool();
                case StartUpMode.SelfHosted:
                    using (var srv = new TinyWallService())
                    {
                        StartService(srv);
                        int ret = StartController(opts);
                        srv.Stop();
                        srv.StoppedEvent.WaitOne();
                        return ret;
                    }
                case StartUpMode.Service:
                    using (var srv = new TinyWallService())
                    {
#if !DEBUG
                        pylorak.Windows.PathMapper.Instance.AutoUpdate = false;
#endif
                        StartService(srv);
#if DEBUG
                        Console.WriteLine(@"Kill process to terminate...");
                        srv.StoppedEvent.WaitOne();
#endif
                    }
                    return 0;
                case StartUpMode.Invalid:
                    return -1;
                default:
                    return -1;
            } // switch
        } // Main

    } // class
} //namespace
