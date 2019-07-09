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

using EyeAPI;


namespace EyeSPARC
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Network net = new Network();
            NetworkMenuItem root = new NetworkMenuItem();
            root.Title = "Network";

            var _clusterData = net.GetClusters();
            var _stationData = net.GetStations();
            var _countryData = net.GetCountries();


            foreach (var k in _countryData)
            {
                root.Items.Add(new CountryMenuItem() { Title = k.Value, ID=k.Key });
            }

            foreach (var k in _clusterData)
            {
                int _countryID = k.Key / 10000;

                root.Items[root.Items.FindIndex(a => a.ID == _countryID * 10000)].Items.Add(new ClusterMenuItem() { Title = k.Value, ID = k.Key });
            }

            foreach (var k in _stationData)
            {
                int _countryID = k.Key / 10000;
                int _clusterID = k.Key / 1000;

                int tmp = root.Items.FindIndex(a => a.ID == _countryID * 10000);

                root.Items[tmp].Items[root.Items[tmp].Items.FindIndex(a => a.ID == _clusterID * 1000)].Items.Add(new StationMenuItem() { Title = k.Value, ID = k.Key });
            }

            networkTreeView.ItemsSource = new List<NetworkMenuItem>() { root };

        }

        private void NetworkTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            StationTitleLabel.Content = ((NodeMenuItem)e.NewValue).Title;
        }
    }
    public class NetworkMenuItem : NodeMenuItem
    {
        public NetworkMenuItem()
        {
            this.Items = new List<CountryMenuItem>();
        }


        public List<CountryMenuItem> Items { get; set; }
    }
    public class CountryMenuItem : NodeMenuItem
    {
        public CountryMenuItem()
        {
            this.Items = new List<ClusterMenuItem>();
        }


        public List<ClusterMenuItem> Items { get; set; }
    }
    public class ClusterMenuItem : NodeMenuItem
    {
        public ClusterMenuItem()
        {
            this.Items = new List<StationMenuItem>();
        }

        public List<StationMenuItem> Items { get; set; }
    }
    public class StationMenuItem : NodeMenuItem
    {

    }

    public class NodeMenuItem
    {
        public string Title { get; set; }
        public int ID { get; set; }
    }
}
