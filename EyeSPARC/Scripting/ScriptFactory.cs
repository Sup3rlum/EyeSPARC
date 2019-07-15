using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace EyeSPARC.Scripting
{
    public static class ScriptFactory
    {
        public static EyeScript FromFile(string filepath)
        {
            EyeScript _script;

            string _file = File.ReadAllText(filepath);

            return _script;
        }
    }
}
