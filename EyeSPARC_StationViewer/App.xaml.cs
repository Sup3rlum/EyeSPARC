using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

using EyeSPARC_StationViewer.Windows;

namespace EyeSPARC_StationViewer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        MainWindow _mainWindow;
        SplashScreenWindow _splashWindow;

        private async void Application_Startup(object sender, StartupEventArgs e)
        {


            _mainWindow = new MainWindow();
            _splashWindow = new SplashScreenWindow();


            _splashWindow.Show();

            await Task.Run(() =>
            {

                //_mainWindow.LoadNetworkItems();

            });

            _splashWindow.Close();
            _mainWindow.Show();


        }
    }
}
