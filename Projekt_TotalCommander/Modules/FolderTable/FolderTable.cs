using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Projekt_TotalCommander
{
    
    public class FolderTable : IGraphicModule
    {
        public bool Redraw { get; set; } = true;
        //public bool UpdateVisual { get; set; } = false;

        public bool IsSelected { get; set; } = false;
        public ConsoleUtils2 Drawer { get; set; }
        public ConsoleUtils2 console2 { get; set; }
        public FileSystemServices Currsystem { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public ConsoleColor Fore_Color { get; set; }
        public ConsoleColor Back_Color { get; set; }
        public ConsoleColor Selected_Color_fore { get; set; }
        public ConsoleColor Selected_Color_back { get; set; }


        public int Selected { get; set; } = 0;
        public int Top { get; set; } = 0;
        public List<string> headerColumns { get; set; }
        public List<FolderTable_Row> Data { get; set; }

        public FolderTable(bool isSelected,int x, int y, int w,int h,
            FileSystemServices service, ConsoleColor fore_Col, ConsoleColor back_Col, 
            ConsoleColor selected_fore, ConsoleColor selected_back)
        {
            this.IsSelected = isSelected;
            this.X = x;
            this.Y = y;
            this.Height = h;
            this.Width = w;
            this.Fore_Color = fore_Col;
            this.Back_Color = back_Col;
            this.Selected_Color_fore = selected_fore;
            this.Selected_Color_back = selected_back;
            this.Drawer = new ConsoleUtils2(x + 1, y + 1, w - 2, (h - 2)-3, fore_Col, back_Col);
            this.console2 = new ConsoleUtils2(x, y, w, h, fore_Col, back_Col);
            this.Currsystem = service;

            this.headerColumns = new List<string>() { "Name", "Size", "MTime" };
            this.Data = this.Currsystem.ReturnFullData();
        }

        public void Draw()
        {
            if (IsSelected /*|| UpdateVisual*/ || this.Currsystem.DataChanged || this.Currsystem.PathChanged || Redraw)
            {
                //console2.Clear(0, 0);

                //DRAWING BORDER/////////////////////////////////////////////////////////////////////////////////////////////////////////
                //
                //
                //
                //
                this.DrawBorder();

                //DRAWING DATA/////////////////////////////////////////////////////////////////////////////////////////////////////////
                //
                //
                //
                //
                //Console.Clear(0, 0);
                this.DrawFolderContent();

                Redraw = false;
                //UpdateVisual = false;
            }
        }
        public void DrawFolderContent()
        {
            this.Drawer.ResetToOrigin();
            List<int> widths = this.Widths();
            this.DrawHeader(this.headerColumns, widths, '|', ' ');

            for (int i = this.Top; i < this.Top + this.Drawer.MaxHeightWrite; i++)
            {
                if (i < this.Data.Count)
                {
                    if (i == this.Selected && this.IsSelected)
                    {
                        this.Drawer.ForeColor = this.Selected_Color_fore;
                        this.Drawer.BackColor = this.Selected_Color_back;
                    }
                    else
                    {
                        this.Drawer.ForeColor = this.Data[i].Row_Color;
                    }

                    this.DrawData(this.Data[i].Values, widths, this.Drawer.ForeColor);
                }
                else
                {
                    this.DrawEmpty(widths, '│');
                }
                this.Drawer.ForeColor = this.Fore_Color;
                this.Drawer.BackColor = this.Back_Color;
            }
        }
        public void DrawBorder()
        {
            this.console2.ResetToOrigin();
            
            string driveSizeRatio = this.Currsystem.CurrentDrive_AV_SizeInGB() + "G/" + this.Currsystem.CurrentDriveSizeInGB() + "G";
            //this.console2.WriteLine("┌" + $"<-{this.Currsystem.Get_Dir_Path}".PadRight(this.console2.MaxWidthWrite - 6, '─') + ".[^]" + "┐");


            this.console2.Write("┌" + "<-");

            string temp = this.Currsystem.CurrentDirPath;
            if (temp.Length > this.Drawer.MaxWidthWrite - 5)
            {
                temp = temp.Substring(0, this.Drawer.MaxWidthWrite - 5);
            }

            if (this.IsSelected)
            {
                this.console2.BackColor = this.Selected_Color_back;
                this.console2.ForeColor = this.Selected_Color_fore;
            }
            this.console2.Write($"{temp}");
            this.console2.BackColor = this.Back_Color;
            this.console2.ForeColor = this.Fore_Color;


            this.console2.WriteLine("".PadRight(this.console2.MaxWidthWrite - 6 - temp.Length, '─') + ".[^]" + "┐");


            for (int i = 0; i < this.console2.MaxHeightWrite - 4; i++)
            {

                this.console2.CursorX = this.console2.InitX;
                this.console2.Write("│");
                this.console2.CursorX = this.console2.InitX + this.console2.MaxWidthWrite-1;
                this.console2.WriteLine("│");
            }
            this.console2.WriteLine("├" + "".PadRight(this.console2.MaxWidthWrite - 2, '─') + "┤");
            this.console2.WriteLine("│" + $"{this.Currsystem.Get_Selected_Name_UNI}".PadRight(this.console2.MaxWidthWrite - 2, ' ') + "│");
            this.console2.WriteLine("└" + $"{driveSizeRatio}".PadLeft(this.console2.MaxWidthWrite - 2, '─') + "┘");

        }

        private void DrawData(List<string> data, List<int> widths,ConsoleColor color)
        {
            this.DrawRow(data, widths, '│', ' ',color);
        }

        private void DrawRow(List<string> data, List<int> widths, char sep, char pad,ConsoleColor color)
        {
            int index = 0;
            foreach (string item in data)
            {
                string tempitem = item;
                if (item.Length > widths[index])
                {
                    tempitem = item.Substring(0, widths[index]);
                }

                this.Drawer.ForeColor = color;
                this.Drawer.Write($"{tempitem.PadRight(widths[index], pad)}");
                this.Drawer.ForeColor = this.Fore_Color;

                if (index < data.Count - 1)
                {
                    this.Drawer.Write(sep.ToString());
                }

                index++;
            }
            this.Drawer.WriteLine();
        }
        private void DrawEmpty(List<int> widths, char sep)
        {
            int index = 0;
            foreach (int x in widths)
            {
                this.Drawer.Write($"{"".PadRight(x)}");
                if (index < widths.Count - 1)
                {
                    this.Drawer.Write(sep.ToString());
                }
                index++;
            }
            this.Drawer.WriteLine();
        }
        private void DrawHeader(List<string> data, List<int> widths, char sep, char pad)
        {
            int index = 0;
            foreach (string item in data)
            {
                string tempitem = item;
                if (item.Length > widths[index])
                {
                    tempitem = item.Substring(0, widths[index]);
                }
                this.Drawer.Write($"{(tempitem.PadLeft(((widths[index] - tempitem.Length) / 2) + tempitem.Length, pad)).PadRight(widths[index], ' ')}");
                if (index < data.Count - 1)
                {
                    this.Drawer.Write(sep.ToString());
                }

                index++;
            }
            this.Drawer.WriteLine();
        }


        private List<int> Widths()
        {
            return new List<int>() { this.Drawer.MaxWidthWrite -  ((this.Drawer.MaxWidthWrite / 4)+(this.Drawer.MaxWidthWrite / 7)), this.Drawer.MaxWidthWrite / 7, this.Drawer.MaxWidthWrite / 4 };
        }

        public void HandleKey(ConsoleKeyInfo key)
        {
            if (this.IsSelected)
            {
                if (key.Key == ConsoleKey.UpArrow && this.Selected > 0)
                {
                    this.Selected--;

                    if (this.Selected == this.Top - 1)
                    {
                        //this.Top--;
                        this.Top = this.Top - Math.Min(10, this.Top);
                    }

                    this.Currsystem.MoveIndex(false);
                }
                else if (key.Key == ConsoleKey.DownArrow && this.Selected < this.Data.Count - 1)
                {
                    this.Selected++;

                    if (this.Selected == this.Top + this.Drawer.MaxHeightWrite)
                    {
                        //this.Top++;
                        this.Top = this.Top + Math.Min(10, this.Data.Count - this.Top - this.Drawer.MaxHeightWrite);
                    }
                    //this.Currsystem.Selected++;
                    this.Currsystem.MoveIndex(true);
                }
                else if (key.Key == ConsoleKey.Enter)
                {
                    //if (this.Selected == 0 /*&& !this.Currsystem.ShowDriveList*/)
                    //{
                        //this.Currsystem.GoParent();
                        this.Currsystem.GoIntoSelected();
                    //}
                    //else
                    //{
                    //    this.Currsystem.GoChild();
                    //}
                }
            }
        }

        public void Update()
        {

            if (this.IsSelected)
            {
                this.Currsystem.IsInControl = true;
            }
            else
            {
                this.Currsystem.IsInControl = false;
            }

            //if (!this.Currsystem.ShowDriveList)
            //{
            if (this.Currsystem.PathChanged)
            {
                this.Refresh_For_Data();
                this.Currsystem.PathChanged = false;

                //if (this.Currsystem.ShowDriveList)
                //{
                //    this.Selected = this.Currsystem.Selected;
                //}
                //else
                //{
                //    this.Selected = this.Currsystem.Selected + 1;
                //}
                this.Selected = 0;
                this.Top = 0;
            }
            if (this.Currsystem.DataChanged)
            {
                this.Refresh_For_Data();
                this.Currsystem.DataChanged = false;

                if (this.Data.Count-1<this.Selected)
                {
                    this.Selected = this.Data.Count - 1;
                }
            }
        }
        private void Refresh_For_Data()
        {
            this.Data = this.Currsystem.ReturnFullData();
        }
        //private void RefreshData()
        //{
        //    this.Data = new List<FolderTable_Row>();
        //    this.Data.Add(new FolderTable_Row(@"\..", "", ""));
        //    for (int i = 0; i < this.Currsystem.Get_Dir_Content.Count; i++)
        //    {
        //        FileSystemInfo item = this.Currsystem.Get_Dir_Content[i];
        //        FolderTable_Row row;
        //        if (item.GetType() == typeof(DirectoryInfo))
        //        {
        //            row = new FolderTable_Row(@"\" + item.Name, FileSystemServices.Size(item), item.LastWriteTime.ToString("MMM dd HH:mm"));
        //        }
        //        else
        //        {
        //            row = new FolderTable_Row(" " + item.Name, FileSystemServices.Size(item), item.LastWriteTime.ToString("MMM dd HH:mm"));
        //        }
        //        this.Data.Add(row);
        //    }
        //}
        //private void RefreshDataForDrive()
        //{
        //    this.Data = new List<FolderTable_Row>();
        //    for (int i = 0; i < this.Currsystem.Get_Drives_List.Count; i++)
        //    {
        //        DriveInfo item = this.Currsystem.Get_Drives_List[i];
        //        FolderTable_Row row = new FolderTable_Row(item.Name, "", "");
        //        this.Data.Add(row);
        //    }
        //}
    }
}
