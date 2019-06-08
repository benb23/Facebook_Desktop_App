using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using FacebookWrapper;
using FacebookWrapper.ObjectModel;
using FacebookDesktopLogic;

namespace B19_EX01_Ben_305401317_Dana_311358543
{
    public partial class AppForm : Form
    {
        private PictureProxy m_ProfileRoundPictureBox;
        private PictureProxy m_Post0Picture;
        private PictureProxy m_Post1Picture;
        private PictureProxy m_Post2Picture;
        private PictureProxy m_Post3Picture;
        private Note m_Note = new Note(new Point(0,-8));

        private bool m_IsBirthdaysLoaded = false;

        public bool IsfriendListLoaded { get; set; }

        private bool IsPostsLoaded { get; set; }

        private enum ePostItem
        {
            NumOfLikes = 1,
            Message,
            Name,
            CreatedTime,
            Picture
        }

        private enum eMatch
        {
            FirstMatch,
            SecondMatch,
            thirdMatch
        }

        private enum eAlbumItem
        {
            AlbumNameIndex,
            FirstPhotoIndex
        }

        public AppForm()
        {
            this.InitializeComponent();
            this.notesPanel.Controls.Add(m_Note);


            m_ProfileRoundPictureBox = new PictureProxy();
            m_ProfileRoundPictureBox.BackColor = System.Drawing.Color.Transparent;
            m_ProfileRoundPictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            m_ProfileRoundPictureBox.Location = new System.Drawing.Point(29, 14);
            m_ProfileRoundPictureBox.Margin = new System.Windows.Forms.Padding(6);
            m_ProfileRoundPictureBox.Size = new System.Drawing.Size(184, 184);
            m_ProfileRoundPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            m_ProfileRoundPictureBox.TabIndex = 0;
            m_ProfileRoundPictureBox.TabStop = false;
            profilePicturePanel.Controls.Add(m_ProfileRoundPictureBox);

            m_Post0Picture = new PictureProxy();
            this.m_Post0Picture.BackColor = System.Drawing.Color.Gainsboro;
            this.m_Post0Picture.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.m_Post0Picture.Location = new System.Drawing.Point(12, 8);
            this.m_Post0Picture.Margin = new System.Windows.Forms.Padding(6);
            this.m_Post0Picture.Name = "post0Picture";
            this.m_Post0Picture.Size = new System.Drawing.Size(66, 66);
            this.m_Post0Picture.TabIndex = 2;
            this.m_Post0Picture.TabStop = false;
            this.post0.Controls.Add(this.m_Post0Picture);

            m_Post1Picture = new PictureProxy();
            this.m_Post1Picture.BackColor = System.Drawing.Color.Gainsboro;
            this.m_Post1Picture.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.m_Post1Picture.Location = new System.Drawing.Point(12, 8);
            this.m_Post1Picture.Margin = new System.Windows.Forms.Padding(6);
            this.m_Post1Picture.Name = "post1Picture";
            this.m_Post1Picture.Size = new System.Drawing.Size(66, 66);
            this.m_Post1Picture.TabIndex = 2;
            this.m_Post1Picture.TabStop = false;
            this.post1.Controls.Add(this.m_Post1Picture);

            m_Post3Picture = new PictureProxy();
            this.m_Post3Picture.BackColor = System.Drawing.Color.Gainsboro;
            this.m_Post3Picture.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.m_Post3Picture.Location = new System.Drawing.Point(12, 8);
            this.m_Post3Picture.Margin = new System.Windows.Forms.Padding(6);
            this.m_Post3Picture.Name = "post3Picture";
            this.m_Post3Picture.Size = new System.Drawing.Size(66, 66);
            this.m_Post3Picture.TabIndex = 2;
            this.m_Post3Picture.TabStop = false;
            this.post3.Controls.Add(this.m_Post3Picture);

            m_Post2Picture = new PictureProxy();
            this.m_Post2Picture.BackColor = System.Drawing.Color.Gainsboro;
            this.m_Post2Picture.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.m_Post2Picture.Location = new System.Drawing.Point(12, 8);
            this.m_Post2Picture.Name = "post3Picture";
            this.m_Post2Picture.Size = new System.Drawing.Size(66, 66);
            this.m_Post2Picture.TabIndex = 2;
            this.m_Post2Picture.TabStop = false;
            this.post2.Controls.Add(this.m_Post2Picture);

            FacebookAppLogic.Instance.AppSettings = AppSettings.LoadFromFile();
            this.Location = FacebookAppLogic.Instance.AppSettings.LastWindowLocation;
            checkBoxRememberMe.Checked = FacebookAppLogic.Instance.AppSettings.RememberUser;
        }

