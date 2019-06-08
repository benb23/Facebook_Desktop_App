using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FacebookDesktopLogic
{
    public class NoteDecorator : INote
    {
        private INote m_Note;

        public NoteDecorator(INote i_Note)
        {
            m_Note = i_Note;
        }


        public void Show() { }

    }
}
