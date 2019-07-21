using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;


namespace EyeSPARC.Scripting
{
    public class Templates
    {
        public static ScriptTemplate CSharp { get; private set; }
        public static ScriptTemplate IronPython { get; private set; }


        public static void LoadAll()
        {
            if (File.Exists("./templates/cs.template"))
            {
                CSharp = new ScriptTemplate()
                {
                    Content = File.ReadAllText("./templates/cs.template"),
                    TargetType = FileType.CSharp
                };
            }
            else
            {
                MessageBox.Show("Could not find file template, empty file will be created");
            }

            if (File.Exists("./templates/py.template"))
            {
                IronPython = new ScriptTemplate()
                {
                    Content = File.ReadAllText("./templates/py.template"),
                    TargetType = FileType.IronPython
                };
            }
            else
            {
                MessageBox.Show("Could not find file template, empty file will be created");
            }
        }
        public static ScriptTemplate ForProject(ProjectType _pType) =>
            _pType switch
            {
                ProjectType.IronPython  => IronPython,
                ProjectType.CSharp      => CSharp,
                _                       => CSharp
            };
    }
    public class ScriptTemplate
    {
        public FileType TargetType { get; set; }
        public string Content { get; set; }

        public string Parse(string _projName, string _fileName)
        {
            return Content.Replace("$project_name$", _projName).Replace("$file_name$", _fileName);
        }
    }
}
