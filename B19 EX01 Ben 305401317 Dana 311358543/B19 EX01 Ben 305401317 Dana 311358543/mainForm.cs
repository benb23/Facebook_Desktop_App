using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using FacebookWrapper;
using FacebookWrapper.ObjectModel;
using FacebookAppLogic;
using System.Threading;

namespace B19_EX01_Ben_305401317_Dana_311358543
{
    public partial class mainForm : Form
    {
        private bool m_IsEventsLoaded = false;
        private bool m_IsBirthdaysLoaded = false;

        public bool IsfriendListLoaded { get; set; }
        private bool IsPostsLoaded { get; set; }


        private enum ePostItem
        {
            NumOfLikes = 1,
            Message,
            Picture,
            Name,
            CreatedTime
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

        //private bool m_IsPostsListBoxLoaded = false;

        private FacebookDesktopLogic m_FacebookDesktopLogic = FacebookDesktopLogic.Instance;

        public mainForm()
        {
            this.InitializeComponent();
            this.m_FacebookDesktopLogic.AppSettings = AppSettings.LoadFromFile();
            this.Location = this.m_FacebookDesktopLogic.AppSettings.LastWindowLocation;
            checkBoxRememberMe.Checked = this.m_FacebookDesktopLogic.AppSettings.RememberUser;
        }

        private void logInButton_Click_(object sender, EventArgs e)
        {
            bool isLogIn = this.m_FacebookDesktopLogic.LoginAndInit();
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
                listBoxFriends.Invoke(new Action(
                                        () => 
                                        {
                                            listBoxFriends.Items.Clear();
                                            listBoxFriends.DisplayMember = "Name";
                                        })); 

                foreach (User friend in this.m_FacebookDesktopLogic.LoggedInUser.Friends)
                {
                    this.m_FacebookDesktopLogic.FriendsList.Add(friend);
                    listBoxFriends.Invoke(new Action(() => listBoxFriends.Items.Add(friend)));
                    friend.ReFetch(DynamicWrapper.eLoadOptions.Full);
                }

                if (this.m_FacebookDesktopLogic.LoggedInUser.Friends.Count == 0)
                {
                    MessageBox.Show("No Friends to retrieve :(");
                }

                this.IsfriendListLoaded = true;
            }
        }

        private void fetchBasicUserInfo()
        {
            string profilePictureUrl = this.m_FacebookDesktopLogic.LoggedInUser.PictureNormalURL;
            this.m_FacebookDesktopLogic.PictureNormalURL = profilePictureUrl;
            profilePictureBox.LoadAsync(profilePictureUrl);
            this.roundImage();
            userNametextBox.Text = this.m_FacebookDesktopLogic.LoggedInUser.Name;
        }

        private void createPostButton_Click(object sender, EventArgs e)
        {
            this.m_FacebookDesktopLogic.LoggedInUser.PostStatus(textBox1.Text);
        }


        public void FetchRecentPosts()
        {
            int postIndex = 0;
            string profilePictureUrl = this.m_FacebookDesktopLogic.LoggedInUser.PictureNormalURL;
            string name = this.m_FacebookDesktopLogic.LoggedInUser.Name;
            Post post;

            
            if (!this.IsPostsLoaded)
            {
            for (int i=0 ; i < postsPanel.Controls.Count ; i++) //??
            {
                this.m_FacebookDesktopLogic.RecentPosts.Add(this.m_FacebookDesktopLogic.LoggedInUser.Posts[postIndex]);
                post = this.m_FacebookDesktopLogic.RecentPosts[postIndex];
                postsPanel.Invoke(new Action(()=> addPostToPostsPanel(i,post)));
                postIndex++;
            }

            this.IsPostsLoaded = true;
        }

    }

