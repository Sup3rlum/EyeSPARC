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
        public StationDataStatus LatestDataStatus = StationDataStatus.Unknown;
        public StationStatus LatestStatus = StationStatus.Unknown;


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

    public enum StationStatus
    {
        Up,
        Issue,
        Down,
        Unknown
    }
    public enum StationDataStatus
    {
        Up,
        Issue,
        Down,
        Unknown
    }

    
}
