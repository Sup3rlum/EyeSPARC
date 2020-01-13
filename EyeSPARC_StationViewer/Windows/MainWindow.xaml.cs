using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

using EyeAPICore;
using EyeAPICore.Data;


using OxyPlot;

using GMap.NET;
using GMap.NET.MapProviders;

namespace EyeSPARC_StationViewer.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public Station _selectedStation { get; set; }

        Network _network = new Network();


        DataPlotViewModel _eventTimeModel;
        DataPlotViewModel _pulseHeightModel;
        DataPlotViewModel _pulseIntegralModel;
        DataPlotViewModel _singlesLowModel;
        DataPlotViewModel _singlesHighModel;
        DataPlotViewModel _singlesRateLowModel;
        DataPlotViewModel _singlesRateHighModel;


        public MainWindow()
        {
            InitializeComponent();

            _eventTimeModel = new DataPlotViewModel("Event Time", ShowDataType.EventTime);
            _pulseHeightModel = new DataPlotViewModel("Pulse Height", ShowDataType.PulseHeight);
            _pulseIntegralModel = new DataPlotViewModel("Pulse Integral", ShowDataType.PulseIntegral);
            _singlesLowModel = new DataPlotViewModel("Single rate histogram (per second above low threshold)", ShowDataType.SinglesLow);
            _singlesHighModel = new DataPlotViewModel("Single rate histogram (per second above high threshold)", ShowDataType.SinglesHigh);
            _singlesRateLowModel = new DataPlotViewModel("Singles (per second above high threshold)", ShowDataType.SinglesRateLow);
            _singlesRateHighModel = new DataPlotViewModel("Singles (per second above high threshold)", ShowDataType.SinglesRateHigh);

            _eventTimePlot.DataContext = _eventTimeModel;
            _pulseHeightPlot.DataContext = _pulseHeightModel;
            _pulseIntegralPlot.DataContext = _pulseIntegralModel;
            _singlesLowPlot.DataContext = _singlesLowModel;
            _singlesHighPlot.DataContext = _singlesHighModel;
            _singlesRateLowPlot.DataContext = _singlesRateLowModel;
            _singlesRateHighPlot.DataContext = _singlesRateHighModel;
        }
        private void AboutMenu_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow _about = new AboutWindow();
            _about.Show();
        }
        public void LoadNetworkItems()
        {
            _network.Load();

            Dispatcher.Invoke(() => networkTreeView.ItemsSource = new List<Network>() { _network });

        }
        private async void NetworkTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (e.NewValue.GetType() == typeof(Station))
            {
                _selectedStation = ((Station)e.NewValue);

                StationTitleLabel.Text = _selectedStation.Name;
                StationIDLabel.Text = _selectedStation.ID.ToString();


                float lat = _selectedStation.Configuration.GetFloat("gps_latitude");
                float lon = _selectedStation.Configuration.GetFloat("gps_longitude");
                float alt = _selectedStation.Configuration.GetFloat("gps_altitude");

                mapControl.Position = new PointLatLng(lat, lon);



                var _latestDetectorStatus = _selectedStation.LatestDetectorStatus;
                var _latestDataStatus = _selectedStation.LatestDataStatus;


                detectorStatusLabel.Text = _latestDetectorStatus.ToString();
                detectorStatusLabel.Foreground = new SolidColorBrush(GetStatusColor(_latestDetectorStatus));
                ssEllipse.Fill = new SolidColorBrush(GetStatusColor(_latestDetectorStatus));

                dataStatusLabel.Text = _latestDataStatus.ToString();
                dataStatusLabel.Foreground = new SolidColorBrush(GetStatusColor(_latestDataStatus));
                dsEllipse.Fill = new SolidColorBrush(GetStatusColor(_latestDataStatus));


                float mas_ch1_volt = _selectedStation.Configuration.GetFloat("mas_ch1_voltage");
                float mas_ch2_volt = _selectedStation.Configuration.GetFloat("mas_ch2_voltage");


                float slv_ch1_volt = _selectedStation.Configuration.GetFloat("slv_ch1_voltage");
                float slv_ch2_volt = _selectedStation.Configuration.GetFloat("slv_ch2_voltage");


            //    label_mas_ch1_volt.Text = mas_ch1_volt.ToString();





                /*   label_mas_ver_fpga.Text = mas_fpga;
                   label_mas_ver_serial.Text = mas_serial;

                   label_slv_ver_fpga.Text = slv_fpga;
                   label_slv_ver_serial.Text = slv_serial;*/




                ConfigGridViewModel cgvm = new ConfigGridViewModel(_selectedStation, DetectorType.Master);
                ConfigGridViewModel cgvm2 = new ConfigGridViewModel(_selectedStation, DetectorType.Slave);

                dgMaster.DataContext = cgvm;
                dgSlave.DataContext = cgvm2;


                _eventTimeModel.SetStation(_selectedStation);
                _pulseHeightModel.SetStation(_selectedStation);
                _pulseIntegralModel.SetStation(_selectedStation);
                _singlesLowModel.SetStation(_selectedStation);
                _singlesHighModel.SetStation(_selectedStation);
                _singlesRateLowModel.SetStation(_selectedStation);
                _singlesRateHighModel.SetStation(_selectedStation);


                if (_selectedStation.DetectorConfiguration == DetectorConfiguration.MasterOnly)
                {
                    dconfEllipse.Fill = Brushes.DarkSlateBlue;
                    dconfLabel.Foreground = Brushes.DarkSlateBlue;
                    dconfLabel.Text = "Master Only";
                }
                else
                {
                    dconfEllipse.Fill = Brushes.Maroon;
                    dconfLabel.Foreground = Brushes.Maroon;
                    dconfLabel.Text = "Master And Slave";
                }
            }

        }

        private void mapControl_Loaded(object sender, RoutedEventArgs e)
        {
            GMaps.Instance.Mode = AccessMode.ServerAndCache;
            mapControl.MapProvider = GMapProviders.BingMap;

            mapControl.Zoom = 12;
            mapControl.ShowCenter = false;
        }

        public Color GetStatusColor(NodeStatus _status) =>
            _status switch
            {
                NodeStatus.Up       => Colors.LimeGreen,
                NodeStatus.Down     => Colors.Red,
                NodeStatus.Issue    => Colors.Orange,
                NodeStatus.Unknown  => Colors.DarkGray,

                _ => Colors.Black
            };

    }
}
