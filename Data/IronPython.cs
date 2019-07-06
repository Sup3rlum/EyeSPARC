using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;
using IronPython.Hosting;
using IronPython.Modules;

namespace EyeSPARC.Data
{
    public static class IronPython
    {
        public static ScriptEngine ScriptEngine;

        public static IronPythonStatus EngineStatus = IronPythonStatus.Unknown;

        public static void Initialize()
        {
            ScriptEngine = Python.CreateEngine();

            ICollection<string> _searchpaths = ScriptEngine.GetSearchPaths();
        }
    }
    public enum IronPythonStatus
    {
        Initialized,
        Failed,
        Unknown
    }
}
