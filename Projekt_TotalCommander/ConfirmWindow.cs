using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt_TotalCommander
{
    public class ConfirmWindow : Window, IDialog
    {
        private string Msg;
        public event Action Function;
        public event Action<bool> Function2;

        public ConfirmWindow(int x, int y, int w, int h, ConsoleColor fore_Col, ConsoleColor back_Col, string message) : base(x, y, w, h, fore_Col, back_Col)
        {
            this.Msg = message;
            Button btnOk = new Button(false, this.X + (this.Width / 2) - 7, this.Y + this.Height - 2, 6, 1, "[ Ok ]", ConsoleColor.Black, ConsoleColor.Cyan, ConsoleColor.Yellow);
            Button btnCancel = new Button(false, this.X + (this.Width / 2), this.Y + this.Height - 2, 10, 1, "[ Cancel ]", ConsoleColor.Black, ConsoleColor.Cyan, ConsoleColor.Yellow);

            btnOk.Click += Ok_btn;
            btnCancel.Click += Cancel_btn;

            Container mainContainer = new Container(true, btnOk, btnCancel);
            this.modules.Add(mainContainer);
        }


        public override void Draw()
        {
            if (this.RedrawAll)
            {
                this.Drawer.Clear();
                this.Drawer.ResetToOrigin();

                string textHeader = this.Msg;
                int header_totWidth = ((this.Drawer.MaxWidthWrite - 2 - textHeader.Length) / 2) + textHeader.Length;

                this.Drawer.WriteLine("┌" + (textHeader.PadLeft(header_totWidth, '─')).PadRight(this.Drawer.MaxWidthWrite - 2, '─') + "┐");

                for (int i = 0; i < this.Drawer.MaxHeightWrite - 2; i++)
                {
                    if (i == 1)
                    {
                        this.Drawer.WriteLine("│" + ("Confirm action?".PadLeft(header_totWidth, ' ')).PadRight(this.Drawer.MaxWidthWrite - 2, ' ') + "│");
                    }
                    else
                    {
                        this.Drawer.WriteLine("│".PadRight(this.Drawer.MaxWidthWrite - 1, ' ') + "│");
                    }
                }

                this.Drawer.WriteLine("└" + "".PadLeft(this.Drawer.MaxWidthWrite - 2, '─') + "┘");

                this.Clear = false;

            }
            base.Draw();
        }
        public void Ok_btn()
        {
            if (this.Function != null)
            {
                this.Function();
            }
            if (this.Function2 != null)
            {
                this.Function2(true);
            }
            this.Close = true;

        }
        public void Cancel_btn()
        {
            this.Close = true;

        }
    }
}
