using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt_TotalCommander
{
    public class CopyDirWindow : Window, IFunctionalWindow
    {
        private TextBox source_TextBox;
        private TextBox dest_TextBox;

        public MainPanels Panels { get; set; }


        public CopyDirWindow(int x, int y, int w, int h, ConsoleColor fore_Col, ConsoleColor back_Col , MainPanels panels) : base(x, y, w, h,fore_Col,back_Col)
        {
            this.Panels = panels;

            string name1 = "Copying '" + this.Panels.Get_Selected_Sys.Get_Selected_Name_UNI + "' with source mask: ";
            TextBox textbox1 = new TextBox
                (false, this.X+3, this.Y + 1, this.Width-15, name1, "*", ConsoleColor.Blue, ConsoleColor.Black, ConsoleColor.Green, ConsoleColor.Yellow, ConsoleColor.Magenta);

            string name2 = "to: ";
            TextBox textbox2 = new TextBox
                (false, this.X+3, this.Y + 1 + textbox1.Height + 1, this.Width-15, name2, this.Panels.Get_Unselected_Sys.CurrentDirPath, ConsoleColor.Blue, ConsoleColor.Black, ConsoleColor.Green,ConsoleColor.Yellow, ConsoleColor.Magenta);

            this.source_TextBox = textbox1;
            this.dest_TextBox = textbox2;

            Button btnOk = new Button
                (false, this.X+(this.Width / 2)-7, this.Y + 1 + textbox1.Height + 1 + textbox2.Height + 1, 6, 1, "[ Ok ]",ConsoleColor.Black,ConsoleColor.DarkBlue,ConsoleColor.DarkGreen);
            Button btnCancel = new Button
                (false, this.X+(this.Width / 2), this.Y + 1 + textbox1.Height + 1 + textbox2.Height + 1, 10, 1, "[ Cancel ]", ConsoleColor.Black, ConsoleColor.DarkBlue, ConsoleColor.DarkGreen);

            btnOk.Click += Ok_btn;
            btnCancel.Click += Cancel_btn;

            Container mainContainer = new Container(true, textbox1, textbox2, btnOk, btnCancel);
            this.modules.Add(mainContainer);
     
        }
        public override void Draw()
        {
            if (this.RedrawAll)
            {
                this.Drawer.Clear();
                this.Drawer.ResetToOrigin();

                string textHeader = "Copy";
                int header_totWidth = ((this.Drawer.MaxWidthWrite - 2 - textHeader.Length) / 2) + textHeader.Length;

                this.Drawer.WriteLine("┌" + (textHeader.PadLeft(header_totWidth, '─')).PadRight(this.Drawer.MaxWidthWrite - 2, '─') + "┐");

                for (int i = 0; i < this.Drawer.MaxHeightWrite - 2; i++)
                {
                    this.Drawer.WriteLine("│".PadRight(this.Drawer.MaxWidthWrite - 1, ' ') + "│");
                }

                this.Drawer.WriteLine("└" + "".PadLeft(this.Drawer.MaxWidthWrite - 2, '─') + "┘");


            }
            base.Draw();
        }
        public void Ok_btn()
        {
            string dest = this.dest_TextBox.Text;
            this.Panels.Copy(dest);
            this.Close = true;

        }
        public void Cancel_btn()
        {
            this.Close = true;

        }
    }
}
