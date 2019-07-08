using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMA
{
    public class Station
    {
        int ID;
        string Name;
        StationDataStatus LatestDataStatus;
        StationStatus LatestStatus;
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
