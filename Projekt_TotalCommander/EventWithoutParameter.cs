using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt_TotalCommander
{
    public class EventWithoutParameter : IDialogEvents
    {
        public event Action Function;
        public string Name { get; set; }

        public EventWithoutParameter(Action function,string name)
        {
            this.Function = function;
            this.Name = name;
        }

        public void ExecFunction()
        {
            this.Function();
        }
    }
}
