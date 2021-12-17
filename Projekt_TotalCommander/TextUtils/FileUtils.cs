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
        public List<string> tempData { get; set; }


        public FileUtils(string filepath)
        {
            this.CurrentFilePath = filepath;
        }
        
        public string GetFileName()
        {
            return Path.GetFileName(CurrentFilePath);
        }
        //public List<List<char>> GetContentOfFile()
        //{
        //    List<List<char>> fulldata = new List<List<char>>();
        //    using (StreamReader reader = new StreamReader(this.CurrentFilePath))
        //    {
        //        while (!reader.EndOfStream)
        //        {
        //            string temp = reader.ReadLine();
        //            List<char> arrchar = temp.ToCharArray().ToList();
        //            arrchar.Add('¬');
        //            fulldata.Add(arrchar);

        //        }
        //    }
        //    return fulldata;
        //}
        public List<string> GetContentOfFile()
        {
            List<string> fulldata = new List<string>();
            using (StreamReader reader = new StreamReader(this.CurrentFilePath))
            {
                while (!reader.EndOfStream)
                {
                    string temp = reader.ReadLine()+ '¬';
                    fulldata.Add(temp);

                }
            }
            return fulldata;
        }

        public void OverwriteTextFile(bool overWrite)
        {
            //DOIMPLEMENTOVAT CONFIRM WINDOW
            if (this.FileDataChanged)
            {
                if (overWrite)
                {
                    using (StreamWriter overwriter = new StreamWriter(this.CurrentFilePath))
                    {
                        foreach (string item in this.tempData)
                        {
                            if (item[item.Length-1]!= '¬')
                            {
                                overwriter.Write(item);
                            }
                            else
                            {
                                overwriter.WriteLine(item.Substring(0, item.Length - 1));
                            }
                        }
                    }
                    this.FileDataChanged = false;
                }
                else
                {
                        EventWithParameter tempEvent = new EventWithParameter(this.OverwriteTextFile, "Overwrite data");
                        Application.OpenDialog(new ConfirmWindow(32, 8, 64, 20, ConsoleColor.Yellow, 
                            ConsoleColor.Red, "Do you want to continue?", tempEvent));
                    
                }
            }
            
        }
    }
}
