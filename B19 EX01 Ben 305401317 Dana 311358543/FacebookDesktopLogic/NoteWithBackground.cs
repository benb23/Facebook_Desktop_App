using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace FacebookDesktopLogic
{
    public class NoteWithBackground : NoteDecorator
    {
        public NoteWithBackground(INote i_Note):base(i_Note)
        {

        }
    }
}
