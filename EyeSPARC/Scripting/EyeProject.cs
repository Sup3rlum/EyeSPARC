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
        public static readonly string Version = "1.0";


        public string Name { get { return _name; } }
        private string _name;

        public ObservableCollection<EyeProjectFile> Files { get; set; }

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

            Files = new ObservableCollection<EyeProjectFile>();

            AddNewConfigFile($"{Name}");
            AddNewFile("script1");


            WriteProjectFile();
        }
        public bool AddNewFile(string name)
        {
            string extension = GetDefaultExtension(_projectType);
            string _full = $"./projects/{Name}/{name}{extension}";

            if (!File.Exists(_full))
            {
                File.WriteAllText(_full, Templates.GetContent(GetDefaultType(_projectType), Name, name));
                Files.Add(new EyeProjectFile(_full));

                return true;
            }
            else
            {
                return false;
            }
        }
        public bool AddNewConfigFile(string name)
        {
            string _full = $"./projects/{Name}/{name}.config.xml";

            if (!File.Exists(_full))
            {
                File.Create(_full).Close();
                Files.Add(new EyeProjectFile(_full));

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

            _writer.WriteAttributeString("Name", Name);
            _writer.WriteAttributeString("Version", Version);
            _writer.WriteAttributeString("Type", _projectType.ToString());


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
        public FileType GetDefaultType(ProjectType _type)
        {
            if (_type == ProjectType.CSharp)
            {
                return FileType.CSharp;
            }
            else
            {
                return FileType.IronPython;
            }
        }
        public static EyeProject Load(string filepath)
        {

            string _version;
            string _type;
            string _name;

            XmlReader _r = XmlReader.Create(filepath);

            while (_r.ReadToFollowing("EyeProject"))
            {
                _name = _r.GetAttribute("Name");
                _version = _r.GetAttribute("Version");
                _type = _r.GetAttribute("Type");

                if (_version != Version)
                {
                    Console.WriteLine($"Version mismatch, current version: {Version}, project version: {_version}");
                }
            }

            EyeProject _proj = new EyeProject(_name, )
        }
    }
    public enum ProjectType
    {
        IronPython,
        CSharp
    }
}
