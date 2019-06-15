using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Timers;

namespace FacebookDesktopLogic
{
    public class Emphasizer
    {
        private IEmphasizable m_Emphasizable;
        private int k_Delta = 15;

        public void Emphasize(IEmphasizable i_Resizable)
        {
            m_Emphasizable = i_Resizable;
            Resize(k_Delta);

        }

        public void DeEmphasize(IEmphasizable i_Resizable)
        {
            m_Emphasizable = i_Resizable;
            Resize(k_Delta * -1);
        }


        private void Resize(int i_Delta)
        {
            m_Emphasizable.Size = new Size(m_Emphasizable.Size.Width + i_Delta, m_Emphasizable.Size.Height + i_Delta);
        }
    }
}

