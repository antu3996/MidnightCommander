using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt_TotalCommander
{
    public class FolderTable_Row
    {
        public List<string> Values { get; set; }
        public ConsoleColor Row_Color { get; set; } = ConsoleColor.Black;
        public FolderTable_Row(params string[] values)
        {
            this.Values = values.ToList();
        }
    }
}
