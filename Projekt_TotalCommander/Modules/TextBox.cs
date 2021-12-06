using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt_TotalCommander
{
    public class TextBox : IGraphicModule
    {
        //public bool UpdateVisual { get; set; } = false;
        public bool Redraw { get; set; } = true;


        public bool IsSelected { get; set; } = false;
        public int Height { get; set; } = 2;
        public int Width { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public ConsoleUtils2 Drawer { get; set; }
        public ConsoleColor Fore_Color { get; set; }
        public ConsoleColor Back_Color { get; set; }
        public ConsoleColor Selected_Color { get; set; }
        public ConsoleColor TextBar_fore { get; set; }
        public ConsoleColor TextBar_back { get; set; }


        public string Name { get; set; }
        public string Text { get; set; }

        public TextBox(bool isSelected,int x, int y, int w,string name,string text,ConsoleColor fore_Col,ConsoleColor back_Col,ConsoleColor selected, ConsoleColor textbar_fore,ConsoleColor textbar_back)
        {
            this.IsSelected = isSelected;
            this.X = x;
            this.Y = y;
            this.Width = w;
            this.Name = name;
            this.Text = text;
            this.Fore_Color = fore_Col;
            this.Back_Color = back_Col;
            this.Selected_Color = selected;
            this.TextBar_fore = textbar_fore;
            this.TextBar_back = textbar_back;
            this.Drawer = new ConsoleUtils2(x, y, w, this.Height,fore_Col,back_Col);
        }

        public void Draw()
        {
            if (IsSelected /*|| UpdateVisual*/ || Redraw)
            {
                this.Drawer.ResetToOrigin();
                if (this.IsSelected)
                {
                    this.Drawer.ForeColor = this.Selected_Color;
                }
                else
                {
                    this.Drawer.ForeColor = this.Fore_Color;
                }

                //Console.Clear(0, 0);
                if (this.Name.Length > this.Drawer.MaxWidthWrite)
                {
                    this.Drawer.WriteLine(this.Name.Substring(0, this.Drawer.MaxWidthWrite));
                }
                else
                {
                    this.Drawer.WriteLine(this.Name.PadRight(this.Drawer.MaxWidthWrite, ' '));
                }
                this.Drawer.BackColor = this.TextBar_back;
                this.Drawer.ForeColor = this.TextBar_fore;
                if (this.Text.Length <= this.Drawer.MaxWidthWrite)
                {
                    this.Drawer.WriteLine(this.Text.PadRight(this.Drawer.MaxWidthWrite, ' '));
                }
                else
                {
                    this.Drawer.WriteLine(this.Text.Substring(0, this.Drawer.MaxWidthWrite));
                }
                this.Drawer.BackColor = this.Back_Color;
                this.Drawer.ForeColor = this.Fore_Color;

                Redraw = false;
                //UpdateVisual = false;
            }
        }

        public void HandleKey(ConsoleKeyInfo key)
        {
            if (this.IsSelected)
            {
                if (key.Key == ConsoleKey.Backspace)
                {
                    if (this.Text.Length > 0)
                    {
                        this.Text = this.Text.Remove(this.Text.Length - 1);
                    }
                }
                else
                {
                    if (!char.IsControl(key.KeyChar))
                    {
                        this.Text += key.KeyChar;
                    }
                }
            }
        }

        public void Update() //neimplementováno 
        {
        }
    }
}
