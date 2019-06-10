using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace FacebookDesktopLogic
{
    public class NoteDecorator : Panel ,INote
    {
        protected INote m_Note; //todo: protected?

        public NoteDecorator(INote i_Note)
        {
            m_Note = i_Note;
            this.Controls.Add(m_Note as Control);
        }

        public virtual void Operation() { }

    }
}
