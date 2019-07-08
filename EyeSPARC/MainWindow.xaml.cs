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

            networkTreeView.ItemsSource = new List<NetworkMenuItem>() { root };





        }
    }
    public class NetworkMenuItem : MenuItem
    {
        public NetworkMenuItem()
        {
            this.Items = new ObservableCollection<ClusterMenuItem>();
        }

        public string Title { get; set; }

        public ObservableCollection<ClusterMenuItem> Items { get; set; }
    }
    public class ClusterMenuItem
    {
        public ClusterMenuItem()
        {
            this.Items = new ObservableCollection<StationMenuItem>();
        }

        public string Title { get; set; }
        public string Country { get; set; }
        public int ID { get; set; }

        public ObservableCollection<StationMenuItem> Items { get; set; }
    }
    public class StationMenuItem
    {
        public string Title { get; set; }
        public int ID { get; set; }
    }
}
