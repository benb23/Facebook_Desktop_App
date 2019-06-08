using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace FacebookDesktopLogic
{
    public class Note : Control, INote
    {
        public Note(Point m_Location)
        {
            this.Visible = true;
            m_BackgroundPicture.Location = m_Location;
            m_TextBox.Location = new Point(m_Location.X + 40, m_Location.Y + 85);
            m_BackgroundPicture.LoadAsync("note.png");
            this.Controls.Add(m_BackgroundPicture);
            this.Controls.Add(m_TextBox);
        }

        private PictureBox m_BackgroundPicture = new PictureBox() { Size = new Size(308, 302) };

        private TextBox m_TextBox = new TextBox() { Size = new Size(232, 169) , BackColor = Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128))))) }; 
    }
}
