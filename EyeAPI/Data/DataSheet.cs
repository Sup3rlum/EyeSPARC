using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Data.Sql;

namespace EyeAPI
{
    public class EventDataSheet
    {
        DataTable _tableInternal;

        DateTime _start;
        DateTime _end;

        public EventDataSheet(string name)
        {
            _tableInternal = new DataTable(name);

            _tableInternal.Columns.Add(new DataColumn("Date", typeof(DateTime)));
            _tableInternal.Columns.Add(new DataColumn("UnixTimestamp", typeof(ulong)));
            _tableInternal.Columns.Add(new DataColumn("NanosecondTimestamp", typeof(ulong)));

            // --- Pulseheights

            _tableInternal.Columns.Add(new DataColumn("Pulseheight_Mas_Ch1", typeof(int)));
            _tableInternal.Columns.Add(new DataColumn("Pulseheight_Mas_Ch2", typeof(int)));

            _tableInternal.Columns.Add(new DataColumn("Pulseheight_Slv_Ch1", typeof(int)));
            _tableInternal.Columns.Add(new DataColumn("Pulseheight_Slv_Ch2", typeof(int)));

            // --- Integral

            _tableInternal.Columns.Add(new DataColumn("Integral_Mas_Ch1", typeof(int)));
            _tableInternal.Columns.Add(new DataColumn("Integral_Mas_Ch2", typeof(int)));

            _tableInternal.Columns.Add(new DataColumn("Integral_Slv_Ch1", typeof(int)));
            _tableInternal.Columns.Add(new DataColumn("Integral_Slv_Ch2", typeof(int)));

            // --- MIPS

            _tableInternal.Columns.Add(new DataColumn("Mips_Mas_Ch1", typeof(float)));
            _tableInternal.Columns.Add(new DataColumn("Mips_Mas_Ch2", typeof(float)));

            _tableInternal.Columns.Add(new DataColumn("Mips_Slv_Ch1", typeof(float)));
            _tableInternal.Columns.Add(new DataColumn("Mips_Slv_Ch2", typeof(float)));

            // --- Arrival times

            _tableInternal.Columns.Add(new DataColumn("Arrivals_Mas_Ch1", typeof(float)));
            _tableInternal.Columns.Add(new DataColumn("Arrivals_Mas_Ch2", typeof(float)));

            _tableInternal.Columns.Add(new DataColumn("Arrivals_Slv_Ch1", typeof(float)));
            _tableInternal.Columns.Add(new DataColumn("Arrivals_Slv_Ch2", typeof(float)));


            _tableInternal.Columns.Add(new DataColumn("TriggerTime", typeof(float)));
            _tableInternal.Columns.Add(new DataColumn("Zenith", typeof(float)));
            _tableInternal.Columns.Add(new DataColumn("Azimuth", typeof(float)));
        }
    }
}
