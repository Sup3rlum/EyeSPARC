using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeAPI
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
