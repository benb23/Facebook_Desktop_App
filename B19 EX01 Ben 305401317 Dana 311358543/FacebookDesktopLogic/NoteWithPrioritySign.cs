using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace FacebookDesktopLogic
{
    public class NoteWithPrioritySign : NoteDecorator
    {
        public  NoteWithPrioritySign(INote i_Note) : base(i_Note)
        {
            m_PriorityPicture.Image = global::FacebookDesktopLogic.Properties.Resources.sign;
            m_PriorityPicture.Location = new Point(this.Location.X + this.Size.Width/2 + 25 ,this.Location.Y + this.Size.Height/2+15);
            this.Controls.Add(m_PriorityPicture);
            m_PriorityPicture.BringToFront();
        }

        private PictureBox m_PriorityPicture = new PictureBox() { Size = new Size(39, 39) };

    }
}
