﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace FacebookDesktopLogic
{
    public interface INote
    {
        Size Size { get; set; }
        Point Location { get; set; }
    }
}
