using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using EyeSPARC.Windows;
using EyeSPARC.Scripting;

namespace EyeSPARC
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
            Environment.LoadConifguration();
            Templates.LoadAll();

            _mainWindow = new MainWindow();
            _splashWindow = new SplashScreenWindow();


            _splashWindow.Show();

            await Task.Run(() =>
            {

                _mainWindow.LoadNetworkItems();
            });

            _splashWindow.Close();
            _mainWindow.Show();


        }
    }
}
