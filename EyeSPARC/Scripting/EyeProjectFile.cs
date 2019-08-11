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

        //  Properties

        public FileType FileType    { get; private set; }

        public string   Filepath    { get; private set; }
        public string   Content     { get; set; }
        public string   Extension   { get => Path.GetExtension(Filepath); }
        public string   Name        { get => Path.GetFileNameWithoutExtension(Filepath); }


        // Functionality

        public EyeProjectFile(string filepath)
        {

            Filepath = filepath;
            FileType = GetType(Extension);

            Content = File.ReadAllText(filepath);
        }

        public void Save()
        {
            File.WriteAllText(Filepath, Content);
        }


        // Static Members

        public static FileType GetType(string _e) =>
            _e switch
            {
                ".py" => FileType.IronPython,
                ".cs" => FileType.CSharp,
                ".xml" => FileType.Xml,
                _ => throw new ArgumentException($"File(s) with extension {_e} are not supported")
            };
    }

    public enum FileType
    {
        IronPython,
        CSharp,
        Xml
    }
}
