using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt_TotalCommander
{
    public class Button : IGraphicModule
    {
        //public bool UpdateVisual { get; set; } = false;
        public bool Redraw { get; set; } = true;



        public bool IsSelected { get; set; } = false;
        public int Height { get; set; }
        public int Width { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public ConsoleUtils2 Drawer { get; set; }
        public ConsoleColor Fore_Color { get; set; }
        public ConsoleColor Back_Color { get; set; }
        public ConsoleColor Selected_Color { get; set; }


        public string Name { get; set; }
        public ConsoleKey FuncKey { get; set; } = ConsoleKey.F12;
        public event Action Click;

        public Button(bool isSelected,int x,int y,int w,int h,string name, ConsoleColor fore_Col, ConsoleColor back_Col, ConsoleColor selected)
        {
            this.IsSelected = isSelected;
            this.X = x;
            this.Y = y;
            this.Width = w;
            this.Height = h;
            this.Name = name;
            this.Fore_Color = fore_Col;
            this.Back_Color = back_Col;
            this.Selected_Color = selected;
            this.Drawer = new ConsoleUtils2(x, y, w, h, fore_Col, back_Col);
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
                for (int i = 0; i < this.Drawer.MaxHeightWrite; i++)
                {
                    if (i == this.Drawer.MaxHeightWrite / 2)
                    {
                        this.Drawer.WriteLine((this.Name.PadLeft((this.Drawer.MaxWidthWrite - this.Name.Length) / 2 + this.Name.Length, ' ')).PadRight(this.Drawer.MaxWidthWrite, ' '));
                    }
                    else
                    {
                        this.Drawer.WriteLine("".PadRight(this.Drawer.MaxWidthWrite, ' '));
                    }
                }

                Redraw = false;
                //UpdateVisual = false;
            }
        }

        public void HandleKey(ConsoleKeyInfo key)
        {
            if(this.IsSelected && key.Key == ConsoleKey.Enter) //for containers
            {
                this.Click();
            }
            if (key.Key == this.FuncKey)
            {
                this.Click();
            }
        }

        public void Update() //neimplementováno
        {
            
        }
    }
}
