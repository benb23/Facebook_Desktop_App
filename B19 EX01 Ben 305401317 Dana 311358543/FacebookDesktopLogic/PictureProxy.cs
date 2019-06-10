using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace FacebookDesktopLogic
{
    public class PictureProxy : PictureBox
    {
        protected override void OnPaint(PaintEventArgs pe)
        {
            roundImage();
            base.OnPaint(pe);
        }

        private void roundImage()
        {
            System.Drawing.Drawing2D.GraphicsPath gp = new System.Drawing.Drawing2D.GraphicsPath();
            gp.AddEllipse(0, 0, this.Width, this.Height);
            Region rg = new Region(gp);
            this.Region = rg;
        }
    }
}
