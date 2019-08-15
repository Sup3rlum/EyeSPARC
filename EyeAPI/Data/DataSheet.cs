using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;

namespace EyeAPI.Data
{
    public class DataSheet
    {
        public DataTable DataTable { get; private set; }

        public PublicDataType Type { get; private set; }

        public string Name { get; set; }

        public DataSheet(string name, PublicDataType type)
        {
            Name = name;

            if (type == PublicDataType.Events)
            {
                DataTable = new DataTable(name);

                DataTable.Columns.Add(new DataColumn("UnixTimestamp", typeof(ulong)));
                DataTable.Columns.Add(new DataColumn("NanosecondTimestamp", typeof(ulong)));

                // --- Pulseheights

                DataTable.Columns.Add(new DataColumn("Pulseheight_Mas_Ch1", typeof(int)));
                DataTable.Columns.Add(new DataColumn("Pulseheight_Mas_Ch2", typeof(int)));

                DataTable.Columns.Add(new DataColumn("Pulseheight_Slv_Ch1", typeof(int)));
                DataTable.Columns.Add(new DataColumn("Pulseheight_Slv_Ch2", typeof(int)));

                // --- Integral

                DataTable.Columns.Add(new DataColumn("Integral_Mas_Ch1", typeof(int)));
                DataTable.Columns.Add(new DataColumn("Integral_Mas_Ch2", typeof(int)));

                DataTable.Columns.Add(new DataColumn("Integral_Slv_Ch1", typeof(int)));
                DataTable.Columns.Add(new DataColumn("Integral_Slv_Ch2", typeof(int)));

                // --- MIPS

                DataTable.Columns.Add(new DataColumn("Mips_Mas_Ch1", typeof(float)));
                DataTable.Columns.Add(new DataColumn("Mips_Mas_Ch2", typeof(float)));

                DataTable.Columns.Add(new DataColumn("Mips_Slv_Ch1", typeof(float)));
                DataTable.Columns.Add(new DataColumn("Mips_Slv_Ch2", typeof(float)));

                // --- Arrival times

                DataTable.Columns.Add(new DataColumn("Arrivals_Mas_Ch1", typeof(float)));
                DataTable.Columns.Add(new DataColumn("Arrivals_Mas_Ch2", typeof(float)));

                DataTable.Columns.Add(new DataColumn("Arrivals_Slv_Ch1", typeof(float)));
                DataTable.Columns.Add(new DataColumn("Arrivals_Slv_Ch2", typeof(float)));

                DataTable.Columns.Add(new DataColumn("TriggerTime", typeof(float)));
                DataTable.Columns.Add(new DataColumn("Zenith", typeof(float)));
                DataTable.Columns.Add(new DataColumn("Azimuth", typeof(float)));
            }
        }
        public void AddRow(params object[] values)
        {
            DataTable.Rows.Add(values);

        }
        public T Compute<T>(string expr, string filter)
        {
            return (T)DataTable.Compute(expr, filter);
        }
    }
}
