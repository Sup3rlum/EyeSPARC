﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
using System.Collections.ObjectModel;

using System.Net;
using Microsoft.Maps.MapControl.WPF;

using EyeAPI;


namespace EyeSPARC
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Network _network = new Network();

        bool _isMapFullTab = false;

        public MainWindow()
        {
            InitializeComponent();


            _network.Load();


            networkTreeView.ItemsSource = new List<Network>() { _network };

        }

        private void NetworkTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (e.NewValue.GetType() == typeof(Station))
            {
                StationTitleLabel.Content = ((Station)e.NewValue).Name;

                var conf = ((Station)e.NewValue);

                float lat = conf.GetStationConfigAttribute<float>("gps_latitude");
                float lon = conf.GetStationConfigAttribute<float>("gps_longitude");
                float alt = conf.GetStationConfigAttribute<float>("gps_altitude");


                LongitudeBoxValue.Text = lon.ToString();
                LatitudeBoxValue.Text = lat.ToString();
                AltitudeBoxValue.Text = alt.ToString();

                stationMap.Center = new Location(lat, lon);
                stationMap.ZoomLevel = 16;
            }

        }

        private void AerialMapButton_Click(object sender, RoutedEventArgs e)
        {
            stationMap.Mode = new AerialMode(true);
        }

        private void RoadMapButton_Click(object sender, RoutedEventArgs e)
        {
            stationMap.Mode = new RoadMode();
        }

        private void FullTabButton_Click(object sender, RoutedEventArgs e)
        {
            if (_isMapFullTab)
            {
                _isMapFullTab = false;

                stationMap.Margin = new Thickness(996, 383, 15, 30);

                FullTabButton.Content = "Expand";
            }
            else
            {
                _isMapFullTab = true;

                stationMap.Margin = new Thickness(308, 214, 15, 30);


                FullTabButton.Content = "Minimize";
            }
        }
    }
}