        private void addPostToPostsPanel(int i_PostIndex, Post i_Post)
        {
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

                    tabPageAlbums.Invoke(new Action(()=> addNewAlbumToAlbumsPanel(albumIndex, panel)));
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

            latestPhotos = this.m_FacebookDesktopLogic.fetchLatestPhotosInAlbum(i_AlbumIndex, i_AlbumPanel.Controls.Count);

            (i_AlbumPanel.Controls[(int)eAlbumItem.AlbumNameIndex] as Label).Text = this.m_FacebookDesktopLogic.LoggedInUser.Albums[i_AlbumIndex].Name;
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
            if (this.m_FacebookDesktopLogic.LoggedInUser != null)
            {
                this.loadHomeTab(); 
            }
            else
            {
                MessageBox.Show("Please login !", "Login", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void albumsPictureBox_Click_1(object sender, EventArgs e)
        {
            if (this.m_FacebookDesktopLogic.LoggedInUser != null)
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
            try
            {
                FacebookService.Logout(null);
                tabControl.SelectedTab = this.tabPageLogIn;
                this.m_FacebookDesktopLogic.LoggedInUser = null;
                profilePictureBox.Image = null;
                userNametextBox.Text = string.Empty;
            }
            catch
            {
                MessageBox.Show("There was a problem. Try logging in again.", "Log-out Problem", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void pictureBoxCalendar_Click(object sender, EventArgs e)
        {
            if (this.m_FacebookDesktopLogic.LoggedInUser != null)
            {
                tabControl.SelectedTab = this.tabPageCalendar;
                this.m_FacebookDesktopLogic.Calendar.FriendsList = this.m_FacebookDesktopLogic.FriendsList;

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

        public void FetchBirthdays()
        {
            if (!this.m_IsBirthdaysLoaded)
            {
                foreach (User friend in m_FacebookDesktopLogic.Calendar.FriendsList)
                {
                    if (DateTime.Parse(friend.Birthday).Month == DateTime.Now.Month)
                    {
                        this.m_FacebookDesktopLogic.Calendar.UpcomingBirthdaysUsers.Add(friend);
                        upcomingBirthdaysListBox.Invoke(new Action(()=> upcomingBirthdaysListBox.Items.Add(friend.Name + " " + friend.Birthday))); 
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
            if (!this.m_IsEventsLoaded)
            {
                foreach (Event eventItem in this.m_FacebookDesktopLogic.LoggedInUser.Events)
                {
                    if (eventItem.StartTime.Value.Month == DateTime.Now.Month)
                    {
                        this.m_FacebookDesktopLogic.Calendar.UpcomingEvents.Add(eventItem);
                        upcomingEventsListBox.Invoke(new Action(()=> upcomingEventsListBox.Items.Add(eventItem.Name + " " + eventItem.StartTime)));
                    }
                }

                this.m_IsEventsLoaded = true;
            }
        }

        private void pictureBoxFaceCupid_Click(object sender, EventArgs e)
        {
            if (this.m_FacebookDesktopLogic.LoggedInUser != null)
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
                this.m_FacebookDesktopLogic.Cupid.FindMyMatch(this.getCheckedGender());
                List<Candidate> cupidResult = this.m_FacebookDesktopLogic.Cupid.CupidResult;
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
            this.m_FacebookDesktopLogic.Cupid.ChosenMatch = this.m_FacebookDesktopLogic.Cupid.CupidResult[(int)i_Match];
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
                this.m_FacebookDesktopLogic.Cupid.postOnMatchWall(postOnMatchWallTextBox.Text);
            }
            catch
            {
                MessageBox.Show("There was a problem posting on your friend's wall.", "Photos Problem", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void wishHappyBirthdayButton_Click(object sender, EventArgs e)
        {
            this.m_FacebookDesktopLogic.Calendar.WishHappyBirthday(upcomingBirthdaysListBox.SelectedIndex);
        }

        private void roundImage()
        {
            System.Drawing.Drawing2D.GraphicsPath gp = new System.Drawing.Drawing2D.GraphicsPath();
            gp.AddEllipse(0, 0, profilePictureBox.Width, profilePictureBox.Height);
            Region rg = new Region(gp);
            profilePictureBox.Region = rg;
        }

        private void goToFacebookLinkButton_Click(object sender, EventArgs e)
        {
            try
            {
                this.m_FacebookDesktopLogic.Calendar.GoToFacebookLink(upcomingEventsListBox.SelectedIndex);
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

        private void checkBoxFriends_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxFriends.Checked)
            {
                this.m_FacebookDesktopLogic.Cupid.CheckFriends = true;
            }
            else
            {
                this.m_FacebookDesktopLogic.Cupid.CheckFriends = false;
            }
        }

        private void checkBoxEvents_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxEvents.Checked)
            {
                this.m_FacebookDesktopLogic.Cupid.CheckEvents = true;
            }
            else
            {
                this.m_FacebookDesktopLogic.Cupid.CheckEvents = false;
            }
        }

        private void checkBoxGroups_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxGroups.Checked)
            {
                this.m_FacebookDesktopLogic.Cupid.CheckGroups = true;
            }
            else
            {
                this.m_FacebookDesktopLogic.Cupid.CheckGroups = false;
            }
        }

        private void checkBoxCheckIns_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxCheckIns.Checked)
            {
                this.m_FacebookDesktopLogic.Cupid.CheckCheckIns = true;
            }
            else
            {
                this.m_FacebookDesktopLogic.Cupid.CheckCheckIns = false;
            }
        }

        private void checkBoxLikedPages_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxLikedPages.Checked)
            {
                this.m_FacebookDesktopLogic.Cupid.CheckLikedPages = true;
            }
            else
            {
                this.m_FacebookDesktopLogic.Cupid.CheckLikedPages = false;
            }
        }

        private void checkBoxHomeTown_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxHomeTown.Checked)
            {
                this.m_FacebookDesktopLogic.Cupid.CheckHomeTown = true;
            }
            else
            {
                this.m_FacebookDesktopLogic.Cupid.CheckHomeTown = false;
            }
        }

        private void checkBoxFieldOfStudy_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxFieldOfStudy.Checked)
            {
                this.m_FacebookDesktopLogic.Cupid.CheckFieldOfStudy = true;
            }
            else
            {
                this.m_FacebookDesktopLogic.Cupid.CheckFieldOfStudy = false;
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (this.checkBoxRememberMe.Checked && this.LogOutLabel.Enabled == true)
            {
                this.updateAppSettings();
            }
            else
            {
                if (this.m_FacebookDesktopLogic.LoggedInUser != null) 
                {
                    this.m_FacebookDesktopLogic.AppSettings.DeleteFile();
                }
            }
        }

        private void updateAppSettings()
        {
            this.m_FacebookDesktopLogic.AppSettings.LastWindowLocation = this.Location;
            this.m_FacebookDesktopLogic.AppSettings.RememberUser = this.checkBoxRememberMe.Checked;

            if (this.m_FacebookDesktopLogic.LoggedInUser != null)
            {          
                if(this.m_FacebookDesktopLogic.AppSettings.RememberUser)
                {
                    this.m_FacebookDesktopLogic.AppSettings.LastAccessToken = this.m_FacebookDesktopLogic.LoginResult.AccessToken;
                }
                else
                {
                    this.m_FacebookDesktopLogic.AppSettings.LastAccessToken = null;
                }
            }

            this.m_FacebookDesktopLogic.AppSettings.SaveToFile();
        }
    }
}
