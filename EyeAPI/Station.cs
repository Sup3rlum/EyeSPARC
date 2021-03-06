﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EyeAPI
{
    public class Station : NetworkNode
    {
        public NodeStatus LatestDataStatus = NodeStatus.Unknown;
        public NodeStatus LatestDetectorStatus = NodeStatus.Unknown;
        public DetectorConfiguration DetectorConfiguration = DetectorConfiguration.Master;

        public Configuration Configuration { get; set; }
        

        public Station(string _config)
        {
            Configuration = new Configuration(_config);

            IsSelected = false;

            string slv_ver = Configuration.GetAttribute<string>("slv_version").Replace("\"", "").Replace("    ", "-");

            if (slv_ver == "Hardware: 0-FPGA: 0")
            {
                DetectorConfiguration = DetectorConfiguration.Master;
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
        JObject _jsonObject;
        
        public string JsonString { get; set; }

        public Configuration(string _json)
        {
            _jsonObject = JObject.Parse(_json);
            JsonString = _json;
        }


        public Dictionary<string, string> GetAllAttributes()
        {
            return JsonConvert.DeserializeObject<Dictionary<string, string>>(JsonString);
        }
        public T GetAttribute<T>(string name)
        {
            return _jsonObject.Value<T>(name);
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
        Master,
        MasterAndSlave
    }
}