        private void logInButton_Click_(object sender, EventArgs e)
        {
            bool isLogIn = FacebookAppLogic.Instance.LoginAndInit();
            if(isLogIn)
            {
                this.fetchBasicUserInfo();
                loadHomeTab();
            }
            else
            {
                MessageBox.Show("Error!"); 
                tabControl.SelectedTab = this.tabPageLogIn;
            }
        }

        public void FetchFriends()
        {
            if (!this.IsfriendListLoaded)
            {
                FacebookAppLogic.Instance.FriendsList.Clear();
                listBoxFriends.Invoke(new Action(
                                        () => 
                                        {
                                            listBoxFriends.Items.Clear();
                                            listBoxFriends.DisplayMember = "Name";
                                        })); 

                foreach (User friend in FacebookAppLogic.Instance.LoggedInUser.Friends)
                {
                    FacebookAppLogic.Instance.FriendsList.Add(friend);
                    listBoxFriends.Invoke(new Action(() => listBoxFriends.Items.Add(friend)));
                    friend.ReFetch(DynamicWrapper.eLoadOptions.Full);
                }

                if (FacebookAppLogic.Instance.LoggedInUser.Friends.Count == 0)
                {
                    MessageBox.Show("No Friends to retrieve :(");
                }

                this.IsfriendListLoaded = true;
            }
        }

        private void fetchBasicUserInfo()
        {
            string profilePictureUrl = FacebookAppLogic.Instance.LoggedInUser.PictureNormalURL;
            FacebookAppLogic.Instance.PictureNormalURL = profilePictureUrl;
            m_ProfileRoundPictureBox.LoadAsync(profilePictureUrl);
            userNametextBox.Text = FacebookAppLogic.Instance.LoggedInUser.Name;
        }

        private void createPostButton_Click(object sender, EventArgs e)
        {
            FacebookAppLogic.Instance.LoggedInUser.PostStatus(textBox1.Text);
        }

        private void FetchRecentPosts()
        {
            int postIndex = 0;
            string profilePictureUrl = FacebookAppLogic.Instance.LoggedInUser.PictureNormalURL;
            string name = FacebookAppLogic.Instance.LoggedInUser.Name;
            Post post;

            if (!this.IsPostsLoaded)
            {
                if (FacebookAppLogic.Instance.LoggedInUser.Posts.Count > 0)
                {
                    for (int i = 0; i < postsPanel.Controls.Count; i++) 
                    {
                        post = FacebookAppLogic.Instance.LoggedInUser.Posts[postIndex];
                        postsPanel.Invoke(new Action(() => addPostToPostsPanel(i, post)));
                        postIndex++;
                    }
                }

            this.IsPostsLoaded = true;
        }
    }

        private void addPostToPostsPanel(int i_PostIndex, Post i_Post)
        {
            foreach(Control control in postsPanel.Controls)
            {
                control.Visible = true;
            }

            (postsPanel.Controls[i_PostIndex].Controls[(int)ePostItem.NumOfLikes] as Label).Text = i_Post.LikedBy.Count.ToString();
            (postsPanel.Controls[i_PostIndex].Controls[(int)ePostItem.Message] as Label).Text = i_Post.Message;
            (postsPanel.Controls[i_PostIndex].Controls[(int)ePostItem.Picture] as PictureBox).LoadAsync(i_Post.From.PictureNormalURL);
            (postsPanel.Controls[i_PostIndex].Controls[(int)ePostItem.Name] as Label).Text = i_Post.From.Name;
            (postsPanel.Controls[i_PostIndex].Controls[(int)ePostItem.CreatedTime] as Label).Text = i_Post.CreatedTime.Value.ToLongDateString();
        }

