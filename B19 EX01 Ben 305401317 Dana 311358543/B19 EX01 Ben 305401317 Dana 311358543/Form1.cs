using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FacebookWrapper;


namespace B19_EX01_Ben_305401317_Dana_311358543
{
    public partial class Form1 : Form
    {
        private FacebookDesktopLogic m_FacebookDesktopLogic = FacebookDesktopLogic.GetFacebookDesktopLogic();

        public Form1()
        {
            InitializeComponent();
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox4_Click_1(object sender, EventArgs e)
        {
            bool isLogIn = m_FacebookDesktopLogic.LoginAndInit();
            if(isLogIn)
            {
                fetchUserInfo();
            }
            else
            {
                MessageBox.Show("Error!"/*result.ErrorMessage*/); //todo: result?
            }
        }

        private void fetchUserInfo()
        {
            pictureBox2.LoadAsync(m_FacebookDesktopLogic.LoggedInUser.PictureNormalURL);
            textBox2.Text = m_FacebookDesktopLogic.LoggedInUser.Name;
            //if (m_FacebookDesktopLogic.LoggedInUser.Posts.Count > 0)
            //{
            //    textBoxStatus.Text = m_FacebookDesktopLogic.LoggedInUser.Posts[0].Message;
            //}
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
