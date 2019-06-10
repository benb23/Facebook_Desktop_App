using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FacebookDesktopLogic
{
    class NoteWithPrioritySign : NoteDecorator
    {
        NoteWithPrioritySign(INote i_Note): base(i_Note)
        {

        }
    }
}
