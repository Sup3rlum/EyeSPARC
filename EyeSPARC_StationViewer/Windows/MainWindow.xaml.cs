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

namespace EyeSPARC_StationViewer.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        Station _selectedStation;

        Network _network = new Network();


        ShowDataSheet _eventtime, _pulseheight, _pulseintegral, _singleslow, _singleshigh, _singlesratelow, _singlesratehigh;


        public MainWindow()
        {
            InitializeComponent();
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


                float lat = _selectedStation.Configuration.GetFloat("gps_latitude");
                float lon = _selectedStation.Configuration.GetFloat("gps_longitude");
                float alt = _selectedStation.Configuration.GetFloat("gps_altitude");


                LongitudeBoxValue.Text = lon.ToString();
                LatitudeBoxValue.Text = lat.ToString();
                AltitudeBoxValue.Text = alt.ToString();


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

                label_mas_ch1_volt.Text = ((int)mas_ch1_volt).ToString();
                label_mas_ch2_volt.Text = ((int)mas_ch2_volt).ToString();

                label_slv_ch1_volt.Text = ((int)slv_ch1_volt).ToString();
                label_slv_ch2_volt.Text = ((int)slv_ch2_volt).ToString();

                string[] mas_ver = _selectedStation.Configuration.GetString("mas_version").Replace("\"", "").Replace("    ", "-").Split('-');
                string[] slv_ver = _selectedStation.Configuration.GetString("slv_version").Replace("\"", "").Replace("    ", "-").Split('-');

                string mas_serial = mas_ver[0].Split(':')[1];
                string mas_fpga = mas_ver[1].Split(':')[1];

                string slv_serial = slv_ver[0].Split(':')[1];
                string slv_fpga = slv_ver[1].Split(':')[1];


                label_mas_ver_fpga.Text = mas_fpga;
                label_mas_ver_serial.Text = mas_serial;

                label_slv_ver_fpga.Text = slv_fpga;
                label_slv_ver_serial.Text = slv_serial;

                float mas_ch1_tresh_low = _selectedStation.Configuration.GetFloat("mas_ch1_thres_low");
                float mas_ch1_tresh_high = _selectedStation.Configuration.GetFloat("mas_ch1_thres_high");

                float mas_ch2_tresh_low = _selectedStation.Configuration.GetFloat("mas_ch2_thres_low");
                float mas_ch2_tresh_high = _selectedStation.Configuration.GetFloat("mas_ch2_thres_high");


                float slv_ch1_tresh_low = _selectedStation.Configuration.GetFloat("slv_ch1_thres_low");
                float slv_ch1_tresh_high = _selectedStation.Configuration.GetFloat("slv_ch1_thres_high");

                float slv_ch2_tresh_low = _selectedStation.Configuration.GetFloat("slv_ch2_thres_low");
                float slv_ch2_tresh_high = _selectedStation.Configuration.GetFloat("slv_ch2_thres_high");

                label_mas_ch1_treshold_low.Text = ((int)mas_ch1_tresh_low).ToString();
                label_mas_ch1_treshold_high.Text = ((int)mas_ch1_tresh_high).ToString();

                label_mas_ch2_treshold_low.Text = ((int)mas_ch1_tresh_low).ToString();
                label_mas_ch2_treshold_high.Text = ((int)mas_ch1_tresh_high).ToString();

                await Task.Run(() => DownloadShowData(_selectedStation));

               // DisplayShowData();

               /* YFormatter = value => value.ToString();
                XFormatter = value => TimeSpan.FromHours((double)value).ToString("hh:mm");*/


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
        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (eventtimeTab.IsSelected)
            {
                chartTitle.Text = "Event count histogram";
            }
            else if (pulseheightsTab.IsSelected)
            {
                chartTitle.Text = "Pulseheight histogram";

            }
            else if (pulseintegralTab.IsSelected)
            {
                chartTitle.Text = "Pulseintegral histogram";
            }
            else if (singleslowTab.IsSelected)
            {
                chartTitle.Text = "Singles (per second above low threshold)";

            }
            else if (singleshighTab.IsSelected)
            {
                chartTitle.Text = "Singles (per second above high threshold)";
            }
            else if (singlesratelowTab.IsSelected)
            {
                chartTitle.Text = "Singles rate histogram (above low threshold)";
            }
            else if (singlesratehighTab.IsSelected)
            {
                chartTitle.Text = "Singles rate histogram (above high threshold)";
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
