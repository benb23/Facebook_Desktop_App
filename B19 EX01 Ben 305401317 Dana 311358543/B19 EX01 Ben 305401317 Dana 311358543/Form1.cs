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
using FacebookWrapper;
using FacebookWrapper.ObjectModel;

namespace B19_EX01_Ben_305401317_Dana_311358543
{
    public partial class Form1 : Form
    {
        private FacebookDesktopLogic m_FacebookDesktopLogic = FacebookDesktopLogic.GetFacebookDesktopLogic();
        private bool m_IsfriendListLoaded = false;

        public Form1()
        {
            InitializeComponent();
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

        private void post_TextChanged(object sender, EventArgs e)
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

        private void tabPageFriends_Click(object sender, EventArgs e)
        {
            if (!m_IsfriendListLoaded)
            {
                fetchFriends();
                m_IsfriendListLoaded = true;
            }
        }

        private void fetchFriends()
        {
            listBoxFriends.Items.Clear();
            listBoxFriends.DisplayMember = "Name";
            foreach (User friend in m_FacebookDesktopLogic.LoggedInUser.Friends)
            {
                listBoxFriends.Items.Add(friend);
                friend.ReFetch(DynamicWrapper.eLoadOptions.Full);
            }

            if (m_FacebookDesktopLogic.LoggedInUser.Friends.Count == 0)
            {
                MessageBox.Show("No Friends to retrieve :(");
            }
        }

        private void tabPageHome_Click(object sender, EventArgs e)
        {
            
        }

        private void createPostButton_Click(object sender, EventArgs e)
        {
            m_FacebookDesktopLogic.LoggedInUser.PostStatus(textBox1.Text);
        }

        private void friendsList_TextChanged(object sender, EventArgs e)
        {

        }

        private void friendsList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
