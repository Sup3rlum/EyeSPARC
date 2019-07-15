using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeSPARC.Scripting
{
    public class EyeScript
    {
        public string Content { get; }
        private string _content;

        public ScriptType ScriptType { get; }
        private ScriptType _scriptType;

        public EyeScript(string content, ScriptType type)
        {
            _content = content;
            _scriptType = type;
        }

    }
    public enum ScriptType
    {
        IronPython,
        CSharp
    }
}
