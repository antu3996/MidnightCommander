using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt_TotalCommander
{
    public class MainPanels
    {

        public FileSystemServices LeftPanel { get; set; }
        public FileSystemServices RightPanel { get; set; }

        public MainPanels(string pathLeft=@"C:\users\sokol\desktop\blbost", string pathRight = @"C:\users")
        {
            this.LeftPanel = new FileSystemServices(pathLeft);
            this.RightPanel = new FileSystemServices(pathRight);
        }

        public FileSystemServices Get_Selected_Sys
        {
            get
            {
                if (this.LeftPanel.IsInControl)
                {
                    return this.LeftPanel;
                }
                else
                {
                    return this.RightPanel;
                }
            }
        }
        public FileSystemServices Get_Unselected_Sys
        {
            get
            {
                if (this.LeftPanel.IsInControl)
                {
                    return this.RightPanel;
                }
                else
                {
                    return this.LeftPanel;
                }
            }
        }

        public void Copy(string destpath)
        {
            this.Get_Selected_Sys.Copy(destpath);
            this.Get_Selected_Sys.DataChanged = true;
            this.Get_Unselected_Sys.DataChanged = true;
            this.Get_Selected_Sys.CheckSelectedOutsideData();
            this.Get_Unselected_Sys.CheckSelectedOutsideData();
        }
        public void Delete(bool deleteSub)
        {
            this.Get_Selected_Sys.Delete(deleteSub);
            if (this.Get_Selected_Sys.ConfirmExecute == true)
            {
                EventWithParameter tempEvent = new EventWithParameter(this.Delete, "Delete");
                //každá funkce s nutným Dialogem ---------- parametr funkce musí být bool
                Application.OpenDialog(new ConfirmWindow(32, 8, 64, 20, ConsoleColor.Yellow, ConsoleColor.Red, "Proceed with deletion?", tempEvent));
            }
                this.Get_Selected_Sys.DataChanged = true;
                this.Get_Unselected_Sys.DataChanged = true;
                this.Get_Selected_Sys.CheckSelectedOutsideData();
                this.Get_Unselected_Sys.CheckSelectedOutsideData();

        }
        public void Move(string destpath)
        {
            this.Get_Selected_Sys.Move(destpath);
            this.Get_Selected_Sys.DataChanged = true;
            this.Get_Unselected_Sys.DataChanged = true;
            this.Get_Selected_Sys.CheckSelectedOutsideData();
            this.Get_Unselected_Sys.CheckSelectedOutsideData();
        }
        public void CreateDir(string dirname)
        {
            this.Get_Selected_Sys.CreateDirectory(dirname);
            this.Get_Selected_Sys.DataChanged = true;
            this.Get_Unselected_Sys.DataChanged = true;
            this.Get_Selected_Sys.CheckSelectedOutsideData();
            this.Get_Unselected_Sys.CheckSelectedOutsideData();
        }
        public void FileChanged()
        {
            this.Get_Selected_Sys.DataChanged = true;
            this.Get_Unselected_Sys.DataChanged = true;
        }
    }
}
