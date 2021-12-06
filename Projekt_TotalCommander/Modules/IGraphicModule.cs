using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt_TotalCommander
{
    public interface IGraphicModule : IModule
    {
        //public bool UpdateVisual { get; set; } //faster Draw()

        public int Width { get; set; }
        public int Height { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public ConsoleUtils2 Drawer { get; set; }
    }
}
