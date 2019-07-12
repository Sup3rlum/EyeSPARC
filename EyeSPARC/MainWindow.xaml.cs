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
using LiveCharts;
using LiveCharts.Wpf;

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

        Station _selectedStation { get; set; }


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
                _selectedStation = ((Station)e.NewValue);

                StationTitleLabel.Text = _selectedStation.Name;


                float lat = _selectedStation.Configuration.GetAttribute<float>("gps_latitude");
                float lon = _selectedStation.Configuration.GetAttribute<float>("gps_longitude");
                float alt = _selectedStation.Configuration.GetAttribute<float>("gps_altitude");


                LongitudeBoxValue.Text = lon.ToString();
                LatitudeBoxValue.Text = lat.ToString();
                AltitudeBoxValue.Text = alt.ToString();

                stationMap.Center = new Location(lat, lon);
                stationMap.ZoomLevel = 16;

                var _latestDetectorStatus = _selectedStation.LatestDetectorStatus;
                var _latestDataStatus = _selectedStation.LatestDataStatus;


                detectorStatusLabel.Text = _latestDetectorStatus.ToString();
                detectorStatusLabel.Foreground = new SolidColorBrush(GetStatusColor(_latestDetectorStatus));
                ssEllipse.Fill = new SolidColorBrush(GetStatusColor(_latestDetectorStatus));

                dataStatusLabel.Text = _latestDataStatus.ToString();
                dataStatusLabel.Foreground = new SolidColorBrush(GetStatusColor(_latestDataStatus));
                dsEllipse.Fill = new SolidColorBrush(GetStatusColor(_latestDataStatus));

                float ch1_volt = _selectedStation.Configuration.GetAttribute<float>("mas_ch1_voltage");
                float ch2_volt = _selectedStation.Configuration.GetAttribute<float>("mas_ch2_voltage");

                label_mas_ch1_volt.Text = ((int)ch1_volt).ToString();
                label_mas_ch2_volt.Text = ((int)ch2_volt).ToString();

                string[] _ver = _selectedStation.Configuration.GetAttribute<string>("mas_version").Replace("\"","").Replace("    ", "-").Split('-');

                string _serial = _ver[0].Split(':')[1];
                string _fpga = _ver[1].Split(':')[1];

                label_mas_ver_fpga.Text = _fpga;
                label_mas_ver_serial.Text = _serial;

                _selectedStation.Query();

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

                stationMap.Margin = new Thickness(704, 393, 15, 30);

                FullTabButton.Content = "Expand";
            }
            else
            {
                _isMapFullTab = true;

                stationMap.Margin = new Thickness(15, 15, 15, 30);


                FullTabButton.Content = "Minimize";
            }
        }

        public Color GetStatusColor(NodeStatus _status)
        {
            switch (_status)
            {
                case NodeStatus.Up:
                    return Colors.LimeGreen;

                case NodeStatus.Down:
                    return Colors.Red;

                case NodeStatus.Issue:
                    return Colors.Orange;

                case NodeStatus.Unknown:
                    return Colors.DarkGray;

                default:
                    return Colors.Black;
            }
        }

        private void ConfMoreButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedStation != null)
            {
                StationConfigWindow _scw = new StationConfigWindow(_selectedStation);
                _scw.Show();
            }
        }
    }
}
