using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using FacebookWrapper;
using FacebookWrapper.ObjectModel;
using FacebookAppLogic;

namespace B19_EX01_Ben_305401317_Dana_311358543
{
    public partial class mainForm : Form
    {
        private bool m_IsPostsListBoxLoaded = false;

        public mainForm()
        {
            InitializeComponent();
        }

        private void logInButton_Click_(object sender, EventArgs e)
        {
            tabControl.SelectedTab = tabPageloading;

            bool isLogIn = FacebookDesktopLogic.instance.LoginAndInit();
            if(isLogIn)
            {
                fetchUserInfo();
                if (!this.m_IsPostsListBoxLoaded)
                {
                    loadHomeTab();
                    this.m_IsPostsListBoxLoaded = true;
                }
                //FacebookDesktopLogic.instance.fetchFriends(); 
                tabControl.SelectedTab = tabPageHome;
            }
            else
            {
                MessageBox.Show("Error!"/*result.ErrorMessage*/); //todo: result?
                tabControl.SelectedTab = tabPageLogIn;
            }
        }

        private void fetchUserInfo()
        {
            string profilePictureUrl = FacebookDesktopLogic.instance.LoggedInUser.PictureNormalURL;
            FacebookDesktopLogic.instance.PictureNormalURL = profilePictureUrl;
            userPictureBox.LoadAsync(FacebookDesktopLogic.instance.LoggedInUser.PictureNormalURL);
            userPictureBox.BackColor = Color.Red;   // TODO: ????
            userNametextBox.Text = FacebookDesktopLogic.instance.LoggedInUser.Name;
        }

        private void initPostsPanel()
        {
            int postIndex = 0;
            string profilePictureUrl = FacebookDesktopLogic.instance.LoggedInUser.PictureNormalURL;
            string name = FacebookDesktopLogic.instance.LoggedInUser.Name;

            foreach (Panel post in postsPanel.Controls)
            {
                (post.Controls[1] as Label).Text = FacebookDesktopLogic.instance.RecentPosts[postIndex].LikedBy.Count.ToString();
                (post.Controls[2] as Label).Text = FacebookDesktopLogic.instance.RecentPosts[postIndex].Message;
                (post.Controls[3] as PictureBox).LoadAsync(profilePictureUrl);
                (post.Controls[4] as Label).Text = name;
                (post.Controls[5] as Label).Text = FacebookDesktopLogic.instance.RecentPosts[postIndex].CreatedTime.Value.ToLongDateString();

                postIndex++;
            }
            
        }
        private void createPostButton_Click(object sender, EventArgs e)
        {
            FacebookDesktopLogic.instance.LoggedInUser.PostStatus(textBox1.Text);
        }



        private void doAfterLogOut()
        {

        }

