using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace FacebookDesktopLogic
{
    public class Note : Panel,INote, IEmphasizable
    {
        private Emphasizer m_Emphasizer;

        public Note(Emphasizer i_Emphasizer)
        {
            InitializeComponent();
            this.m_Emphasizer = i_Emphasizer;

        }

        protected override void OnMouseHover(EventArgs e)
        {
            this.m_Emphasizer.Emphasize(this);
            base.OnMouseHover(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            this.m_Emphasizer.DeEmphasize(this);
            base.OnMouseLeave(e);
        }

        public PictureBox m_BackgroundPicture;

        public TextBox m_TextBox;

        private void InitializeComponent()
        {
            this.m_BackgroundPicture = new System.Windows.Forms.PictureBox();
            this.m_TextBox = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.m_BackgroundPicture)).BeginInit();
            this.SuspendLayout();
            // 
            // m_BackgroundPicture
            // 
            this.m_BackgroundPicture.BackgroundImage = global::FacebookDesktopLogic.Properties.Resources.note;
            this.m_BackgroundPicture.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.m_BackgroundPicture.Dock = System.Windows.Forms.DockStyle.Top;
            this.m_BackgroundPicture.Location = new System.Drawing.Point(0, 0);
            this.m_BackgroundPicture.Name = "m_BackgroundPicture";
            this.m_BackgroundPicture.Size = new System.Drawing.Size(170, 170);
            this.m_BackgroundPicture.TabIndex = 0;
            this.m_BackgroundPicture.TabStop = false;
            // 
            // m_TextBox
            // 
            this.m_TextBox.BackColor = Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.m_TextBox.Location = new System.Drawing.Point(25, 45);
            this.m_TextBox.Name = "m_TextBox";
            this.m_TextBox.Size = new System.Drawing.Size(this.Size.Width-100, 200);
            this.m_TextBox.TabIndex = 1;
            this.m_TextBox.Text = "test";
            // 
            // Note
            // 
            this.Controls.Add(this.m_BackgroundPicture);
            this.Controls.Add(this.m_TextBox);
            m_TextBox.BringToFront();

            this.Size = new System.Drawing.Size(170, 170);
            ((System.ComponentModel.ISupportInitialize)(this.m_BackgroundPicture)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
