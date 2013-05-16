using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using System.Text;

namespace Rimto
{
    public partial class App : Application
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        static App()
        {
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(DomainUnhandledException);
        }

        public App()
        {
            DispatcherUnhandledException += new DispatcherUnhandledExceptionEventHandler(AppUnhandledException);
        }

        ~App()
        {
        }

        protected override void OnStartup(StartupEventArgs args)
        {
            try
            {
                base.OnStartup(args);

                Logger.Info("Application instance starting with {0}...", String.Join(@", ", args.Args));

                HandleAppStartup();
            }
            catch (Exception e)
            {
                Logger.Error("Exception: {0}", e.ToString());
            }
        }

        protected override void OnExit(ExitEventArgs args)
        {
            try
            {
                Logger.Info("Application exiting with {0}.", args.ApplicationExitCode);

                HandleAppExit();

                base.OnExit(args);
            }
            catch (Exception e)
            {
                Logger.Error("Exception: {0}", e.ToString());
            }
        }

        private void HandleAppStartup()
        {
            ShutdownMode = ShutdownMode.OnMainWindowClose;
            MainWindow = new RimtoWindow();
            MainWindow.Show();
        }

        private void HandleAppExit()
        {
        }

        static void DomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = e.ExceptionObject as Exception;
            string message = new StringBuilder()
                .AppendLine(ex.Message)
                .AppendLine().ToString();

            Logger.Error("DOMAIN UNHANDLED EXCEPTION: {0}", ex.ToString());

            MessageBox.Show(message,
                            "DOMAIN UNHANDLED EXCEPTION",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);

            Current.Shutdown(-1);
        }

        static void AppUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Exception ex = e.Exception as Exception;
            string message = new StringBuilder()
                .AppendLine(ex.Message)
                .AppendLine("Continue?")
                .ToString();

            Logger.Error("APP UNHANDLED EXCEPTION: {0}", ex.ToString());

            var res = MessageBox.Show(message,
                                      "APP UNHANDLED EXCEPTION",
                                      MessageBoxButton.YesNo,
                                      MessageBoxImage.Error);

            e.Handled = (res == MessageBoxResult.Yes);

            if (!e.Handled)
            {
                Current.Shutdown(-1);
            }
        }
    }
}
