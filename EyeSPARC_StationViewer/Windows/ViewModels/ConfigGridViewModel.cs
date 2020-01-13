using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Text;

using EyeAPICore;

namespace EyeSPARC_StationViewer.Windows
{
    public class ConfigGridViewModel
    {

        public ListCollectionView CollectionView { get; set; }

        public ConfigGridViewModel(Station _station, DetectorType _t)
        {
            string prefix = "";

            if (_t == DetectorType.Master)
            {
                prefix = "mas";
            }
            else
            {
                prefix = "slv";
            }

            string[] ver = _station.Configuration.GetString($"{prefix}_version").Replace("\"", "").Replace("    ", "-").Split('-');

            string serial = ver[0].Split(':')[1];
            string fpga = ver[1].Split(':')[1];

            ObservableCollection<ConfigEntry> ConfigEntryCollection = new ObservableCollection<ConfigEntry>();

            ConfigEntryCollection.Add(new ConfigEntry("Serial", serial, ConfigType.Mainboard));
            ConfigEntryCollection.Add(new ConfigEntry("FPGA", fpga,  ConfigType.Mainboard));



            ConfigEntryCollection.Add(new ConfigEntry("Voltage", _station.Configuration.GetFloat($"{prefix}_ch1_thres_low").ToString(),  ConfigType.Channel1));
            ConfigEntryCollection.Add(new ConfigEntry("Threshold Low", _station.Configuration.GetFloat($"{prefix}_ch1_thres_low").ToString(), ConfigType.Channel1));
            ConfigEntryCollection.Add(new ConfigEntry("Threshold High", _station.Configuration.GetFloat($"{prefix}_ch1_thres_high").ToString(), ConfigType.Channel1));


            ConfigEntryCollection.Add(new ConfigEntry("Voltage", _station.Configuration.GetFloat($"{prefix}_ch2_thres_low").ToString(), ConfigType.Channel2));
            ConfigEntryCollection.Add(new ConfigEntry("Threshold Low", _station.Configuration.GetFloat($"{prefix}_ch2_thres_low").ToString(), ConfigType.Channel2));
            ConfigEntryCollection.Add(new ConfigEntry("Threshold High", _station.Configuration.GetFloat($"{prefix}_ch2_thres_high").ToString(), ConfigType.Channel2));


            CollectionView = new ListCollectionView(ConfigEntryCollection);
            CollectionView.GroupDescriptions.Add(new PropertyGroupDescription("ConfigType"));
        }
    }
    public class ConfigEntry
    {
        public string Property { get; set; }
        public string Value { get; set; }
        public ConfigType ConfigType { get; set; }

        public ConfigEntry(string p, string v, ConfigType _c)
        {
            Property = p;
            Value = v;
            ConfigType = _c;
        }
    }
    public enum DetectorType
    {
        Master,
        Slave
    }
    public enum ConfigType
    {
        Mainboard,
        Channel1,
        Channel2
    }
}
