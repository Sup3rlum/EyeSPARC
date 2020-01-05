using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;

namespace EyeSPARC.Data.HiSPARC
{
    public class DataPackage
    {
        public DateTime Start { get; private set; }
        public DateTime End { get; private set; }

        public string Name { get; private set; }
        public string FileName { get; private set; }

        public DataPackageType Type { get; private set; }

        public DataPackage(DateTime start, DateTime end, string name, string filename, DataPackageType type)
        {
            Start = start;
            End = end;
            Name = name;
            FileName = filename;
            Type = type;
        }
    }

    public enum DataPackageType
    {
        SQL
    }
}
