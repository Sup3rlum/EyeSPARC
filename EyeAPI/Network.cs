using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Text.RegularExpressions;


namespace EyeAPI
{
    public class Network
    {
        public Dictionary<int, string> GetClusters()
        {

            Dictionary<int, string> _stationData = new Dictionary<int, string>();

            string _data = " ";//_wc.DownloadString("http://data.hisparc.nl/api/clusters/");

            Regex rgx = new Regex("{\"name\": \"(?<name>[a-zA-Z ]+)\", \"number\": (?<number>[0-9]+)}");

            var _matches = rgx.Matches(_data);

            foreach (Match m in _matches)
            {
                _stationData.Add(Int32.Parse(m.Groups["name"].Value), m.Groups["number"].Value);
            }

            return _stationData;


        }
    }
    public enum ConnectionStatus
    {
        Available,
        Unavailable,
        Unknown
    }
}
