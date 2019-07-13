using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using System.Net;

using System.Threading.Tasks;

namespace EyeAPI.Data
{
    public class PublicDB
    {
        public static DataSheet Query(Station _station, PublicDataType _type, DateTime _start, DateTime _end)
        {

            DataSheet _sheet = new DataSheet($"{_station.ID}_{_type}_{_start}_{_end}", _type);


            WebClient _wc = new WebClient();


            string url = $"http://data.hisparc.nl/data/{_station.ID}/";

            if (_type == PublicDataType.Events)
                url += "events/";


            var _data = _wc.DownloadString(url);
            var _lines = _data.Split('\n').Where(p => p[0] != '#');


            foreach (string s in _lines)
            {
                var d = s.Split('\t');

                _sheet.AddRow
                    (
                        ulong.Parse(d[2]), // Unix Timestamp
                        ulong.Parse(d[3]), // ns Timestamp

                        int.Parse(d[4]), // ph1
                        int.Parse(d[5]), // ph2
                        int.Parse(d[6]), // ph3
                        int.Parse(d[7]), // ph4

                        int.Parse(d[8]), // int1
                        int.Parse(d[9]), // int2
                        int.Parse(d[10]), // int3
                        int.Parse(d[11]), // int4

                        float.Parse(d[12]), // mips1
                        float.Parse(d[13]), // mips2
                        float.Parse(d[14]), // mips3
                        float.Parse(d[15]), // mips4

                        float.Parse(d[16]), // arr1
                        float.Parse(d[17]), // arr2
                        float.Parse(d[18]), // arr3
                        float.Parse(d[19]), // arr4

                        float.Parse(d[20]), // trigger
                        float.Parse(d[21]), // zenith
                        float.Parse(d[22]) // azimuth

                    );
            }
            
            return _sheet;
        } 

        public static DataSheet Query(Station _station, PublicDataType _type)
        {
            return Query(_station, _type, DateTime.Today.AddDays(-1), DateTime.Today);
        }
    }
    public enum PublicDataType
    {
        Events,
        Weather,
        Coincidences
    }
}