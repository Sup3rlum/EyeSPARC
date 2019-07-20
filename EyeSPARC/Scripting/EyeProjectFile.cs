using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace EyeSPARC.Scripting
{
    public class EyeProjectFile
    {

        public string Name { get { return Path.GetFileName(_filepath); } }
        private string _filepath;

        public FileType FileType { get { return _fileType; } }
        private FileType _fileType;

        public string Filepath { get { return _filepath; } }

        public string Extension { get { return Path.GetExtension(_filepath); } }

        public string Content { get; set; }

        public EyeProjectFile(string filepath)
        {

            _filepath = filepath;


            if (Extension == ".py" || Extension == ".ipy")
            {
                _fileType = FileType.IronPython;
            }
            else if (Extension == ".cs" || Extension == ".csharp")
            {
                _fileType = FileType.CSharp;
            }
            else if (Extension == ".xml")
            {
                _fileType = FileType.Xml;
            }

            Content = File.ReadAllText(filepath);
        }

        public void Save()
        {
            File.WriteAllText(_filepath, Content);
        }

    }

    public enum FileType
    {
        IronPython,
        CSharp,
        Xml
    }
}
