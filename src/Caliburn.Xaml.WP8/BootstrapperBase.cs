﻿using Caliburn.Light;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Navigation;
using Weakly;

namespace Caliburn.Xaml
{
    /// <summary>
    /// Inherit from this class in order to customize the configuration of the framework.
    /// </summary>
    public abstract class BootstrapperBase : IServiceLocator
    {
        private bool _isInitialized;

        /// <summary>
        /// The application.
        /// </summary>
        protected Application Application { get; set; }

        /// <summary>
        /// Initialize the framework.
        /// </summary>
        public void Initialize()
        {
            if (_isInitialized) return;
            _isInitialized = true;

            var inDesignMode = DesignerProperties.IsInDesignTool;
            UIContext.Initialize(inDesignMode, new ViewAdapter());

            if (inDesignMode)
            {
                try
                {
                    StartDesignTime();
                }
                catch
                {
                    //if something fails at design-time, there's really nothing we can do...
                    _isInitialized = false;
                    throw;
                }
            }
            else
            {
                StartRuntime();
            }
        }

        /// <summary>
        /// Called by the bootstrapper's constructor at design time to start the framework.
        /// </summary>
        protected virtual void StartDesignTime()
        {
            TypeResolver.Reset();
            SelectAssemblies().ForEach(TypeResolver.AddAssembly);

            Configure();
            IoC.Initialize(this);
        }

        /// <summary>
        /// Called by the bootstrapper's constructor at runtime to start the framework.
        /// </summary>
        protected virtual void StartRuntime()
        {
            SelectAssemblies().ForEach(TypeResolver.AddAssembly);

            Application = Application.Current;
            PrepareApplication();

            Configure();
            IoC.Initialize(this);
        }

        /// <summary>
        /// Provides an opportunity to hook into the application object.
        /// </summary>
        protected virtual void PrepareApplication()
        {
            Application.Startup += OnStartup;
            Application.UnhandledException += OnUnhandledException;
            Application.Exit += OnExit;

            PhoneService = new PhoneApplicationService();
            PhoneService.Activated += OnActivate;
            PhoneService.Deactivated += OnDeactivate;
            PhoneService.Launching += OnLaunch;
            PhoneService.Closing += OnClose;

            Application.ApplicationLifetimeObjects.Add(PhoneService);

            RootFrame = CreatePhoneApplicationFrame();
            RootFrame.Navigated += OnNavigated;
        }

        /// <summary>
        /// Override to configure the framework and setup your IoC container.
        /// </summary>
        protected virtual void Configure()
        {
        }

        /// <summary>
        /// Override to tell the framework where to find assemblies to inspect for views, etc.
        /// </summary>
        /// <returns>A list of assemblies to inspect.</returns>
        protected virtual IEnumerable<Assembly> SelectAssemblies()
        {
            return new[] {GetType().Assembly};
        }

        /// <summary>
        /// Override this to provide an IoC specific implementation.
        /// </summary>
        /// <param name="service">The service to locate.</param>
        /// <param name="key">The key to locate.</param>
        /// <returns>The located service.</returns>
        public virtual object GetInstance(Type service, string key)
        {
            return Activator.CreateInstance(service);
        }

        /// <summary>
        /// Override this to provide an IoC specific implementation
        /// </summary>
        /// <param name="service">The service to locate.</param>
        /// <returns>The located services.</returns>
        public virtual IEnumerable<object> GetAllInstances(Type service)
        {
            return new[] {GetInstance(service, null)};
        }

        /// <summary>
        /// Override this to provide an IoC specific implementation.
        /// </summary>
        /// <param name="instance">The instance to perform injection on.</param>
        public virtual void InjectProperties(object instance)
        {
        }

        /// <summary>
        /// The phone application service.
        /// </summary>
        protected PhoneApplicationService PhoneService { get; private set; }

        /// <summary>
        /// The root frame used for navigation.
        /// </summary>
        protected PhoneApplicationFrame RootFrame { get; private set; }

        private void OnNavigated(object sender, NavigationEventArgs e)
        {
            if (Application.RootVisual != RootFrame)
                Application.RootVisual = RootFrame;
        }

        /// <summary>
        /// Creates the root frame used by the application.
        /// </summary>
        /// <returns>The frame.</returns>
        protected virtual PhoneApplicationFrame CreatePhoneApplicationFrame()
        {
            return new PhoneApplicationFrame();
        }

        /// <summary>
        /// Override this to add custom behavior to execute after the application starts.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The args.</param>
        protected virtual void OnStartup(object sender, StartupEventArgs e)
        {
        }

        /// <summary>
        /// Override this to add custom behavior on exit.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        protected virtual void OnExit(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Override this to add custom behavior for unhandled exceptions.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        protected virtual void OnUnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
        }

        /// <summary>
        /// Occurs when a fresh instance of the application is launching.
        /// </summary>
        protected virtual void OnLaunch(object sender, LaunchingEventArgs e)
        {
        }

        /// <summary>
        /// Occurs when a previously tombstoned or paused application is resurrected/resumed.
        /// </summary>
        protected virtual void OnActivate(object sender, ActivatedEventArgs e)
        {
        }

        /// <summary>
        /// Occurs when the application is being tombstoned or paused.
        /// </summary>
        protected virtual void OnDeactivate(object sender, DeactivatedEventArgs e)
        {
        }

        /// <summary>
        /// Occurs when the application is closing.
        /// </summary>
        protected virtual void OnClose(object sender, ClosingEventArgs e)
        {
        }
    }
}