        private void loadHomeTab()
        {
            try
            {
                tabControl.SelectedTab = this.tabPageHome;
                new Thread(FetchFriends).Start();
                new Thread(FetchRecentPosts).Start();
            }
            catch
            {
                MessageBox.Show("There was a problem loading posts from Facebook.", "Problem", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void fetchLatestPhotos()
        {
            try
            {
                int albumIndex = 0;

                foreach (Panel panel in tabPageAlbums.Controls)
                {
                    tabPageAlbums.Invoke(new Action(() => addNewAlbumToAlbumsPanel(albumIndex, panel)));
                    albumIndex++;
                }
            }
            catch
            {
                MessageBox.Show("There was a problem loading the photos.", "Photos Problem", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void addNewAlbumToAlbumsPanel(int i_AlbumIndex, Panel i_AlbumPanel)
        {
            List<string> latestPhotos;

            latestPhotos = FacebookAppLogic.Instance.fetchLatestPhotosInAlbum(i_AlbumIndex, i_AlbumPanel.Controls.Count);

            (i_AlbumPanel.Controls[(int)eAlbumItem.AlbumNameIndex] as Label).Text = FacebookAppLogic.Instance.LoggedInUser.Albums[i_AlbumIndex].Name;
            int currentItem = (int)eAlbumItem.FirstPhotoIndex;

            foreach (string photo in latestPhotos)
            {
                if (currentItem >= i_AlbumPanel.Controls.Count)
                {
                    break;
                }

                (i_AlbumPanel.Controls[currentItem] as PictureBox).LoadAsync(photo);
                currentItem++;
            }
        }

        private void homePictureBox_Click(object sender, EventArgs e)
        {
            if (FacebookAppLogic.Instance.LoggedInUser != null)
            {
                tabControl.SelectedTab = this.tabPageHome;
                this.loadHomeTab(); 
            }
            else
            {
                MessageBox.Show("Please login !", "Login", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void albumsPictureBox_Click_1(object sender, EventArgs e)
        {
            if (FacebookAppLogic.Instance.LoggedInUser != null)
            {
                tabControl.SelectedTab = this.tabPageAlbums;
                this.fetchLatestPhotos();
            }
            else
            {
                MessageBox.Show("Please login !", "Login", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void labelLogOut_Click(object sender, EventArgs e)
        {
            if (IsfriendListLoaded && IsPostsLoaded)
            {
                try
                {
                    FacebookService.Logout(doAfterLogout);
                }
                catch
                {
                    MessageBox.Show("There was a problem. Try logging in again.", "Log-out Problem", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void doAfterLogout()
        {
            tabControl.SelectedTab = this.tabPageLogIn;
            cleanUserDataInUI();
        }

        private void cleanUserDataInUI()
        {
            m_ProfileRoundPictureBox.Image = null;
            userNametextBox.Text = string.Empty;

            foreach(Control control in postsPanel.Controls)
            {
                control.Visible = false;
            }

            listBoxFriends.Items.Clear();

            if (upcomingBirthdaysListBox.Items.Count > 0)
            {
                upcomingBirthdaysListBox.Items.Clear();
            }

            if (upcomingEventsListBox.Items.Count > 0)
            {
                upcomingEventsListBox.Items.Clear();
            }

            match2Name.Text = null;
            match2PictureBox = null;
            match3Name.Text = null;
            match3PictureBox = null;
            match1Name.Text = null;
            match1PictureBox = null;
            IsfriendListLoaded = false;
            IsPostsLoaded = false;
        }

        private void pictureBoxCalendar_Click(object sender, EventArgs e)
        {
            if (FacebookAppLogic.Instance.LoggedInUser != null)
            {
                tabControl.SelectedTab = this.tabPageCalendar;

                try
                {
                    loadCalendarTab();
                }
                catch
                {
                    MessageBox.Show("There was a problem loading information Facebook ", " Problem", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Please login !", "Login", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void FetchBirthdays()
        {
            if (!this.m_IsBirthdaysLoaded)
            {
                FacebookAppLogic.Instance.UpcomingBirthdaysUsers.Clear();
                foreach (User friend in FacebookAppLogic.Instance.FriendsList)
                {
                    if (DateTime.Parse(friend.Birthday).Month == DateTime.Now.Month)
                    {
                        FacebookAppLogic.Instance.UpcomingBirthdaysUsers.Add(friend);
                        upcomingBirthdaysListBox.Invoke(new Action(() => upcomingBirthdaysListBox.Items.Add(friend.Name + " " + friend.Birthday))); 
                    }
                }

                this.m_IsBirthdaysLoaded = true;
            }
        }

        private void loadCalendarTab()
        {
            FetchBirthdays();
            FetchEvents();
        }

        public void FetchEvents()
        {
            eventBindingSource.DataSource = FacebookAppLogic.Instance.LoggedInUser.Events;
            eventBindingSource.Filter = "Date = 'DateTime.Now.Month' "; // todo:??????
        }

        private void pictureBoxFaceCupid_Click(object sender, EventArgs e)
        {
            if (FacebookAppLogic.Instance.LoggedInUser != null)
            {
                tabControl.SelectedTab = this.tabPageCupid;
            }
            else
            {
                MessageBox.Show("Please login !", "Login", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private User.eGender? getCheckedGender()
        {
            User.eGender? gender;

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

        private void findMatchButtonPictureBox_Click(object sender, EventArgs e)
        {
            try
            {
                List<Candidate> cupidResult = FacebookAppLogic.Instance.FindMyMatch(this.getCheckedGender());
                int resIndex = 0;

                if (cupidResult != null && cupidResult.Count != 0)
                {
                    match1Name.Text = cupidResult[resIndex].User.Name;
                    match1PictureBox.LoadAsync(cupidResult[resIndex].User.PictureNormalURL);
                    scoreLabel1.Text = cupidResult[resIndex].Score.ToString();
                    resIndex++;

                    match2Name.Text = cupidResult[resIndex].User.Name;
                    match2PictureBox.LoadAsync(cupidResult[resIndex].User.PictureNormalURL);
                    scoreLabel2.Text = cupidResult[resIndex].Score.ToString();
                    resIndex++;

                    match3Name.Text = cupidResult[resIndex].User.Name;
                    match3PictureBox.LoadAsync(cupidResult[resIndex].User.PictureNormalURL);
                    scoreLabel3.Text = cupidResult[resIndex].Score.ToString();

                    choosMatchLabel.Visible = true;
                }
                else
                {
                    throw new Exception();
                }
            }
            catch
            {
                MessageBox.Show("There was a problem loading information from facebook.", "Problem", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void match2Panel_Click(object sender, EventArgs e)
        {
            this.updateChosenMatch(eMatch.SecondMatch);
        }

        private void match1Panel_Click(object sender, EventArgs e)
        {
            this.updateChosenMatch(eMatch.FirstMatch);
        }

        private void match3Panel_Click(object sender, EventArgs e)
        {
            this.updateChosenMatch(eMatch.thirdMatch);
        }

        private void updateChosenMatch(eMatch i_Match)
        {
            FacebookAppLogic.Instance.ChosenMatch = FacebookAppLogic.Instance.CupidResult[(int)i_Match];
            postOnMatchWallLabel.Visible = true;
            postOnMatchWallTextBox.Visible = true;
            sendMsgToMatchButton.Visible = true;
        }

        private void match2PictureBox_Click(object sender, EventArgs e)
        {
            this.updateChosenMatch(eMatch.SecondMatch);
        }

        private void match1PictureBox_Click(object sender, EventArgs e)
        {
            this.updateChosenMatch(eMatch.FirstMatch);
        }

        private void match3PictureBox_Click(object sender, EventArgs e)
        {
            this.updateChosenMatch(eMatch.thirdMatch);
        }

        private void postMsgOnMatchWallButton_Click(object sender, EventArgs e)
        {
            try
            {
                FacebookAppLogic.Instance.postOnMatchWall(postOnMatchWallTextBox.Text);
            }
            catch
            {
                MessageBox.Show("There was a problem posting on your friend's wall.", "Photos Problem", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void wishHappyBirthdayButton_Click(object sender, EventArgs e)
        {
            FacebookAppLogic.Instance.WishHappyBirthday(upcomingBirthdaysListBox.SelectedIndex);
        }

        private void goToFacebookLinkButton_Click(object sender, EventArgs e)
        {
            try
            {
                FacebookAppLogic.Instance.GoToFacebookLink(upcomingEventsListBox.SelectedIndex);
            }
            catch
            {
                MessageBox.Show("There was a problem loading link to Facebook event ", " Problem", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void addMissionButton_Click(object sender, EventArgs e)
        {
            toDoListCheckedListBox.Items.Add(newMissionTextBox.Text, false);
        }

        private void mainForm_Load(object sender, EventArgs e)
        {
            float h = Screen.PrimaryScreen.WorkingArea.Height / 1.55f;
            float w = Screen.PrimaryScreen.WorkingArea.Width / 1.3f;
            this.ClientSize = new Size((int)w, (int)h);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (this.checkBoxRememberMe.Checked && this.LogOutLabel.Enabled == true)
            {
                this.updateAppSettings();
            }
            else
            {
                if (FacebookAppLogic.Instance.LoggedInUser != null) 
                {
                    FacebookAppLogic.Instance.AppSettings.DeleteFile();
                }
            }
        }

        private void updateAppSettings()
        {
            FacebookAppLogic.Instance.AppSettings.LastWindowLocation = this.Location;
            FacebookAppLogic.Instance.AppSettings.RememberUser = this.checkBoxRememberMe.Checked;

            if (FacebookAppLogic.Instance.LoggedInUser != null)
            {          
                if(FacebookAppLogic.Instance.AppSettings.RememberUser)
                {
                    FacebookAppLogic.Instance.AppSettings.LastAccessToken = FacebookAppLogic.Instance.LoginResult.AccessToken;
                }
                else
                {
                    FacebookAppLogic.Instance.AppSettings.LastAccessToken = null;
                }
            }

            FacebookAppLogic.Instance.AppSettings.SaveToFile();
        }

        private void note1TextBox_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
