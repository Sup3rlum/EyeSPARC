using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

using System.IO;
using System.Xml.Linq;

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
            XDocument _doc =
                new XDocument(new XElement("EyeProject", new XAttribute("Name", Name), new XAttribute("Version", Version), new XAttribute("Type", _projectType),
                    new XElement("Files",
                        Files.Select(p => new XElement("File", new XAttribute("Path", p.Filepath))).ToArray()
                        )
                    )
                );

            _doc.Save($"projects/{Name}/{Name}.eyeproj");

        }
        public static bool Exists(string Name)
        {
            return Directory.Exists($"./projects/{Name.Replace(" ", "_")}/");
        }

        public static string GetDefaultExtension(FileType _t) =>
            _t switch
            {
                FileType.CSharp     => ".cs",
                FileType.IronPython => ".py",
                FileType.Xml        => ".xml",
                _                   => ""
            };

        public static string GetDefaultExtension(ProjectType _t) =>
            _t switch
            {
                ProjectType.CSharp      => ".cs",
                ProjectType.IronPython  => ".py",
                _                       => ""
            };

        public FileType GetDefaultType(ProjectType _type) =>
            _type switch
            {
                ProjectType.CSharp      => FileType.CSharp,
                ProjectType.IronPython  => FileType.IronPython,
                _                       => FileType.CSharp,
            };


        public static EyeProject Load(string filepath)
        {
            XDocument _doc = XDocument.Load(filepath);


            var files = from p in _doc.Descendants("Files")
                        select p.Attribute("Path").Value;

            string _name        = _doc.Element("EyeProject").Attribute("Name").Value;
            string _version     = _doc.Element("EyeProject").Attribute("Version").Value;
            string _type        = _doc.Element("EyeProject").Attribute("Type").Value;


            if (_version != Version)
            {
                Console.WriteLine($"Version mismatch, current version: {Version}, project version: {_version}");
            }



            EyeProject _proj = new EyeProject(_name, (ProjectType)Enum.Parse(typeof(ProjectType), _type));

            return _proj;
        }
    }
    public enum ProjectType
    {
        IronPython,
        CSharp
    }
}
