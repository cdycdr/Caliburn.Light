﻿using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Caliburn.Light;

namespace Demo.ExceptionHandling
{
    public static class EntryPoint
    {
        [STAThread]
        public static void Main()
        {
            try
            {
                AppDomain.CurrentDomain.UnhandledException += (sender, e) => Debug.WriteLine(">>> AppDomain - {0}", e.ExceptionObject);
                TaskScheduler.UnobservedTaskException      += (sender, e) => Debug.WriteLine(">>> TaskScheduler - {0}", e.Exception);
                TaskHelper.TaskFaulted                     += (sender, e) => Debug.WriteLine(">>> TaskFaulted - {0}", e.Task.Exception);
                TaskHelper.TaskWatched                     += (sender, e) => Debug.WriteLine(">>> TaskWatched");

                LogManager.Initialize(t => new DebugLogger(t));

                var app = new App();
                //app.InitializeComponent();

                var bootstrapper = new AppBootstrapper();
                bootstrapper.Initialize();

                app.Run();
            }
            catch (Exception exception)
            {
                Debug.WriteLine(">>> Main - {0}", exception);
            }
        }
    }
}
