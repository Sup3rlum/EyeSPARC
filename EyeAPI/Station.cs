using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeAPI
{
    public class Station : NetworkNode
    {
        public StationDataStatus LatestDataStatus = StationDataStatus.Unknown;
        public StationStatus LatestStatus = StationStatus.Unknown;

        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public float Altitude { get; set; }



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
