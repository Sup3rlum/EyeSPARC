using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Text;

using OxyPlot;
using OxyPlot.Axes;

using EyeAPICore;
using EyeAPICore.Data;

namespace EyeSPARC_StationViewer.Windows
{
    public class DataPlotViewModel
    {
        public string Title { get; private set; }

        public ShowDataType _plotType;
        public ShowDataSheet _dataSheet;

        public ObservableCollection<DataPoint> Channel1 { get; private set; }
        public ObservableCollection<DataPoint> Channel2 { get; private set; }
        public ObservableCollection<DataPoint> Channel3 { get; private set; }
        public ObservableCollection<DataPoint> Channel4 { get; private set; }

        public double Interval { get; set; }

        public DataPlotViewModel(string _title, ShowDataType _plotType)
        {
            this._plotType = _plotType;
            this.Title = _title;


            Interval = 50;

            Channel1 = new ObservableCollection<DataPoint>();
            Channel2 = new ObservableCollection<DataPoint>();
            Channel3 = new ObservableCollection<DataPoint>();
            Channel4 = new ObservableCollection<DataPoint>();
        }

        public async void SetStation(Station _station)
        {

            Channel1.Clear();
            Channel2.Clear();
            Channel3.Clear();
            Channel4.Clear();

            await Task.Run(() => _dataSheet = ShowDB.Query(_station, _plotType));

            // Channel 1

            for (int i = 0; i < _dataSheet.Data[0].Count; i++)
            {
                Channel1.Add(new DataPoint(XFormat(i), _dataSheet.Data[0][i]));
            }

            // Channel 2

            if (_dataSheet.Data.Length >= 2)
            {



                for (int i = 0; i < _dataSheet.Data[1].Count; i++)
                {
                    Channel2.Add(new DataPoint(XFormat(i), _dataSheet.Data[1][i]));
                }
            }

            if (_station.DetectorConfiguration == DetectorConfiguration.MasterAndSlave)
            {



                // Channel 3

                for (int i = 0; i < _dataSheet.Data[2].Count; i++)
                {
                    Channel3.Add(new DataPoint(XFormat(i), _dataSheet.Data[2][i]));
                }

                // Channel 4

                for (int i = 0; i < _dataSheet.Data[3].Count; i++)
                {
                    Channel4.Add(new DataPoint(XFormat(i), _dataSheet.Data[3][i]));
                }
            }
        }
        public double XFormat(double value)
        {
            if (_plotType == ShowDataType.PulseHeight || 
                _plotType == ShowDataType.PulseIntegral ||
                _plotType == ShowDataType.SinglesLow ||
                _plotType == ShowDataType.SinglesHigh)
            {
                return value;
            }
            else
            {
                return TimeSpanAxis.ToDouble(TimeSpan.FromHours(value));
            }
        }
    }
}
