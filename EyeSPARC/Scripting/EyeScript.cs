using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeSPARC.Scripting
{
    public class EyeScript
    {
        public string Raw { get; set; }
        public ScriptType ScriptType { get; set; }
    }
    public enum ScriptType
    {
        IronPython,
        CSharp
    }
}
