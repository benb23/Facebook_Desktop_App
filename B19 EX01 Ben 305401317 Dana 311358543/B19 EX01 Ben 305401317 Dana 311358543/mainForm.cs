﻿using System;
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

        private bool m_IsPostsListBoxLoaded = false;

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
            tabControl.SelectedTab = this.tabPageloading;

            bool isLogIn = this.m_FacebookDesktopLogic.LoginAndInit();
            if(isLogIn)
            {
                this.fetchBasicUserInfo();
                this.m_FacebookDesktopLogic.FetchFriends();
                if (!this.m_IsPostsListBoxLoaded)
                {
                    this.loadHomeTab();
                    this.m_IsPostsListBoxLoaded = true;
                }

                tabControl.SelectedTab = this.tabPageHome;
            }
            else
            {
                MessageBox.Show("Error!"); 
                tabControl.SelectedTab = this.tabPageLogIn;
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

        private void initPostsPanel()
        {
            int postIndex = 0;
            string profilePictureUrl = this.m_FacebookDesktopLogic.LoggedInUser.PictureNormalURL;
            string name = this.m_FacebookDesktopLogic.LoggedInUser.Name;
            Post post;

            foreach (Panel posItem in postsPanel.Controls)
            {
                post = this.m_FacebookDesktopLogic.RecentPosts[postIndex];
                (posItem.Controls[(int)ePostItem.NumOfLikes] as Label).Text = post.LikedBy.Count.ToString();
                (posItem.Controls[(int)ePostItem.Message] as Label).Text = post.Message;
                (posItem.Controls[(int)ePostItem.Picture] as PictureBox).LoadAsync(post.From.PictureNormalURL);
                (posItem.Controls[(int)ePostItem.Name] as Label).Text = name;
                (posItem.Controls[(int)ePostItem.CreatedTime] as Label).Text = post.CreatedTime.Value.ToLongDateString();

                postIndex++;
            }
        }

        private void createPostButton_Click(object sender, EventArgs e)
        {
            this.m_FacebookDesktopLogic.LoggedInUser.PostStatus(textBox1.Text);
        }

        private void loadHomeTab()
        {
            try
            {
                this.m_FacebookDesktopLogic.FetchRecentPosts(postsPanel.Controls.Count);
                this.initFriendsListBox();
            }
            catch
            {
                MessageBox.Show("There was a problem loading posts from Facebook.", "Problem", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            this.initPostsPanel();
        }

        private void updateLatestPhotos()
        {
            try
            {
                int AlbumIndex = 0;

                foreach (Panel panel in tabPageAlbums.Controls)
                {
                    AlbumIndex = this.m_FacebookDesktopLogic.FetchLatestPhotos(AlbumIndex, panel.Controls.Count);

                    (panel.Controls[(int)eAlbumItem.AlbumNameIndex] as Label).Text = this.m_FacebookDesktopLogic.LoggedInUser.Albums[AlbumIndex].Name;
                    int currentItem = (int)eAlbumItem.FirstPhotoIndex;

                    foreach (string photo in this.m_FacebookDesktopLogic.LatestPhotos)
                    {
                        if (currentItem >= panel.Controls.Count)
                        {
                            break;
                        }

                        (panel.Controls[currentItem] as PictureBox).LoadAsync(photo);
                        currentItem++;
                    }

                    AlbumIndex++;
                }
            }
            catch
            {
                MessageBox.Show("There was a problem loading the photos.", "Photos Problem", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void homePictureBox_Click(object sender, EventArgs e)
        {
            if (this.m_FacebookDesktopLogic.LoggedInUser != null)
            {
                tabControl.SelectedTab = this.tabPageloading;
                this.loadHomeTab(); 
                tabControl.SelectedTab = this.tabPageHome;
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
                tabControl.SelectedTab = this.tabPageloading;
                this.updateLatestPhotos();
                tabControl.SelectedTab = this.tabPageAlbums;
            }
            else
            {
                MessageBox.Show("Please login !", "Login", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void initFriendsListBox()
        {
            listBoxFriends.Items.Clear();
            listBoxFriends.DisplayMember = "Name";
            foreach (User friend in this.m_FacebookDesktopLogic.FriendsList)
            {
                listBoxFriends.Items.Add(friend);
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
            tabControl.SelectedTab = this.tabPageloading;

            if (this.m_FacebookDesktopLogic.LoggedInUser != null)
            {
                this.m_FacebookDesktopLogic.Calendar.FriendsList = this.m_FacebookDesktopLogic.FriendsList;
                try
                {
                    this.m_FacebookDesktopLogic.Calendar.FetchBirthdays();
                    this.initUpcomingBirthdaysListBox();
                    this.m_FacebookDesktopLogic.Calendar.FetchEvents();
                }
                catch
                {
                    MessageBox.Show("There was a problem loading information Facebook ", " Problem", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                this.initUpcomingEventsListBox();
                tabControl.SelectedTab = this.tabPageCalendar;
            }
            else
            {
                MessageBox.Show("Please login !", "Login", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void initUpcomingBirthdaysListBox()
        {
            foreach (User user in this.m_FacebookDesktopLogic.Calendar.UpcomingBirthdaysUsers)
            {
                upcomingBirthdaysListBox.Items.Add(user.Name + " " + user.Birthday);
            }
        }

        private void initUpcomingEventsListBox()
        {
            foreach (Event eventItem in this.m_FacebookDesktopLogic.Calendar.UpcomingEvents)
            {
                upcomingEventsListBox.Items.Add(eventItem.Name + " " + eventItem.StartTime);
            }
        }

        private void pictureBoxFaceCupid_Click(object sender, EventArgs e)
        {
            tabControl.SelectedTab = this.tabPageloading;

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
