using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;
using IronPython;
using IronPython.Hosting;
using IronPython.Runtime;


namespace EyeSPARC.Data
{
    public class Network
    {
        public Network()
        {
            IronPython.Initialize();

            IronPython.ScriptEngine.ImportModule("sapphire");
        }
    }
}
