using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Xml.Linq;
using System.Reflection;

namespace EyeSPARC
{
    public class Environment
    {

        public static readonly string Version = "0.4";

        public static string ProjectFolderPath { get; private set; }
        public static string TemplatesFolderPath { get; private set; }
        public static string ApplicationPath { get; private set; }

        public static void LoadConifguration()
        {
            ApplicationPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            if (!File.Exists("config.xml"))
            {
                Console.WriteLine($"Configuration file config.xml not found! Applying default settings.");
                WriteDefualtConfiguration();
                LoadConifguration();
            }
            else
            {
                XDocument _doc = XDocument.Load("config.xml");

                if (_doc.Element("EyeSPARC") == null)
                {
                    Console.WriteLine($"Invalid configuration file config.xml! Applying default settings.");
                    WriteDefualtConfiguration();
                    LoadConifguration();
                }
                else
                {
                    string _version = _doc.Element("EyeSPARC").Attribute("Version").Value;

                    if (Version != _version)
                    {
                        Console.WriteLine(
                            $"Version mismatch in configuration file! Current version: {Version}, configuration file version: {_version}"
                        );

                    }

                    try
                    {
                        var _xpathElements = _doc.Element("EyeSPARC").Element("Settings")
                            .Element("EnvironmentVariables")
                            .Element("Path");

                        ProjectFolderPath = _xpathElements.Element("Projects").Attribute("Value").Value;
                        TemplatesFolderPath = _xpathElements.Element("Templates").Attribute("Value").Value;

                    }
                    catch (Exception ex)
                    {
                        WriteDefualtConfiguration();
                        LoadConifguration();
                    }
                }
            }
        }

        public static void WriteDefualtConfiguration()
        {


            string _projectPath = ApplicationPath + "\\projects\\";
            string _templatesPath = ApplicationPath + "\\templates\\";

            XDocument _doc = new XDocument
                (
                    new XElement("EyeSPARC", 
                        new XAttribute("Version", Version),
                        new XElement("Settings",
                            new XElement("EnvironmentVariables",
                                new XElement("Path",
                                    new XElement("Projects",new XAttribute("Value", _projectPath)),
                                    new XElement("Templates", new XAttribute("Value", _templatesPath))
                                )
                            )
                        )
                    )
                );

            _doc.Save("config.xml");
        }
    }
}
