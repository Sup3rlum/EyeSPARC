using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;


namespace EyeSPARC.Scripting
{
    public static class Templates
    {

        private static string _csharp;
        private static string _py;

        public static void LoadAll()
        {
            if (File.Exists("./templates/cs.template"))
            {
                _csharp = File.ReadAllText("./templates/cs.template");
            }
            else
            {
                MessageBox.Show("Could not find file template, empty file will be created");
            }

            if (File.Exists("./templates/py.template"))
            {
                _py = File.ReadAllText("./templates/py.template");
            }
            else
            {
                MessageBox.Show("Could not find file template, empty file will be created");
            }
        }
        public static string GetContent(FileType _type, string projectname, string filename)
        {
            if (_type == FileType.CSharp)
            {
                return _csharp.Replace("$project_name$", projectname).Replace("$file_name$", filename);
            }
            else if (_type == FileType.IronPython)
            {
                return _py;
            }
            else
            {
                return "<xml></xml>";
            }
        }
    }
}
