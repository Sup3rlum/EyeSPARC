using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Text.RegularExpressions;
using System.IO;

using Newtonsoft.Json;


namespace EyeAPI
{
    public class Network : NetworkNode
    {
        public List<Country> Countries { get; set; }


        WebClient _wc = new WebClient();

        public Dictionary<int, string> GetApiNode(string node)
        {

            

            Dictionary<int, string> _stationData = new Dictionary<int, string>();

            string _data = _wc.DownloadString($"http://data.hisparc.nl/api/{node}");

            MatchCollection _matches = Regex.Matches(_data, "{\"name\": \"(?<name>[a-zA-Z ]+)\", \"number\": (?<number>[0-9]+)}");


            foreach (Match m in _matches)
            {
                _stationData.Add(Int32.Parse(m.Groups["number"].Value), m.Groups["name"].Value);
            }

            return _stationData;
        }
        public string GetStationConfigJson(int stationId)
        {
            string _dataString = _wc.DownloadString($"http://data.hisparc.nl/api/station/{stationId}/config");

            return _dataString;
        }


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


                var _configData = GetStationConfigJson(k.Key);

                Countries[tmp].Clusters[Countries[tmp].Clusters.FindIndex(a => a.ID == _clusterID * 1000)].Stations.Add(new Station(_configData)
                {
                    Name = k.Value,
                    ID = k.Key,
                }
                );
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
