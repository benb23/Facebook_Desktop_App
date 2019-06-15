using System.Windows.Forms;

namespace FacebookDesktopLogic
{
    public class NoteDecorator : Panel ,INote
    {
        protected INote m_Note; 

        public NoteDecorator(INote i_Note)
        {
            m_Note = i_Note;
            this.Controls.Add(m_Note as Control);
            this.Size = m_Note.Size;
        }

        public virtual void Operation() { }
    }
}
