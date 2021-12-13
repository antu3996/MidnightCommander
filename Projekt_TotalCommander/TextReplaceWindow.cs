using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt_TotalCommander
{
    public class TextReplaceWindow : Window
    {
        private TextEditor TextEdit;
        private TextBox srcText;
        private TextBox repText;
        private Window refresh;


        public TextReplaceWindow(int x, int y, int w, int h, ConsoleColor fore_Col, ConsoleColor back_Col, TextEditor textedit,Window parent) : base(x, y, w, h, fore_Col, back_Col)
        {
            this.TextEdit = textedit;
            this.refresh = parent;
            string name1 = "Replace";
            TextBox textbox1 = new TextBox
                (false, this.X + 3, this.Y + 1, this.Width - 15, name1, "", ConsoleColor.Blue, ConsoleColor.Black, ConsoleColor.Green, ConsoleColor.Yellow, ConsoleColor.Magenta);

            string name2 = "with";
            TextBox textbox2 = new TextBox
                (false, this.X + 3, this.Y + 1+textbox1.Height + 1, this.Width - 15, name2, "", ConsoleColor.Blue, ConsoleColor.Black, ConsoleColor.Green, ConsoleColor.Yellow, ConsoleColor.Magenta);
            this.srcText = textbox1;
            this.repText = textbox2;
            Button replace = new Button
                (false, this.X + (this.Width / 4) - 10, this.Y + 1 + textbox1.Height + 1 + textbox2.Height + 1, 10, 1, "[ Replace ]", ConsoleColor.Black, ConsoleColor.DarkBlue, ConsoleColor.DarkGreen);
            Button all = new Button
                (false, this.X + (this.Width / 4)+2, this.Y + 1 + textbox1.Height + 1 + textbox2.Height + 1, 6, 1, "[ All ]", ConsoleColor.Black, ConsoleColor.DarkBlue, ConsoleColor.DarkGreen);
            Button skip = new Button
                (false, this.X + (this.Width / 4)+9, this.Y + 1 + textbox1.Height + 1 + textbox2.Height + 1, 8, 1, "[ Skip ]", ConsoleColor.Black, ConsoleColor.DarkBlue, ConsoleColor.DarkGreen);
            Button btnCancel = new Button
                (false, this.X + (this.Width / 4)+19, this.Y + 1 + textbox1.Height + 1 + textbox2.Height + 1, 10, 1, "[ Cancel ]", ConsoleColor.Black, ConsoleColor.DarkBlue, ConsoleColor.DarkGreen);

            replace.Click += Replace_btn;
            all.Click += ReplaceAll;
            skip.Click += Skip;
            btnCancel.Click += Cancel_btn;
            
            Container mainContainer = new Container(true, textbox1,textbox2, replace,all,skip, btnCancel);
            this.modules.Add(mainContainer);

        }
        public override void Draw()
        {
            this.refresh.Draw();
            this.RedrawAll = true;
            if (this.RedrawAll)
            {
                this.Drawer.Clear();
                this.Drawer.ResetToOrigin();

                string textHeader = "Replace";
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
        public void Replace_btn()
        {
            string srcText = this.srcText.Text;
            string toText = this.repText.Text;
            this.TextEdit.FindAndReplace(srcText,toText);
        }
        public void ReplaceAll()
        {
            string srcText = this.srcText.Text;
            string toText = this.repText.Text;
            this.TextEdit.ReplaceFromCursor(srcText, toText);
        }
        public void Skip()
        {
            string srcText = this.srcText.Text;
            this.TextEdit.SkipFind(srcText);
        }
        public void Cancel_btn()
        {
            this.Close = true;

        }
    }
}
