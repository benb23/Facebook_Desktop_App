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
    public class PictureProxy : PictureBox, IEmphasizable
    {
        private Emphasizer m_Emphasizer;
        private bool m_IsEmphasized = false;
        private Size m_OriginalSize;

        public PictureProxy(Emphasizer i_SizeIncreaser)
        {
            this.m_Emphasizer = i_SizeIncreaser;
        }

        protected override void OnMouseHover(EventArgs e)
        {
            if (!m_IsEmphasized)
            {
                this.m_Emphasizer.Emphasize(this);
                m_IsEmphasized = true;
            }
            base.OnMouseHover(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            if (m_IsEmphasized)
            {
                this.m_Emphasizer.DeEmphasize(this);
                m_IsEmphasized = false;
            }
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
