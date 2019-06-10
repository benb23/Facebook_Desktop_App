using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace FacebookDesktopLogic
{
    public class NoteWithHeader : NoteDecorator
    {
        public NoteWithHeader(INote i_Note, string i_Header) : base(i_Note)
        {
            m_HeaderTextBox.Location = this.m_Note.Location;
            m_HeaderTextBox.Text = i_Header;
        }

        public string Header
        {
            set { m_HeaderTextBox.Text = value; }
        }

        private TextBox m_HeaderTextBox = new TextBox() { Size = new Size(100, 100)};

        public override void Operation()
        {
            base.Operation();
            OperationX();
        }

        public void OperationX()
        {
            
        }
    }
}
