using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Projekt_TotalCommander
{
  
    public class FileSystemServices
    {
        public bool UPDir_Selected { get; set; } = true;
        public bool ShowDriveList { get; private set; } = false;
        public string CurrentDirPath { get; set; }
        public int Selected { get; set; } = 0;
        public bool PathChanged { get; set; } = false;
        public bool DataChanged { get; set; } = false;
        public bool IsInControl { get; set; } = false;

        public bool ConfirmExecute { get; set; } = false;
        //public string Get_Dir_Path
        //{
        //    get
        //    {
        //        //if (this.CurrentDirPath[this.CurrentDirPath.Length - 1].ToString() != @"\")
        //        //{
        //        //    return this.CurrentDirPath + @"\";
        //        //}
        //        //else
        //        //{
        //        //    return this.CurrentDirPath;
        //        //}
        //        return this.CurrentDirPath;
        //    }
        //}
        public DirectoryInfo DirInfo
        {
            get { return new DirectoryInfo(this.CurrentDirPath); }
        }
        public List<DirectoryInfo> Get_List_Dir
        {
            get { return this.DirInfo.GetDirectories().ToList(); }
        }
        public List<FileInfo> Get_List_Files
        {
            get { return this.DirInfo.GetFiles().ToList(); }
        }
        public List<FileSystemInfo> Get_Dir_Content
        {
            get { return this.Get_List_Dir.Cast<FileSystemInfo>().Concat(this.Get_List_Files).ToList(); }
        }
        public List<DriveInfo> Get_Drives_List
        {
            get { return DriveInfo.GetDrives().ToList(); }
        }
        public FileSystemInfo Get_Selected
        {
            get {
                    return this.Get_Dir_Content[Selected];
            }
        }

        public DriveInfo Get_Selected_Drive
        {
            get { return this.Get_Drives_List[Selected]; }
        }

        public string Get_Selected_Name_UNI
        {
            get
            {
                if (this.ShowDriveList)
                {
                    return this.Get_Selected_Drive.Name;
                }
                else
                {
                    if (!this.UPDir_Selected)
                    {
                        return this.Get_Selected.Name;
                    }
                    else
                    {
                        return "/..";
                    }
                }
            }
        }
        public bool Editable
        {
            get
            {
                if (!this.ShowDriveList && !this.UPDir_Selected)
                {
                    return (this.Get_Selected.GetType() == typeof(FileInfo));
                }
                else
                {
                    return false;
                }
            }
        }


        public FileSystemServices(string currdir = @"C:\")
        {
            DirectoryInfo dir = new DirectoryInfo(currdir);
            if (dir.Exists)
            {
                this.CurrentDirPath = currdir;
            }
            else
            {
                this.CurrentDirPath = DriveInfo.GetDrives()[0].Name;
            }
            //if (currdir[currdir.Length - 1].ToString() != @"\")
            //{
            //    this.CurrentDirPath += @"\";
            //}
            //path checker

            //je to nutné??????????????
        }
        public void CheckSelectedOutsideData()
        {
            if (this.Get_Dir_Content.Count - 1 < this.Selected)
            {
                this.Selected = this.Get_Dir_Content.Count - 1;
            }

        }
        public void MoveIndex(bool down)
        {
            //if (this.ShowDriveList)
            //{
            //    if (down)
            //    {
            //        this.Selected = this.Selected + 1;
            //    }
            //    else
            //    {
            //        this.Selected = this.Selected-1;
            //    }
            //}
            //else
            //{
            //    if (down)
            //    {
            //        if (this.UPDir_Selected)
            //        {
            //            this.UPDir_Selected = false;
            //        }
            //        else
            //        {
            //            this.Selected = this.Selected + 1;
            //            //this.UPDir_Selected = false;
            //        }
            //    }
            //    else
            //    {
            //        if(this.Selected==0)
            //        {
            //            this.UPDir_Selected = true;
            //        }
            //        else
            //        {
            //            this.Selected=this.Selected-1;
            //            //this.UPDir_Selected = false;
            //        }
            //    }
            //}


            if (down && this.ShowDriveList)
            {
                this.Selected = this.Selected + 1;
            }
            else if (!down && this.ShowDriveList)
            {
                this.Selected = this.Selected - 1;
            }

            else if (down && !this.ShowDriveList)
            {
                if (this.UPDir_Selected)
                {
                    this.UPDir_Selected = false;
                }
                else
                {
                    this.Selected = this.Selected + 1;
                    //this.UPDir_Selected = false;
                }
            }
            else if (this.Selected == 0 && !down && !this.ShowDriveList)
            {
                this.UPDir_Selected = true;
            }
            else if (this.Selected > 0 && !down && !this.ShowDriveList)
            {
                this.Selected = this.Selected - 1;
                //this.UPDir_Selected = false;
            }


        }
        public void Delete(bool deleteNotEmpty)
        {
            if (!this.UPDir_Selected && !this.ShowDriveList) //try catch a okno s chybou asi
            {
                if (this.Get_Selected.GetType() == typeof(DirectoryInfo))
                    {
                    DirectoryInfo tempDir = (DirectoryInfo)this.Get_Selected;
                    if (tempDir.GetFiles().Length == 0 && tempDir.GetDirectories().Length == 0)
                    {
                        tempDir.Delete();
                    }
                    else
                    {
                        if (!deleteNotEmpty)
                        {
                            this.ConfirmExecute = true;
                        }
                        else
                        {
                            this.ConfirmExecute = false;
                            tempDir.Delete(true);
                        }
                    }
                    }
                    else
                    {
                        FileInfo temp = (FileInfo)this.Get_Selected;
                        temp.Delete();
                    }
                    this.DataChanged = true;
 //nutno jelikož se volá tempEvent.Function() ne MainPanels.Delete()

                //Application.CloseWindow();
            }
            else
            {
                Application.OpenDialog(new ErrorWindow(32, 8, 64, 20, ConsoleColor.Yellow, ConsoleColor.Red, $"Cannot work with {this.Get_Selected_Name_UNI}"));
                
            }
        }
        public void Move(string destpath)
        {
            if (!this.UPDir_Selected && !this.ShowDriveList) //try catch a okno s chybou asi
            {
                //string temppath = destpath;
                //if (destpath[destpath.Length - 1].ToString() != @"\")
                //{
                //    temppath += @"\";
                //}
                //path checker

                Copy(destpath);
                    Delete(true);
                //this.DataChanged = true;
                //this.Selected--;
                //Application.CloseWindow();
            }
            else
            {
                Application.OpenDialog(new ErrorWindow(32, 8, 64, 20, ConsoleColor.Yellow, ConsoleColor.Red, $"Cannot work with {this.Get_Selected_Name_UNI}"));

            }
        }
        public void CreateDirectory(string dirname)
        {
            if (!this.ShowDriveList)
            {
                //DirectoryInfo newDir = new DirectoryInfo(this.Get_Dir_Path + dirname);
                DirectoryInfo newDir = new DirectoryInfo(Path.Combine(this.CurrentDirPath,dirname));

                if (!newDir.Exists)
                {
                    newDir.Create();
                }
                this.DataChanged = true;


                //Application.CloseWindow();
            }
            else
            {
                Application.OpenDialog(new ErrorWindow(32, 8, 64, 20, ConsoleColor.Yellow, ConsoleColor.Red, $"Cannot work with {this.Get_Selected_Name_UNI}"));

            }
        }
        public void Copy(string destpath)
        {
            if (!this.UPDir_Selected && !this.ShowDriveList) //try catch a okno s chybou asi
            {
                //kopíruju vždy do složky
                //string temppath = destpath;
                //if (destpath[destpath.Length - 1].ToString() != @"\")
                //{
                //    temppath += @"\";
                //}
                //path checker

                DirectoryInfo destdir = new DirectoryInfo(/*temppath*/destpath);
                    if (!destdir.Exists)
                    {
                        destdir.Create();
                    }

                    if (this.Get_Selected.GetType() == typeof(DirectoryInfo))
                    {
                        string selectedDirName = this.Get_Selected.Name;
                        string selectedDirPath = this.Get_Selected.FullName;
                    //DirectoryCopy(selectedDirPath, temppath+selectedDirName, true);
                    DirectoryCopy(selectedDirPath, Path.Combine(destpath,selectedDirName), true);

                }
                else
                    {
                        FileInfo temp = (FileInfo)this.Get_Selected;
                    //temp.CopyTo(temppath + temp.Name);
                    temp.CopyTo(Path.Combine(destpath,temp.Name));

                }
                this.DataChanged = true;

                //Application.CloseWindow();
            }
            else
            {
                Application.OpenDialog(new ErrorWindow(32, 8, 64, 20, ConsoleColor.Yellow, ConsoleColor.Red, $"Cannot work with {this.Get_Selected_Name_UNI}"));

            }
        }
        public void GoIntoSelected()
        {
            if (this.UPDir_Selected)
            {
                this.GoParent();
            }
            else
            {
                this.GoChild();
            }
        }
        public void GoParent()
        {
            if (this.DirInfo.Parent != null) 
            {
                 this.CurrentDirPath = this.DirInfo.Parent.FullName;
                this.Selected = 0;
                this.UPDir_Selected = true;
            } 
            else
            {
                this.ShowDriveList = true;
                this.CurrentDirPath = "DRIVES";
                this.Selected = 0;
                this.UPDir_Selected = false;
            }
            this.PathChanged = true;
        }
        public void GoChild()
        {
            if (this.ShowDriveList)
            {
                this.CurrentDirPath = this.Get_Selected_Drive.Name;
                this.ShowDriveList = false;
                this.PathChanged = true;
                this.Selected = 0;
                this.UPDir_Selected = true;
            } 
            else
            {
                if (this.Get_Selected.GetType() == typeof(DirectoryInfo))
                {
                    this.CurrentDirPath = this.Get_Selected.FullName;
                    this.PathChanged = true;
                    this.Selected = 0;
                    this.UPDir_Selected = true;
                }
            }
        }
        





       //ZKOPÍROVÁNO Z MICROSOFT DOKUMENTACE
        private static void DirectoryCopy(string sourceDirPath, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirPath);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirPath);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();

            // If the destination directory doesn't exist, create it.       
            Directory.CreateDirectory(destDirName);

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string tempPath = Path.Combine(destDirName, file.Name);
                file.CopyTo(tempPath, false);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string tempPath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, tempPath, copySubDirs);
                }
            }
        }
        public static string Size(FileSystemInfo info)
        {
            if (info.GetType() == typeof(DirectoryInfo))
            {
                //return SizeDir((DirectoryInfo)info);
                return "DIR";
            }
            else
            {
                FileInfo file = (FileInfo)info;
                return file.Length.ToString();
            }
        }
        //private static long SizeDir(DirectoryInfo d)
        //{
        //    try
        //    {
        //        long size = 0;

        //        FileInfo[] fis = d.GetFiles();
        //        foreach (FileInfo fi in fis)
        //        {
        //            size += fi.Length;
        //        }

        //        DirectoryInfo[] dis = d.GetDirectories();
        //        foreach (DirectoryInfo di in dis)
        //        {
        //            size += SizeDir(di);
        //        }
        //        return size;
        //    }
        //    catch
        //    {
        //        return 0;
        //    }
        //}
        public int CurrentDriveSizeInGB()
        {
            if (!this.ShowDriveList)
            {
                string drivepath = Path.GetPathRoot(this.CurrentDirPath);
                DriveInfo drive = new DriveInfo(drivepath);
                return (int)(drive.TotalSize / 1000000000);
            }
            else
            {
                return 0;
            }
        }
        public int CurrentDrive_AV_SizeInGB()
        {
            if (!this.ShowDriveList)
            {
                string drivepath = Path.GetPathRoot(this.CurrentDirPath);
                DriveInfo drive = new DriveInfo(drivepath);
                return (int)(drive.AvailableFreeSpace / 1000000000);
            }
            else
            {
                return 0;
            }
        }

        public List<FolderTable_Row> ReturnFullData()
        {
            List<FolderTable_Row> fulldata = new List<FolderTable_Row>();
            if (this.ShowDriveList)
            {
                for (int i = 0; i < this.Get_Drives_List.Count; i++)
                {
                    DriveInfo item = this.Get_Drives_List[i];
                    FolderTable_Row row = new FolderTable_Row(item.Name, "DRIVE", "");
                    row.Row_Color = ConsoleColor.White;
                    fulldata.Add(row);
                }
            }
            else
            {
                FolderTable_Row firstrow = new FolderTable_Row(@"\..", "UP-DIR", "");
                firstrow.Row_Color = ConsoleColor.White;
                fulldata.Add(firstrow);
                for (int i = 0; i < this.Get_Dir_Content.Count; i++)
                {
                    FileSystemInfo item = this.Get_Dir_Content[i];
                    FolderTable_Row row;
                    if (item.GetType() == typeof(DirectoryInfo))
                    {
                        row = new FolderTable_Row(@"\" + item.Name, FileSystemServices.Size(item), item.LastWriteTime.ToString("MMM dd HH:mm"));
                        row.Row_Color = ConsoleColor.White;
                    }
                    else
                    {
                        row = new FolderTable_Row(" " + item.Name, FileSystemServices.Size(item), item.LastWriteTime.ToString("MMM dd HH:mm"));
                        string file_ext = Path.GetExtension(item.FullName);

                        if(file_ext == ".exe")
                        {
                            row.Row_Color = ConsoleColor.DarkGreen;
                        }
                        else if (file_ext == ".txt")
                        {
                            row.Row_Color = ConsoleColor.DarkYellow;
                        }
                        else
                        {
                            row.Row_Color = ConsoleColor.Gray;
                        }

                    }
                    fulldata.Add(row);
                }
            }
            return fulldata;
        }
    }
}
