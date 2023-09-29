using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI.WindowManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MultiThreadApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void btnCreateView_Click(object sender, RoutedEventArgs e)
        {
            ApplicationView appWindow = null;
            Frame rootFrame = null;

            var view = CoreApplication.CreateNewView();
            await view.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, (Windows.UI.Core.DispatchedHandler)(() => 
            {
                appWindow = ApplicationView.GetForCurrentView();
                appWindow.Consolidated += AppWindow_Consolidated;
                ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(400, 300));

                rootFrame = new Frame();
                Window.Current.Closed += Current_Closed;
                Window.Current.Content = rootFrame;
                Window.Current.Activate();
            }));

            await ApplicationViewSwitcher.TryShowAsStandaloneAsync(
                appWindow.Id,
                ViewSizePreference.Default,
                appWindow.Id,
                ViewSizePreference.Default);

            await ApplicationViewSwitcher.SwitchAsync(appWindow.Id);

            await view.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                rootFrame.Navigate(typeof(ViewPage));
            });
        }

        private void AppWindow_Consolidated(ApplicationView sender, ApplicationViewConsolidatedEventArgs args)
        {
            var window = Window.Current;
            if (window != null)
            {
                window.Dispatcher.StopProcessEvents();
                window.Close();
            }
        }

        private void Current_Closed(object sender, Windows.UI.Core.CoreWindowEventArgs e)
        {
            // After shutting down window ASTA thread is still in the background waiting.
        }
    }
}
