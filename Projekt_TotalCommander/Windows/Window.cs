using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Projekt_TotalCommander
{
    public abstract class Window
    {
        public List<IModule> modules { get; set; } = new List<IModule>();
        public int Height { get; set; }
        public int Width { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public bool Close { get; set; } = false;
        public ConsoleUtils2 Drawer { get; set; }
        //public bool FirstDraw { get; set; } = true;
        public bool RedrawAll { get; set; } = true;
        public Window(int x,int y,int w,int h, ConsoleColor fore_Col, ConsoleColor back_Col)
        {
            this.X = x;
            this.Y = y;
            this.Width = w;
            this.Height = h;
            this.Drawer = new ConsoleUtils2(x, y, w, h, fore_Col, back_Col);
        }
        public virtual void Draw()
        {
                foreach (IModule item in this.modules)
                {
                    if (this.RedrawAll)
                    {
                        item.Redraw = true;
                    }
                    item.Draw();
                }
                this.RedrawAll = false;
            
        }
        public virtual void HandleKey(ConsoleKeyInfo key)
        {
                foreach (IModule item in this.modules)
            {
                item.HandleKey(key);
            
        }
}
        public virtual void Update()
        {
                foreach (IModule item in this.modules)
                {
                    item.Update();
                }
            
        }
    }
}
