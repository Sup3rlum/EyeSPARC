using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;
using System.Xml.Linq;
using IronPython.Modules;

namespace EyeSPARC.Data.HiSPARC
{
    public class DataPackageManager
    {
        public static List<DataPackage> Packages { get; private set; }

        public static void Initialize()
        {
            if (!Directory.Exists(Environment.DataPackagePath))
            {
                Directory.CreateDirectory(Environment.DataPackagePath);
            }

            if (!File.Exists(Environment.DataPackagePath + "datapackages.config.xml"))
            {
                WriteDefaultConfig();
            }
        }

        public static void WriteConfig()
        {
            XDocument _doc = new XDocument
            (
                new XElement("EyeSPARC",
                    new XElement("Packages",
                        Packages.Select(x => new XElement("Database", new XAttribute("Name", x.Name),
                            new XAttribute("FileName", x.FileName), new XAttribute("StartDateTime", x.Start),
                            new XAttribute("EndDateTime", x.End), new XAttribute("Type", x.Type)))
                    )
                )
            );

            _doc.Save(Environment.DataPackagePath + "datapackages.config.xml");
        }

        public static void WriteDefaultConfig()
        {
            XDocument _doc = new XDocument
            (
                new XElement("EyeSPARC",
                    new XElement("Packages"
                    )
                )
            );

            _doc.Save(Environment.DataPackagePath + "datapackages.config.xml");
        }

        public static void LoadConfig()
        {
            try
            {
                XDocument _doc = XDocument.Load(Environment.DataPackagePath + "datapackages.config.xml");

                var _descendants = _doc.Element("EyeSPARC").Element("Packages").Descendants();

                Packages = _descendants.Select(x => new DataPackage
                    (
                        DateTime.Parse(x.Attribute("StartDateTime").Value),
                        DateTime.Parse(x.Attribute("EndDateTime").Value),
                        x.Attribute("Name").Value,
                        x.Attribute("FileName").Value,
                        DataPackageType.SQL
                    )
                ).ToList();
            }
            catch (Exception ex)
            {
                WriteConfig();
                LoadConfig();
            }
        }

       /* public static DataPackage RequestPackage(DateTime start, DateTime end)
        {

        }*/
    }
}
