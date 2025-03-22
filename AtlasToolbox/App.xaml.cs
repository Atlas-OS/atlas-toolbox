using Microsoft.UI.Xaml;
using AtlasToolbox.HostBuilder;
using Microsoft.Extensions.Hosting;
using AtlasToolbox.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using System;
using System.IO.Pipes;
using System.IO;
using WinUIEx;
using System.Threading;
using NLog;
using NLog.Config;
using NLog.Targets;
using System.Configuration;
using AtlasToolbox.Utils;

namespace AtlasToolbox
{
    public partial class App : Application
    {
        public static readonly Logger logger = LogManager.GetCurrentClassLogger();
        public static IHost _host { get; set; }

        public static Window m_window;
        public static Window s_window;
        public static Window f_window;
        public static XamlRoot XamlRoot { get; set; }
        public static string CurrentCategory { get; set; }

        private static Mutex _mutex = new(true, "{AtlasToolbox}");

        public App()
        {
            ConfigureNLog();
            logger.Info("App Started");
            _host = CreateHostBuilder().Build();
            logger.Info("Building host");
            _host.Start();
            logger.Info("Starting host");
            this.InitializeComponent();
            logger.Info("Finished initializing components");
            this.UnhandledException += OnAppUnhandledException;
        }

        /// <summary>
        /// Registers all configuration services
        /// </summary>
        /// <returns></returns>
        private static IHostBuilder CreateHostBuilder() =>
            Host.CreateDefaultBuilder()
                .AddStores()
                .AddServices()
                .AddViewModels();

        /// <summary>
        /// Configures NLog for logging
        /// </summary>
        private void ConfigureNLog()
        {
            string name = $"logs/toolbox-log-{DateTime.Now.Year}_{DateTime.Now.Month}_{DateTime.Now.Day}_{DateTime.Now.Hour}_{DateTime.Now.Minute}_{DateTime.Now.Second}.log";
            var config = new LoggingConfiguration();
            var logfile = new FileTarget("logfile")
            {
                FileName = name,
                Layout = "${longdate} ${level}: ${message} ${exception}"
            };
            config.AddTarget(logfile); config.AddRuleForAllLevels(logfile);
            LogManager.Configuration = config;
        }

        /// <summary>
        /// Catches unhandled exceptions and logs them
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAppUnhandledException(object sender, Microsoft.UI.Xaml.UnhandledExceptionEventArgs e)
        {
            logger.Error(e.Exception, "Unhandled exception occurred");
        }
        
        /// <summary>
        /// App behavior on launch
        /// </summary>
        /// <param name="args"></param>
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            if (CompatibilityHelper.IsCompatible())
            {
                Task.Run(() => StartNamedPipeServer());

                if (!_mutex.WaitOne(TimeSpan.Zero, true))
                {
                    CheckForExistingInstance();
                    Environment.Exit(0);
                    return;
                }

                string[] arguments = Environment.GetCommandLineArgs();
                bool wasRanWithArgs = false;
                // planned for args but they are not currently used
                //foreach (var arg in arguments)
                //{
                //    if (arg.StartsWith("-"))
                //    {
                //        switch (arg)
                //        {
                //            case "-silent":
                //                InitializeVMSilent();
                //                wasRanWithArgs = true;
                //                break;
                //            case "-toforeground":
                //                m_window.Show();
                //                wasRanWithArgs = true;
                //                break;
                //            case "-runEnabled":
                //                break;
                //            case "-runDefaults":
                //                break;
                //        }
                //    }
                //}
                if (!wasRanWithArgs)
                {
                    logger.Info("Loading without args");
                    s_window = new LoadingWindow();
                    s_window.Activate();

                    InitializeVMAsync();
                }
            }
            else
            {
                m_window = new IncompatibleVersionWindow();
                m_window.Activate();
            }
        }
        /// <summary>
        /// Checks for an existing toolbox instance in processes
        /// </summary>
        private void CheckForExistingInstance()
        {
            try
            {
                using (var client = new NamedPipeClientStream(".", "pipe", PipeDirection.Out))
                {
                    client.Connect(1000);
                    using (var writer = new StreamWriter(client))
                    {
                        writer.WriteLine("-toforeground");
                        writer.Flush();
                    }
                }
                Environment.Exit(0);
            }
            catch (Exception ex)
            { System.Diagnostics.Debug.WriteLine($"Error: {ex.Message}"); }
        }

        /// <summary>
        /// Start the pipe server and waits for a connection
        /// </summary>
        private void StartNamedPipeServer()
        {
            while (true)
            {
                using (var server = new NamedPipeServerStream("pipe", PipeDirection.In))
                {
                    server.WaitForConnection();
                    using (var reader = new StreamReader(server))
                    {
                        if (reader.ReadLine() == "-toforeground")
                        {
                            m_window.DispatcherQueue.TryEnqueue(() =>
                            {
                                m_window.Activate();
                            });
                        }
                    }
                }
            }
        }
        
        //private void InitializeVMSilent()
        //{
        //    _host.Services.GetRequiredService<ConfigPageViewModel>();
        //}

        /// <summary>
        /// Starts the program and get all the required services for a faster load time
        /// </summary>
        private async void InitializeVMAsync()
        {
            logger.Info("Loading configuration services");
            await Task.Run(() => _host.Services.GetRequiredService<ConfigPageViewModel>());
            logger.Info("Configuration services loaded");

            m_window = new MainWindow();
            m_window.Activate();

            s_window.Close();
        }

        /// <summary>
        /// Calls a content dialog
        /// </summary>
        /// <param name="type">type of content dialog</param>
        public static void ContentDialogCaller(string type) 
        {
            var mainWindow = m_window as MainWindow;
            mainWindow.ContentDialogContoller(type);
        }
    }
}
