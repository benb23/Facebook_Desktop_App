using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace FacebookDesktopLogic
{
    class NoteWithPrioritySign : NoteDecorator
    {
        public NoteWithPrioritySign(INote i_Note, string i_Header) : base(i_Note)
        {
            this.Controls.Add(m_HeaderTextBox);
            m_HeaderTextBox.Location = new Point(this.m_Note.Location.X, this.m_Note.Location.Y + 10);
            m_HeaderTextBox.Text = i_Header;
            m_HeaderTextBox.BringToFront();
        }

        public string Header
        {
            set { m_HeaderTextBox.Text = value; }
        }

        private TextBox m_HeaderTextBox = new TextBox() { Size = new Size(100, 100) };

    }
}
