using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EyeAPI
{
    public class Station : NetworkNode
    {
        public NodeStatus LatestDataStatus = NodeStatus.Unknown;
        public NodeStatus LatestDetectorStatus = NodeStatus.Unknown;


        JObject _jsonObject;

        public Station(string _config)
        {
            _jsonObject = JObject.Parse(_config);

        }

        public T GetStationConfigAttribute<T>(string name)
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
    
}
