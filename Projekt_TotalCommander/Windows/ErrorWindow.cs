using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt_TotalCommander
{
    public class ErrorWindow : Window
    {
        private string error_Msg;
        public ErrorWindow(int x, int y, int w, int h, ConsoleColor fore_Col, ConsoleColor back_Col,string errorMessage) : base(x, y, w, h, fore_Col, back_Col)
        {
            this.error_Msg = errorMessage;

            Button escape = new Button(true, 0, 0, 0, 0, string.Empty,ConsoleColor.Black, ConsoleColor.Black, ConsoleColor.Black);
            escape.Click += Escape_btn;
            escape.FuncKey = ConsoleKey.Escape;
            this.modules.Add(escape);
        }


        public override void Draw()
        {
            if (this.RedrawAll)
            {
                this.Drawer.Clear();
                this.Drawer.ResetToOrigin();

                string textHeader = "Error";
                int header_totWidth = ((this.Drawer.MaxWidthWrite - 2 - textHeader.Length) / 2) + textHeader.Length;

                this.Drawer.WriteLine("┌" + (textHeader.PadLeft(header_totWidth, '─')).PadRight(this.Drawer.MaxWidthWrite - 2, '─') + "┐");

                for (int i = 0; i < this.Drawer.MaxHeightWrite - 2; i++)
                {
                    if (i == (this.Drawer.MaxHeightWrite / 2) - 1)
                    {
                        int msg_totWidth = ((this.Drawer.MaxWidthWrite - 2 - this.error_Msg.Length) / 2) + this.error_Msg.Length;
                        this.Drawer.WriteLine("│" + (this.error_Msg.PadLeft(msg_totWidth, ' ')).PadRight(this.Drawer.MaxWidthWrite - 2, ' ') + "│");
                    }
                    else
                    {
                        this.Drawer.WriteLine("│".PadRight(this.Drawer.MaxWidthWrite - 1, ' ') + "│");
                    }
                }

                this.Drawer.WriteLine("└" + "".PadLeft(this.Drawer.MaxWidthWrite - 2, '─') + "┘");


            }
        }
        public void Escape_btn()
        {
            this.Close = true;

        }
    }
}
