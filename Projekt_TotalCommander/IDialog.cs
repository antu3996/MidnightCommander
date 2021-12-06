using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt_TotalCommander
{
    public interface IDialog
    {
        public event Action Function;
        public event Action<bool> Function2;

    }
}
