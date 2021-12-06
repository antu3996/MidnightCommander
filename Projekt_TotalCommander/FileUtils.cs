using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace Projekt_TotalCommander
{
    public class FileUtils
    {
        //podobné jako FolderTable -> FileSystemServices?
        public string CurrentFilePath { get; set; }
        public bool FileDataChanged { get; set; } = false;
        //public bool ReadOnly { get; set; }

        //public string FileName
        //{
        //}


        public FileUtils(string filepath)
        {
            this.CurrentFilePath = filepath;
        }
        
        public string GetFileName()
        {
            return Path.GetFileName(CurrentFilePath);
        }
        //public int GetFileCharsCount()
        //{
        //    return File.ReadAllText(CurrentFilePath).Length;
        //}
        public List<List<char>> GetContentOfFile()
        {
            List<List<char>> fulldata = new List<List<char>>();
            using (StreamReader reader = new StreamReader(this.CurrentFilePath))
            {
                while (!reader.EndOfStream)
                {
                    string temp = reader.ReadLine();
                    List<char> arrchar = temp.ToCharArray().ToList();
                    arrchar.Add(' ');
                    fulldata.Add(arrchar);

                }
            }
            return fulldata;
        }
        
        public void OverwriteTextFile(List<List<char>> sourceData)
        {
            //DOIMPLEMENTOVAT CONFIRM WINDOW
            if (this.FileDataChanged)
            {
                using (StreamWriter overwriter = new StreamWriter(this.CurrentFilePath))
                {
                    foreach (List<char> item in sourceData)
                    {
                        string line = new string(item.ToArray());
                        overwriter.WriteLine(line.Substring(0,line.Length-1));
                    }
                }
                this.FileDataChanged = false;
            }
            
        }
    }
}
