using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
using Microsoft.Win32;

using EyeAPICore;

namespace EyeSPARC_StationViewer.Windows
{
    public class StationConfigPropery
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
    }

    public partial class StationConfigWindow : Window
    {
        Station _selectedStation;

        public StationConfigWindow(Station _station)
        {
            InitializeComponent();

            var attributes = _station.Configuration.GetAllAttributes();


            _selectedStation = _station;

            List<StationConfigPropery> _list = new List<StationConfigPropery>();

            foreach (var v in attributes)
            {
                _list.Add(new StationConfigPropery() { Name = v.Key, Value = v.Value, Description = "" });
            }

            dgConfig.ItemsSource = _list;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.FileName = $"station_{_selectedStation.ID}_config";
            dlg.DefaultExt = ".json";
            dlg.Filter = "JSON Document|*.json";

            bool? result = dlg.ShowDialog();

            if (result == true)
            {
                File.WriteAllText(dlg.FileName, _selectedStation.Configuration.JsonString);
            }
        }
    }
}
