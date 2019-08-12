using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

using System.IO;
using System.Windows;
using System.Xml.Linq;
using Microsoft.Scripting.Utils;

namespace EyeSPARC.Scripting
{
    public class EyeProject
    {
        public static readonly string Version = "1.0";


        // --- Properties


        public string                               Name        { get; private set; }
        public ObservableCollection<EyeProjectFile> Files       { get; set; }
        public ProjectType                          ProjectType { get; private set; }


        // --- Functionality

        public bool AddNewFile(string filename)
        {
            string _extension = GetDefaultExtension(ProjectType);
            string _full = Environment.ProjectFolderPath + $"{Name}\\{filename}{_extension}";

            if (!File.Exists(_full))
            {
                File.WriteAllText(_full, Templates.ForProject(ProjectType).Parse(Name, filename));
                Files.Add(new EyeProjectFile(_full));

                return true;
            }
            else
                return false;
        }
        public bool AddConfigFile(string filename)
        {
            string _full = Environment.ProjectFolderPath + $"{Name}\\{filename}.config.xml";

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

            _doc.Save(Environment.ProjectFolderPath + $"{Name}\\{Name}.eyeproj");

        }

        public void SaveAllFiles()
        {
            foreach (var _file in Files)
            {
                _file.Save();
            }
        }
        /*
         *
         *  Project Loading
         *
         */


        public static EyeProject Load(string filepath)
        {
            XDocument _doc = XDocument.Load(filepath);


            if (_doc.Element("EyeProject") == null)
            {
                MessageBox.Show($"Invalid project file {filepath}");
                return null;
            }

            var files = from p in _doc.Element("EyeProject").Element("Files").Descendants()
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

            _proj.Files = new ObservableCollection<EyeProjectFile>();
            _proj.Files.AddRange(files.Select(f => new EyeProjectFile(f)));

            return _proj;
        }

        public static EyeProject New(string name, ProjectType type)
        {
            EyeProject _proj = new EyeProject();

            _proj.Name = name.Replace(" ", "_");
            _proj.ProjectType = type;


            if (!Directory.Exists(Environment.ProjectFolderPath))
            {
                Directory.CreateDirectory(Environment.ProjectFolderPath);
            }

            if (Directory.Exists(Environment.ProjectFolderPath + _proj.Name))
            {
                MessageBox.Show($"A project file with the name {name} already exists!");
                return null;
            }

            Directory.CreateDirectory(Environment.ProjectFolderPath + _proj.Name);

            _proj.Files = new ObservableCollection<EyeProjectFile>();

            _proj.AddConfigFile($"{_proj.Name}");
            _proj.AddNewFile("script1");


            _proj.WriteProjectFile();

            return _proj;
        }

        /*
         *
         *  Static Functionality
         *
         */

        public static bool Exists(string Name)
        {
            return Directory.Exists(Environment.ProjectFolderPath + Name.Replace(" ", "_"));
        }

        public static string GetExtension(FileType _t) =>
            _t switch
            {
                FileType.CSharp => ".cs",
                FileType.IronPython => ".py",
                FileType.Xml => ".xml",
                _ => ""
            };

        public static string GetDefaultExtension(ProjectType _t) =>
            _t switch
            {
                ProjectType.CSharp => ".cs",
                ProjectType.IronPython => ".py",
                _ => ""
            };

        public static FileType GetDefaultType(ProjectType _type) =>
            _type switch
            {
                ProjectType.CSharp => FileType.CSharp,
                ProjectType.IronPython => FileType.IronPython,
                _ => FileType.CSharp,
            };
    }
    public enum ProjectType
    {
        IronPython,
        CSharp
    }
}
