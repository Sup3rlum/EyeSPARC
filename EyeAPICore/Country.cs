using System;
using System.Collections.Generic;
using System.Text;

namespace EyeAPICore
{
    public class Country : NetworkNode
    {
        public List<Cluster> Clusters { get; set; }

        public Country()
        {
            Clusters = new List<Cluster>();
        }
    }
}
