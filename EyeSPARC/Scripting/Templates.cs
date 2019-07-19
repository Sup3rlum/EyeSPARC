using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace EyeSPARC.Scripting
{
    public static class Templates
    {

        private static string _csharp;

        public static void LoadAll()
        {
            if (File.Exists("./templates/cs.template"))
            {
                _csharp = File.ReadAllText("./templates/cs.template");
            }

            if (File.Exists("./templates/py.template"))
            {
                _csharp = File.ReadAllText("./templates/py.template");
            }
        }

    }
}
