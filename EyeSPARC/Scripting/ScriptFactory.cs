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

            ScriptType _type;

            string _fileContent = File.ReadAllText(filepath);
            string extension = Path.GetExtension(filepath);


            if (extension == ".py" || extension == ".ipy")
            {
                _type = ScriptType.IronPython;
            }
            else if (extension == ".cs" || extension == ".csharp")
            {
                _type = ScriptType.CSharp;
            }
            else
            {
                throw new NotSupportedException($"File extenstions with the type \"{extension}\" are not supported.");
            }


            EyeScript _script = new EyeScript(_fileContent, _type);


            return _script;
        }
    }
}
