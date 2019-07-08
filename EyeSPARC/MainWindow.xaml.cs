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
using System.Collections.ObjectModel;


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

            NetworkMenuItem root = new NetworkMenuItem();
            root.Title = "Network";

            ClusterMenuItem _cluster = new ClusterMenuItem();

            _cluster.Title = "London";
            _cluster.ID = 17;

            StationMenuItem station = new StationMenuItem();

            station.Title = "IOP";
            station.ID = 17001;


            _cluster.Items.Add(station);

            root.Items.Add(_cluster);

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
        public int ID { get; set; }

        public ObservableCollection<StationMenuItem> Items { get; set; }
    }
    public class StationMenuItem
    {
        public string Title { get; set; }
        public int ID { get; set; }
    }
}
