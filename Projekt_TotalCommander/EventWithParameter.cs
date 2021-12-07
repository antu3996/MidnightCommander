using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt_TotalCommander
{
    public class EventWithParameter : IDialogEvents
    {
        public event Action<bool> Function;
        public string Name { get; set; }

        public EventWithParameter(Action<bool> function,string name)
        {
            this.Function = function;
            this.Name = name;
        }

        public void ExecFunction()
        {
            this.Function(true);
        }

    }
}
