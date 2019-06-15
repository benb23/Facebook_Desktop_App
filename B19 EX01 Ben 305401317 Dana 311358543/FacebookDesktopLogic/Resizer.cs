using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Timers;

namespace FacebookDesktopLogic
{
    public class Resizer
    {
        private IResizable m_Resizable;
        private int k_Delta = 15;

        public void IncreaseSize(IResizable i_Resizable)
        {
            Resize(i_Resizable, k_Delta);
        }

        public void DecreaseSize(IResizable i_Resizable)
        {
            Resize(i_Resizable, k_Delta*-1);
        }

        private void Resize(IResizable i_Resizable, int i_Delta)
        {
            m_Resizable = i_Resizable;
            m_Resizable.Size = new Size(m_Resizable.Size.Width + i_Delta, m_Resizable.Size.Height + i_Delta);
        }
    }
}

