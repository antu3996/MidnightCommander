using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt_TotalCommander
{
    public interface IDialogEvents 
    {
        public string Name { get; set; }
        public void ExecFunction();
    }
}
