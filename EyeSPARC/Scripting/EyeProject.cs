using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Xml;

namespace EyeSPARC.Scripting
{
    public class EyeProject
    {
        public string Name { get { return _name; } }
        private string _name;

        public List<ProjectFile> Files { get; set; }

        public EyeProject(string name)
        {
            _name = name.Replace(" ", "_");

            if (!Directory.Exists("./projects/"))
            {
                Directory.CreateDirectory("./projects/");

            }
            if (!Directory.Exists($"./projects/{Name}/"))
            {
                Directory.CreateDirectory($"./projects/{Name}/");

            }
            else
            {
                //throw new Exception($"Project with name { Name } already exists");
            }


            Files = new List<ProjectFile>();

            File.Create($"./projects/{Name}/script1.ipy");
            File.Create($"./projects/{Name}/script2.cs");
            File.Create($"./projects/{Name}/{Name}.config.xml");



            Files.Add(new ProjectFile($"./projects/{Name}/script1.ipy"));
            Files.Add(new ProjectFile($"./projects/{Name}/script2.cs"));
            Files.Add(new ProjectFile($"./projects/{Name}/{Name}.config.xml"));

            WriteProjectFile();
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
    }
}
