using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

using System.IO;
using System.Xml;

namespace EyeSPARC.Scripting
{
    public class EyeProject
    {
        public string Name { get { return _name; } }
        private string _name;

        public ObservableCollection<ProjectFile> Files { get; set; }

        public ProjectType ProjectType { get { return _projectType; } }
        private ProjectType _projectType;


        public EyeProject(string name, ProjectType _type)
        {
            _name = name.Replace(" ", "_");
            _projectType = _type;

            if (!Directory.Exists("./projects/"))
            {
                Directory.CreateDirectory("./projects/");

            }
            if (!Directory.Exists($"./projects/{Name}/"))
            {
                Directory.CreateDirectory($"./projects/{Name}/");

            }

            Files = new ObservableCollection<ProjectFile>();

            AddNewConfigFile($"{Name}");
            AddNewFile("script1");


            WriteProjectFile();
        }
        public bool AddNewFile(string name)
        {
            string extension = GetDefaultExtension(_projectType);
            string _full = $"./project/{Name}/{name}{extension}";

            if (!File.Exists(_full))
            {
                File.Create(_full);
                Files.Add(new ProjectFile(_full));

                return true;
            }
            else
            {
                return false;
            }
        }
        public bool AddNewConfigFile(string name)
        {
            string _full = $"./project/{Name}/{name}.config.xml";

            if (!File.Exists(_full))
            {
                File.Create(_full);
                Files.Add(new ProjectFile(_full));

                return true;
            }
            else
            {
                return false;
            }
        }
        public void WriteProjectFile()
        {
            XmlWriter _writer = XmlWriter.Create($"projects/{Name}/{Name}.eyeproj", new XmlWriterSettings() { Indent = true });


            _writer.WriteStartDocument();
            _writer.WriteStartElement("EyeProject");

            _writer.WriteStartElement("Files");


            foreach (var v in Files)
            {
                _writer.WriteStartElement("File");

                _writer.WriteString(v.Filepath);

                _writer.WriteEndElement();
            }

            _writer.WriteEndElement();

            _writer.WriteEndElement();
            _writer.WriteEndDocument();

            _writer.Close();

            _writer.Flush();
        }
        public static bool Exists(string Name)
        {
            return Directory.Exists($"./projects/{Name.Replace(" ", "_")}/");
        }

        public static string GetDefaultExtension(FileType _t)
        {
            if (_t == FileType.CSharp)
            {
                return ".cs";
            }
            else if (_t == FileType.IronPython)
            {
                return ".py";
            }
            else if (_t == FileType.Xml)
            {
                return ".xml";
            }
            else
            {
                return "";
            }
        }
        public static string GetDefaultExtension(ProjectType _t)
        {
            if (_t == ProjectType.CSharp)
            {
                return ".cs";
            }
            else if (_t == ProjectType.IronPython)
            {
                return ".py";
            }
            else
            {
                return "";
            }
        }
    }
    public enum ProjectType
    {
        IronPython,
        CSharp
    }
}
