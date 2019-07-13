using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using System.Net;

using System.Threading.Tasks;
using System.Collections;

namespace EyeAPI.Data
{
    public class ShowDB
    {
        public static ShowDataSheet Query(Station _station, ShowDataType _type)
        {
            WebClient _wc = new WebClient();

            DateTime _yesterday = DateTime.Now.AddDays(-1);

            string url = $"http://data.hisparc.nl/show/source/{_type.ToString().ToLower()}/{_station.ID}/{_yesterday.Year}/{_yesterday.Month}/{_yesterday.Day}/";

            var _data = _wc.DownloadString(url).Split('\n');

            ShowDataSheet _sheet = new ShowDataSheet(_data[19].Split('\t').Length);

            foreach (var s in _data)
            {
                _sheet.AddRow(IntegerCast(s.Split('\t')).ToArray());
            }
           
            return _sheet;
        }
        public static IEnumerable<int> IntegerCast(string[] values)
        {
            foreach (var s in values)
            {
                yield return Int32.Parse(s);
            }
        }
    }
    public class ShowDataSheet
    {
        public int ColumnCount { get { return _columnCount; } }
        int _columnCount;

        public List<int>[] Data { get; set; }

        public ShowDataSheet(int _num_columns)
        {
            _columnCount = _num_columns;

            Data = new List<int>[_columnCount];

            for (int i = 0; i < _columnCount; i++)
            {
                Data[_columnCount] = new List<int>();
            }
        }
        public void AddRow(params int[] values)
        {
            if (values.Length != _columnCount)
            {
                throw new ArgumentOutOfRangeException();
            }

            for (int i = 0; i < _columnCount; i++)
            {
                Data[i].Add(values[i]);
            }
        }
    }
    public enum ShowDataType
    {
        EventTime,
        PulseHeight,
        PulseIntegral,
        SinglesLow,
        SinglesHigh,
        SinglesRateLow,
        SinglesRateHigh
    }
}
