using System;
using System.Collections.Generic;
using System.Text;

namespace EyeAPICore
{
    public class Cluster : NetworkNode
    {
        public List<Station> Stations { get; set; }

        public Cluster()
        {
            Stations = new List<Station>();
        }
    }
}
