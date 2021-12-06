using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Projekt_TotalCommander
{
    public class Container : IModule
    {
        public bool Redraw { get; set; } = true;
        public bool IsSelected { get; set; } = false;
        public List<IModule> Modules { get; set; } = new List<IModule>();
        public int Selected { get; set; } = 0;
        public ConsoleKey NextKey { get; set; } = ConsoleKey.Tab;

        public Container(bool isSelected,params IModule[] modules)
        {
            this.IsSelected = isSelected;
            foreach (IModule item in modules)
            {
                this.Modules.Add(item);
            }
        }
        public void Draw()
        {
            foreach (IModule item in this.Modules)
            {
                if (this.Redraw)
                {
                    item.Redraw = true;
                }
                item.Draw();
            }
            this.Redraw = false;
        }

        public void HandleKey(ConsoleKeyInfo key)
        {
            if(this.IsSelected)
            {
                if (key.Key == this.NextKey)
                {
                    this.Modules[this.Selected].IsSelected = false;

                    //if (this.Modules[this.Selected] is IGraphicModule)
                    //{
                    //    ((IGraphicModule)this.Modules[this.Selected]).UpdateVisual = true;
                    //}
                    this.Modules[this.Selected].Redraw = true;
                    //?? Redraw X UpdateVisual

                    this.Selected = (this.Selected + 1) % this.Modules.Count;
                    this.Modules[this.Selected].IsSelected = true;
                }
                else
                {
                    this.Modules[this.Selected].HandleKey(key);
                }
            }
            //else if (key.Key == ConsoleKey.Enter)
            //{
            //    this.SelectedModule.HandleKey(key);
            //}
        }

        public void Update()
        {
            if (this.IsSelected)
            {
                this.Modules[this.Selected].IsSelected = true;
            }
            foreach (IModule item in this.Modules)
            {
                item.Update();
            }

        }
    }
}
