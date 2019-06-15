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
    public class PictureProxy : PictureBox, IResizable
    {
        private Resizer m_Resizer;

        public PictureProxy(Resizer i_SizeIncreaser)
        {
            this.m_Resizer = i_SizeIncreaser;
        }

        protected override void OnMouseHover(EventArgs e)
        {
            this.m_Resizer.IncreaseSize(this);
            base.OnMouseHover(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            this.m_Resizer.DecreaseSize(this);
            base.OnMouseLeave(e);
        }
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
