using System;
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
using LiveCharts.Configurations;
using LiveCharts.Defaults;

using EyeAPI;
using EyeAPI.Data;


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

        public SeriesCollection _evSeriesCollection { get; set; }
        public SeriesCollection _phSeriesCollection { get; set; }
        public SeriesCollection _piSeriesCollection { get; set; }
        public SeriesCollection _slSeriesCollection { get; set; }
        public SeriesCollection _shSeriesCollection { get; set; }
        public SeriesCollection _srlSeriesCollection { get; set; }
        public SeriesCollection _srhSeriesCollection { get; set; }

        public Func<int, string> YFormatter { get; set; }
        public Func<int, string> XFormatter { get; set; }



        public MainWindow()
        {
            InitializeComponent();

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

                await Task.Run(() => DownloadShowData(_selectedStation));

                DisplayShowData();

                YFormatter = value => value.ToString();
                XFormatter = value => TimeSpan.FromHours((double)value).ToString("hh:mm");


                latestDataChart.DataContext = this;
                latestDataChart.DataTooltip.Background = Brushes.DarkGray;
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
                stationMap.Margin = new Thickness(790, 378, 15, 45);

                FullTabButton.Content = "Expand";
            }
            else
            {
                _isMapFullTab = true;
                stationMap.Margin = new Thickness(15, 15, 15, 45);

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

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (eventtimeTab.IsSelected)
            {
                latestDataChart.Series = _evSeriesCollection;
                chartTitle.Text = "Event count histogram";
            }
            else if (pulseheightsTab.IsSelected)
            {
                latestDataChart.Series = _phSeriesCollection;
                chartTitle.Text = "Pulseheight histogram";

            }
            else if (pulseintegralTab.IsSelected)
            {
                latestDataChart.Series = _piSeriesCollection;
                chartTitle.Text = "Pulseintegral histogram";
            }
            else if (singleslowTab.IsSelected)
            {
                latestDataChart.Series = _slSeriesCollection;
                chartTitle.Text = "Singles (per second above low treshold)";

            }
            else if (singleshighTab.IsSelected)
            {
                latestDataChart.Series = _shSeriesCollection;
                chartTitle.Text = "Singles (per second above high treshold)";
            }
            else if (singlesratelowTab.IsSelected)
            {
                latestDataChart.Series = _srlSeriesCollection;
                chartTitle.Text = "Singles rate histogram (above low treshold)";
            }
            else if (singlesratehighTab.IsSelected)
            {
                latestDataChart.Series = _srhSeriesCollection;
                chartTitle.Text = "Singles rate histogram (above high treshold)";
            }
        }
        public void DownloadShowData(Station _st)
        {
            _eventtime = ShowDB.Query(_selectedStation, ShowDataType.EventTime);
            _pulseheight = ShowDB.Query(_selectedStation, ShowDataType.PulseHeight);
            _pulseintegral = ShowDB.Query(_selectedStation, ShowDataType.PulseIntegral);

            _singleslow = ShowDB.Query(_selectedStation, ShowDataType.SinglesLow);
            _singleshigh = ShowDB.Query(_selectedStation, ShowDataType.SinglesHigh);

            _singlesratelow = ShowDB.Query(_selectedStation, ShowDataType.SinglesRateLow);
            _singlesratehigh = ShowDB.Query(_selectedStation, ShowDataType.SinglesRateHigh);
        }
        public void DisplayShowData()
        {
            _evSeriesCollection = new SeriesCollection();
            _phSeriesCollection = new SeriesCollection();
            _piSeriesCollection = new SeriesCollection();
            _slSeriesCollection = new SeriesCollection();
            _shSeriesCollection = new SeriesCollection();
            _srlSeriesCollection = new SeriesCollection();
            _srhSeriesCollection = new SeriesCollection();

            // Event Time
            _evSeriesCollection.Add(new StepLineSeries
            {
                Title = "Average Event Count",
                Values = new ChartValues<int>(_eventtime.Data[0]),
                PointGeometry = null,
                StrokeThickness = 1
            });

            // Pulsehieghts

            for (int i = 0; i < _pulseheight.Data.Length;i++)
            {

                _phSeriesCollection.Add(new StepLineSeries
                {
                    Title = $"Pulseheights Channel {i+1}",
                    Values = new ChartValues<int>(_pulseheight.Data[i]),
                    PointGeometry = null
                });
            }

            // Pulseintegral
            for (int i = 0; i < _pulseintegral.Data.Length; i++)
            {

                _piSeriesCollection.Add(new StepLineSeries
                {
                    Title = $"Pulseintegral Channel {i + 1}",
                    Values = new ChartValues<int>(_pulseintegral.Data[i]),
                    PointGeometry = null
                });
            }
            // Singles Low 
            for (int i = 0; i < _singleslow.Data.Length; i++)
            {

                _slSeriesCollection.Add(new StepLineSeries
                {
                    Title = "Singles Low",
                    Values = new ChartValues<int>(_singleslow.Data[i]),
                    PointGeometry = null
                });
            }

            // Singles High
            for (int i = 0; i < _singleshigh.Data.Length; i++)
            {

                _shSeriesCollection.Add(new StepLineSeries
                {
                    Title = "Singles High",
                    Values = new ChartValues<int>(_singleshigh.Data[i]),
                    PointGeometry = null
                });
            }

            // Singles Rates Low 
            for (int i = 0; i < _singlesratelow.Data.Length; i++)
            {

                _srlSeriesCollection.Add(new StepLineSeries
                {
                    Title = "Singles Rate  Low",
                    Values = new ChartValues<int>(_singlesratelow.Data[i]),
                    PointGeometry = null
                });
            }
            // Singles Rates High

            for (int i = 0; i < _singlesratehigh.Data.Length; i++)
            {

                _srhSeriesCollection.Add(new StepLineSeries
                {
                    Title = "Singles Rate High",
                    Values = new ChartValues<int>(_singlesratehigh.Data[i]),
                    PointGeometry = null
                });
            }

        }

        ShowDataSheet _eventtime, _pulseheight, _pulseintegral, _singleslow, _singleshigh, _singlesratelow, _singlesratehigh;
    }
}
