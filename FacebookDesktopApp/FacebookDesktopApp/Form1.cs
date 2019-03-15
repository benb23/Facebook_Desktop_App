using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FacebookWrapper.ObjectModel;
using FacebookWrapper;


namespace FacebookDesktopApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        User m_LoggedInUser;

        private void loginAndInit()
        {
            /// Owner: design.patterns

            /// Use the FacebookService.Login method to display the login form to any user who wish to use this application.
            /// You can then save the result.AccessToken for future auto-connect to this user:
            LoginResult result = FacebookService.Login("1450160541956417", /// (desig patter's "Design Patterns Course App 2.4" app)

                "public_profile",
                "email",
                "publish_to_groups",
                "user_birthday",
                "user_age_range",
                "user_gender",
                "user_link",
                "user_tagged_places",
                "user_videos",
                "publish_to_groups",
                "groups_access_member_info",
                "user_friends",
                "user_events",
                "user_likes",
                "user_location",
                "user_photos",
                "user_posts",
                "user_hometown"

                /// DEPRECATED PERMISSIONS:
                ///"publish_actions"
                ///"user_about_me",
                ///"user_education_history",
                ///"user_actions.video",
                ///"user_actions.news",
                ///"user_actions.music",
                ///"user_actions.fitness",
                ///"user_actions.books",
                ///"user_games_activity",
                ///"user_managed_groups",
                ///"user_relationships",
                ///"user_relationship_details",
                ///"user_religion_politics",
                ///"user_tagged_places",
                ///"user_website",
                ///"user_work_history",
                ///"read_custom_friendlists",
                ///"read_page_mailboxes",
                ///"manage_pages",
                ///"publish_pages",
                ///"publish_actions",
                ///"rsvp_event"
                ///"user_groups" (This permission is only available for apps using Graph API version v2.3 or older.)
                ///"user_status" (This permission is only available for apps using Graph API version v2.3 or older.)
                /// "read_mailbox", (This permission is only available for apps using Graph API version v2.3 or older.)
                /// "read_stream", (This permission is only available for apps using Graph API version v2.3 or older.)
                /// "manage_notifications", (This permission is only available for apps using Graph API version v2.3 or older.)

                );
            /// The documentation regarding facebook login and permissions can be found here: 
            // https://developers.facebook.com/docs/facebook-login/permissions#reference


            if (!string.IsNullOrEmpty(result.AccessToken))
            {
                m_LoggedInUser = result.LoggedInUser;
                fetchUserInfo();
                
            }
            else
            {
                MessageBox.Show(result.ErrorMessage);
            }
        }

        private void fetchUserInfo()
        {
            //picture_smallPictureBox.LoadAsync(m_LoggedInUser.PictureNormalURL);
            //if (m_LoggedInUser.Posts.Count > 0)
            //{
            //    textBoxStatus.Text = m_LoggedInUser.Posts[0].Message;
            //}
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }
    }
}
