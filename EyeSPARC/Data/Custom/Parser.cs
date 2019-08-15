using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace EyeSPARC.Data.Custom
{
    public class Parser
    {
        public static List<string[]> CsvReadAll(string filepath)
        {
            return (List<string[]>)File.ReadAllLines(filepath).Select(s => s.Split(','));
        }
        public static List<string[]> TsvReadAll(string filepath)
        {
            return (List<string[]>)File.ReadAllLines(filepath).Select(s => s.Split('\t'));
        }
    }
}
