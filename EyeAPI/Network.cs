using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Text.RegularExpressions;


namespace EyeAPI
{
    public class Network : NetworkNode
    {
        public Dictionary<int, string> GetClusters()
        {

            return GetApiNode("clusters");
        }

        public Dictionary<int, string> GetStations()
        {

            return GetApiNode("stations");
        }
        public Dictionary<int, string> GetCountries()
        {

            return GetApiNode("countries");
        }


        public Dictionary<int, string> GetApiNode(string node)
        {

            WebClient _wc = new WebClient();

            Dictionary<int, string> _stationData = new Dictionary<int, string>();

            string _data = _wc.DownloadString($"http://data.hisparc.nl/api/{node}");

            Regex rgx = new Regex("{\"name\": \"(?<name>[a-zA-Z ]+)\", \"number\": (?<number>[0-9]+)}");

            var _matches = rgx.Matches(_data);

            foreach (Match m in _matches)
            {
                _stationData.Add(Int32.Parse(m.Groups["number"].Value), m.Groups["name"].Value);
            }

            return _stationData;
        }

        public List<Country> Countries { get; set; }

        public string Load()
        {
            Countries = new List<Country>();

            var _clusterData = GetApiNode("clusters");
            var _stationData = GetApiNode("stations");
            var _countryData = GetApiNode("countries");


            foreach (var k in _countryData)
            {
                Countries.Add(new Country { Name = k.Value, ID = k.Key });
            }

            foreach (var k in _clusterData)
            {
                int _countryID = k.Key / 10000;

                Countries[Countries.FindIndex(a => a.ID == _countryID * 10000)].Clusters.Add(new Cluster() { Name = k.Value, ID = k.Key });
            }

            foreach (var k in _stationData)
            {
                int _countryID = k.Key / 10000;
                int _clusterID = k.Key / 1000;

                int tmp = Countries.FindIndex(a => a.ID == _countryID * 10000);

                Countries[tmp].Clusters[Countries[tmp].Clusters.FindIndex(a => a.ID == _clusterID * 1000)].Stations.Add(new Station() { Name = k.Value, ID = k.Key });
            }


            this.Name = "Network";
            this.ID = 0;

            return "";
        }

        

    }
    public enum ConnectionStatus
    {
        Available,
        Unavailable,
        Unknown
    }
}
