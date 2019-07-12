using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using System.Net;

using System.Threading.Tasks;

namespace EyeAPI.Data
{
    public class SimpleDB
    {
        public static int[] Query(Station _station, DataType _type)
        {
            WebClient _wc = new WebClient();

            DateTime _yesterday = DateTime.Now.AddDays(-1);

            string url = $"http://data.hisparc.nl/show/source/eventtime/{_station.ID}/{_yesterday.Year}/{_yesterday.Month}/{_yesterday.Day}/";

            var _data = _wc.DownloadString(url).Split('\n');

            return _data.Where(p => p.Length != 0 && p[0] != '#').Select(q => int.Parse(q.Split('\t')[1])).ToArray();
        }
    }
}
