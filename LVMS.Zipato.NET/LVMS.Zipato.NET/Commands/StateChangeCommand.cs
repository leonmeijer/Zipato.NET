﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LVMS.Zipato.Commands
{
    public class StateChangeCommand : ICommand
    {
        public bool NewState { get; set; }
    }
}
