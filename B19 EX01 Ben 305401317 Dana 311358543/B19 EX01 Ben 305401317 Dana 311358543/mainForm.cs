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
    public partial class mainForm : Form
    {
        private FacebookDesktopLogic m_FacebookDesktopLogic = FacebookDesktopLogic.GetFacebookDesktopLogic();
        private bool m_IsfriendListLoaded = false;
        private bool m_IsPostsLoaded = false;

        public mainForm()
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
        
        private void logInButton_Click_(object sender, EventArgs e)
        {
            bool isLogIn = m_FacebookDesktopLogic.LoginAndInit();
            if(isLogIn)
            {
                fetchUserInfo();
                tabControl.SelectedTab = tabPageHome;
                if (!m_IsPostsLoaded)
                {
                    fetchPosts();
                    m_IsPostsLoaded = true;
                }
            }
            else
            {
                MessageBox.Show("Error!"/*result.ErrorMessage*/); //todo: result?
            }
        }

        private void fetchUserInfo()
        {
            userPictureBox.LoadAsync(m_FacebookDesktopLogic.LoggedInUser.PictureNormalURL);
            userNametextBox.Text = m_FacebookDesktopLogic.LoggedInUser.Name;
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

        private void fetchPosts()
        {
            foreach (Post post in m_FacebookDesktopLogic.LoggedInUser.Posts)
            {
                if (post.Message != null)
                {
                    listBoxMyPosts.Items.Add(post.Message);
                }
                else if (post.Caption != null)
                {
                    listBoxMyPosts.Items.Add(post.Caption);
                }
                else
                {
                    listBoxMyPosts.Items.Add(string.Format("[{0}]", post.Type));
                }
            }

            if (m_FacebookDesktopLogic.LoggedInUser.Posts.Count == 0)
            {
                MessageBox.Show("No Posts to retrieve :(");
            }
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

        //private void label7_Click(object sender, EventArgs e)
        //{
        //    FacebookService.Logout(doAfterLogOut);
        //}

        private void doAfterLogOut()
        {

        }

        private void listBoxMyPosts_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void homeButton_Click(object sender, EventArgs e)
        {
            tabControl.SelectedTab = tabPageHome;
            if (!m_IsPostsLoaded)
            {
                fetchPosts();
                m_IsPostsLoaded = true;
            }
        }

        private void albumsButton_Click(object sender, EventArgs e)
        {
            tabControl.SelectedTab = tabPageAlbums;
            updateLatestPhotos();

        }

        private void updateLatestPhotos()
        {
            try
            {
                int albumIndex = 0;
                List<string> latestPhotos;

                foreach (Panel panel in tabPageAlbums.Controls)
                {
                    latestPhotos = this.m_FacebookDesktopLogic.GetLatestPhotosInAlbum(albumIndex, panel.Controls.Count);

                    while (latestPhotos.Count == 0)
                    {
                        albumIndex++;
                        latestPhotos = this.m_FacebookDesktopLogic.GetLatestPhotosInAlbum(albumIndex, panel.Controls.Count);
                    }

                    (panel.Controls[0] as Label).Text = m_FacebookDesktopLogic.LoggedInUser.Albums[albumIndex].Name;
                    int currentItem = 1;

                    foreach (string photo in latestPhotos)
                    {
                        if (currentItem >= panel.Controls.Count)
                        {
                            break;
                        }

                        (panel.Controls[currentItem] as PictureBox).LoadAsync(photo);
                        currentItem++;
                    }

                    albumIndex++;
                }
            }
            catch
            {
                MessageBox.Show("There was a problem loading the photos.", "Photos Problem", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            tabControl.SelectedTab = tabPageLogIn;
        }

        private void friendsButton_Click(object sender, EventArgs e)
        {
            tabControl.SelectedTab = tabPageFriends;
            if (!m_IsfriendListLoaded)
            {
                fetchFriends();
                m_IsfriendListLoaded = true;
            }
        }

        private void settingsButton_Click(object sender, EventArgs e)
        {
            tabControl.SelectedTab = tabPageSettings;
        }

        private void gameButton_Click(object sender, EventArgs e)
        {
            tabControl.SelectedTab = tabPageGame;
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void logOutButton_Click(object sender, EventArgs e)
        {
            FacebookService.Logout(doAfterLogOut);
        }

        private void homeButton_Click_1(object sender, EventArgs e)
        {
            tabControl.SelectedTab = tabPageHome;
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }
    }
}
