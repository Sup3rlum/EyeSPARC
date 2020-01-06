using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Net;



namespace EyeAPICore
{
    public class Station : NetworkNode
    {
        public NodeStatus LatestDataStatus = NodeStatus.Unknown;
        public NodeStatus LatestDetectorStatus = NodeStatus.Unknown;
        public DetectorConfiguration DetectorConfiguration = DetectorConfiguration.MasterOnly;

        public Configuration Configuration { get; set; }


        public Station(string _config)
        {
            Configuration = new Configuration(_config);

            IsSelected = false;

            string slv_ver = Configuration.GetString("slv_version").Replace("\"", "").Replace("    ", "-");

            if (slv_ver == "Hardware: 0-FPGA: 0")
            {
                DetectorConfiguration = DetectorConfiguration.MasterOnly;
            }
            else
            {
                DetectorConfiguration = DetectorConfiguration.MasterAndSlave;
            }
        }

        public bool IsSelected { get; set; }
    }

    public class Configuration
    {
        JsonDocument _jsonObject;

        public string JsonString { get; set; }

        public Configuration(string _json)
        {
            _jsonObject = JsonDocument.Parse(_json);
            JsonString = _json;
        }


        public Dictionary<string, string> GetAllAttributes()
        {
            return JsonSerializer.Deserialize<Dictionary<string, string>>(JsonString);
        }

        public float GetFloat(string name)
        {
            return _jsonObject.RootElement.GetProperty(name).GetSingle();
        }
        public string GetString(string name)
        {
            return _jsonObject.RootElement.GetProperty(name).GetString();
        }
    }

    public enum NodeStatus
    {
        Up,
        Issue,
        Down,
        Unknown
    }
    public enum DetectorConfiguration
    {
        MasterOnly,
        MasterAndSlave
    }
}
