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



        public string Name { get; private set; }

        public ObservableCollection<EyeProjectFile> Files { get; set; }

        public ProjectType ProjectType { get; private set; }

        public static EyeProject New(string name, ProjectType type)
        {
            EyeProject _proj = new EyeProject();

            _proj.Name = name.Replace(" ", "_");
            _proj.ProjectType = type;


            if (!Directory.Exists("./projects/"))
            {
                Directory.CreateDirectory("./projects/");

            }
            if (Directory.Exists($"./projects/{name}"))
            {
                return null;
            }
            else
            {
                Directory.CreateDirectory($"./projects/{name}/");

            }

            _proj.Files = new ObservableCollection<EyeProjectFile>();

            _proj.AddNewConfigFile($"{name}");
            _proj.AddNewFile("script1");


            _proj.WriteProjectFile();

            return _proj;
        }
        public bool AddNewFile(string filename)
        {
            string extension = GetDefaultExtension(ProjectType);
            string _full = $"./projects/{Name}/{filename}{extension}";

            if (!File.Exists(_full))
            {
                File.WriteAllText(_full, Templates.ForProject(ProjectType).Parse(Name, filename));
                Files.Add(new EyeProjectFile(_full));

                return true;
            }
            else
                return false;
        }
        public bool AddNewConfigFile(string filename)
        {
            string _full = $"./projects/{Name}/{filename}.config.xml";

            if (!File.Exists(_full))
            {
                File.Create(_full).Close();
                Files.Add(new EyeProjectFile(_full));

                return true;
            }
            else
                return false;
        }
        public void WriteProjectFile()
        {
            XDocument _doc =
                new XDocument(new XElement("EyeProject", new XAttribute("Name", Name), new XAttribute("Version", Version), new XAttribute("Type", ProjectType),
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

        public static FileType GetDefaultType(ProjectType _type) =>
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

            EyeProject _proj = new EyeProject();

            _proj.Name = _name;
            _proj.ProjectType = (ProjectType)Enum.Parse(typeof(ProjectType), _type);

            return _proj;
        }
    }
    public enum ProjectType
    {
        IronPython,
        CSharp
    }
}
