﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt_TotalCommander
{
    public interface IDialog
    {
        public IDialogEvents Event { get; set; }

    }
}
