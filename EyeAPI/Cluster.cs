﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeAPI
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