        private void loadHomeTab()
        {
            try
            {
                FacebookDesktopLogic.instance.fetchFriends();
                FacebookDesktopLogic.instance.fetchRecentPosts(postsPanel.Controls.Count);
                initFriendsListBox();
            }
            catch
            {
                MessageBox.Show("There was a problem loading posts from Facebook.", "Problem", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            initPostsPanel();
        }

        private void updateLatestPhotos()
        {
            try
            {
                int albumIndex = 0;
                List<string> latestPhotos;

                foreach (Panel panel in tabPageAlbums.Controls)
                {
                    latestPhotos = FacebookDesktopLogic.instance.GetLatestPhotosInAlbum(albumIndex, panel.Controls.Count);

                    while (latestPhotos.Count == 0)
                    {
                        albumIndex++;
                        latestPhotos = FacebookDesktopLogic.instance.GetLatestPhotosInAlbum(albumIndex, panel.Controls.Count);
                    }

                    (panel.Controls[0] as Label).Text = FacebookDesktopLogic.instance.LoggedInUser.Albums[albumIndex].Name;
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

        private void homePictureBox_Click(object sender, EventArgs e)
        {
            if (FacebookDesktopLogic.instance.LoggedInUser != null)
            {
                tabControl.SelectedTab = tabPageloading;
                loadHomeTab(); 
                tabControl.SelectedTab = tabPageHome;
            }
            else
            {
                MessageBox.Show("Please login !", "Login", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void albumsPictureBox_Click_1(object sender, EventArgs e)
        {
            if (FacebookDesktopLogic.instance.LoggedInUser != null)
            {
                tabControl.SelectedTab = tabPageloading;
                updateLatestPhotos();
                tabControl.SelectedTab = tabPageAlbums;
            }
            else
            {
                MessageBox.Show("Please login !", "Login", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //private void friendsPictureBox_Click(object sender, EventArgs e)
        //{
        //    tabControl.SelectedTab = tabPageloading;

        //    if (FacebookDesktopLogic.instance.LoggedInUser != null)
        //    {
        //        //FacebookDesktopLogic.instance.fetchFriends();
        //        initFriendsListBox();
        //        tabControl.SelectedTab = tabPageFriends;
        //    }
        //    else
        //    {
        //        tabControl.SelectedTab = tabPageLogIn;
        //        MessageBox.Show("Please login !", "Login", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //    }
        //}

        private void initFriendsListBox()
        {
            listBoxFriends.Items.Clear();
            listBoxFriends.DisplayMember = "Name";
            foreach (User friend in FacebookDesktopLogic.instance.FriendsList)
            {
                listBoxFriends.Items.Add(friend);
            }
        }

        private void labelLogOut_Click_1(object sender, EventArgs e)
        {
            FacebookService.Logout(doAfterLogOut);
        }

        private void pictureBoxCalendar_Click(object sender, EventArgs e)
        {
            tabControl.SelectedTab = tabPageloading;

            if (FacebookDesktopLogic.instance.LoggedInUser != null)
            {
                //FacebookDesktopLogic.instance.fetchFriends();
                Calendar.instance.FriendsList = FacebookDesktopLogic.instance.FriendsList;
                try
                {
                    Calendar.instance.fetchBirthdays();
                }
                catch
                {
                    MessageBox.Show("There was a problem loading friends birthdays from Facebook ", " Problem", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                initUpcomingBirthdaysListBox();

                try
                {
                    Calendar.instance.fetchEvents();
                }
                catch
                {
                    MessageBox.Show("There was a problem loading Events from Facebook ", " Problem", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                initUpcomingEventsListBox();
                tabControl.SelectedTab = tabPageCalendar;
            }
            else
            {
                MessageBox.Show("Please login !", "Login", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void initUpcomingBirthdaysListBox()
        {
            foreach (User user in Calendar.instance.UpcomingBirthdaysUsers)
            {
                upcomingBirthdaysListBox.Items.Add(user.Name + " " + user.Birthday);
            }
        }

        private void initUpcomingEventsListBox()
        {
            foreach (Event eventItem in Calendar.instance.UpcomingEvents)
            {
                upcomingEventsListBox.Items.Add(eventItem.Name + " " + eventItem.StartTime);
            }
        }

        private void pictureBoxFaceCupid_Click(object sender, EventArgs e)
        {
            tabControl.SelectedTab = tabPageloading;

            if (FacebookDesktopLogic.instance.LoggedInUser != null)
            {
                tabControl.SelectedTab = tabPageCupid;
            }
            else
            {
                MessageBox.Show("Please login !", "Login", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private User.eGender? getCheckedGender()
        {
            User.eGender? gender ;

            //todo : make sure the user choose gender!!!!!!!!!!! (else?)
            if (checkBoxFemale.Checked)
            {
                gender = User.eGender.female;
            }
            else if(checkBoxMale.Checked)
            {
                gender = User.eGender.male;
            }
            else
            {
                gender = null;
            }
            return gender;
        }

        private void updateCheckedFields()
        {
            if(checkBoxCheckIns.Checked)
            {
                FacebookCupid.instance.CheckCheckIns = true;
            }
            if (checkBoxEvents.Checked)
            {
                FacebookCupid.instance.CheckEvents = true;
            }
            if (checkBoxFieldOfStudy.Checked)
            {
                FacebookCupid.instance.CheckFieldOfStudy = true;
            }
            if (checkBoxFriends.Checked)
            {
                FacebookCupid.instance.CheckFriends = true;
            }
            if (checkBoxGroups.Checked)
            {
                FacebookCupid.instance.CheckGroups = true;
            }
            if (checkBoxHomeTown.Checked)
            {
                FacebookCupid.instance.CheckHomeTown = true;
            }
            if (checkBoxLikedPages.Checked)
            {
                FacebookCupid.instance.CheckLikedPages = true;
            }
        }
        //look
        private void findMatchButtonPictureBox_Click(object sender, EventArgs e)
        {
            updateCheckedFields();
            try
            {
                FacebookCupid.instance.FindMyMatch(getCheckedGender());
            }
            catch
            {
                MessageBox.Show("There was a problem loading information from Facebook.", "Problem", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            List<Candidate> cupidResult = FacebookCupid.instance.CupidResult;

            if (cupidResult != null && cupidResult.Count != 0)
            {
                match1Name.Text = cupidResult[0].User.Name;
                match1PictureBox.LoadAsync(cupidResult[0].User.PictureNormalURL);
                scoreLabel1.Text = cupidResult[0].Score.ToString();

                match2Name.Text = cupidResult[1].User.Name;
                match2PictureBox.LoadAsync(cupidResult[1].User.PictureNormalURL);
                scoreLabel2.Text = cupidResult[1].Score.ToString();

                match3Name.Text = cupidResult[2].User.Name;
                match3PictureBox.LoadAsync(cupidResult[2].User.PictureNormalURL);
                scoreLabel3.Text = cupidResult[2].Score.ToString();

                choosMatchLabel.Visible = true;
            }
            else
            {
                MessageBox.Show("There was a problem loading information from facebook.", "Problem", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void match2Panel_Click(object sender, EventArgs e)
        {
            updateChosenMatch(1);
        }

        private void match1Panel_Click(object sender, EventArgs e)
        {
            updateChosenMatch(0);
        }

        private void match3Panel_Click(object sender, EventArgs e)
        {
            updateChosenMatch(2);
        }

        private void updateChosenMatch(int i_index)
        {
            FacebookCupid.instance.ChosenMatch = FacebookCupid.instance.CupidResult[i_index];
            postOnMatchWallLabel.Visible = true;
            postOnMatchWallTextBox.Visible = true;
            sendMsgToMatchButton.Visible = true;
        }

        private void match2PictureBox_Click(object sender, EventArgs e)
        {
            updateChosenMatch(1);
        }

        private void match1PictureBox_Click(object sender, EventArgs e)
        {
            updateChosenMatch(0);
        }

        private void match3PictureBox_Click(object sender, EventArgs e)
        {
            updateChosenMatch(2);
        }

        private void postMsgOnMatchWallButton_Click(object sender, EventArgs e)
        {
            try
            {
                FacebookCupid.instance.postOnMatchWall(postOnMatchWallTextBox.Text);
            }
            catch
            {
                MessageBox.Show("There was a problem posting on your friend's wall.", "Photos Problem", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void wishHappyBirthdayButton_Click(object sender, EventArgs e)
        {
            Calendar.instance.wishHappyBirthday(upcomingBirthdaysListBox.SelectedIndex);
        }

        private void userPictureBox_BackColorChanged(object sender, EventArgs e)
        {
            System.Drawing.Drawing2D.GraphicsPath gp = new System.Drawing.Drawing2D.GraphicsPath();
            gp.AddEllipse(0, 0, userPictureBox.Width, userPictureBox.Height);
            Region rg = new Region(gp);
            userPictureBox.Region = rg;
        }

        private void goToFacebookLinkButton_Click(object sender, EventArgs e)
        {
            try
            {
                Calendar.instance.goToFacebookLink(upcomingEventsListBox.SelectedIndex);
            }
            catch
            {
                MessageBox.Show("There was a problem loading link to Facebook event ", " Problem", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void addMissionButton_Click(object sender, EventArgs e)
        {
            toDoListCheckedListBox.Items.Add(newMissionTextBox.Text,false);
        }

        private void mainForm_Load(object sender, EventArgs e)
        {
            float h = Screen.PrimaryScreen.WorkingArea.Height / 1.55f;
            float w = Screen.PrimaryScreen.WorkingArea.Width / 1.4f;
            this.ClientSize = new Size((int)w, (int)h);
        }
    }
}
