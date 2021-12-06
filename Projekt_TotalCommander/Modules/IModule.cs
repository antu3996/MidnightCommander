using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt_TotalCommander
{
    public interface IModule
    {
        public bool IsSelected { get; set; }
        public void Update();
        public void Draw();
        public void HandleKey(ConsoleKeyInfo key);
        public bool Redraw { get; set; }
    }
}
