using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt_TotalCommander
{
    public class DeleteWindow : Window, IFunctionalWindow
    {
        public MainPanels Panels { get; set; }


        public DeleteWindow(int x, int y, int w, int h, ConsoleColor fore_Col, ConsoleColor back_Col, MainPanels panels) : base(x, y, w, h, fore_Col, back_Col)
        {
            this.Panels = panels;

            Button btnOk = new Button(false, this.X+(this.Width / 2)-7 , this.Y+this.Height-2, 6, 1, "[ Ok ]", ConsoleColor.Black,ConsoleColor.Cyan,ConsoleColor.Yellow);
            Button btnCancel = new Button(false, this.X+(this.Width / 2), this.Y+this.Height - 2, 10, 1, "[ Cancel ]", ConsoleColor.Black, ConsoleColor.Cyan, ConsoleColor.Yellow);

            btnOk.Click += Ok_btn;
            btnCancel.Click += Cancel_btn;

            Container mainContainer = new Container(true, btnOk, btnCancel);
            this.modules.Add(mainContainer);
        }


        public override void Draw()
        {
            if (this.RedrawAll || Active)
            {
                this.Drawer.Clear();
                this.Drawer.ResetToOrigin();

                string textHeader = "Delete";
                int header_totWidth = ((this.Drawer.MaxWidthWrite - 2 - textHeader.Length) / 2) + textHeader.Length;

                this.Drawer.WriteLine("┌" + (textHeader.PadLeft(header_totWidth, '─')).PadRight(this.Drawer.MaxWidthWrite - 2, '─') + "┐");

                for (int i = 0; i < this.Drawer.MaxHeightWrite - 2; i++)
                {
                    if (i == 1)
                    {
                        this.Drawer.WriteLine("│" + (textHeader.PadLeft(header_totWidth, ' ')).PadRight(this.Drawer.MaxWidthWrite - 2, ' ') + "│");
                    }
                    if (i == 2)
                    {
                        string temp = this.Panels.Get_Selected_Sys.Get_Selected_Name_UNI;
                        int temp_totWidth = ((this.Drawer.MaxWidthWrite - 2 - temp.Length) / 2) + temp.Length;
                        this.Drawer.WriteLine("│" + ($"'{temp}' ?".PadLeft(temp_totWidth, ' ')).PadRight(this.Drawer.MaxWidthWrite - 2, ' ') + "│");
                    }
                    else
                    {
                        this.Drawer.WriteLine("│".PadRight(this.Drawer.MaxWidthWrite - 1, ' ') + "│");
                    }
                }

                this.Drawer.WriteLine("└" + "".PadLeft(this.Drawer.MaxWidthWrite - 2, '─') + "┘");



            }
            base.Draw();
        }
        public void Ok_btn()
        {
            //this.Close = true;
            this.Panels.Delete(false);
            this.Close = true;
        }
        public void Cancel_btn()
        {
            this.Close = true;
        }
    }
}
