using System;
using System.Collections.Generic;
using System.Text;
using FacebookWrapper;
using FacebookWrapper.ObjectModel;

namespace B19_EX01_Ben_305401317_Dana_311358543
{
    public class FacebookDesktopLogic
    {
        private bool m_IsLogIn = false;

        private static FacebookDesktopLogic s_FacebookDesktopLogic = null;
        private User m_LoggedInUser;

        public User LoggedInUser
        {
            get { return m_LoggedInUser; }
        }

        private FacebookDesktopLogic() { }
        public static FacebookDesktopLogic GetFacebookDesktopLogic()
        {
            //todo: lock
            if (s_FacebookDesktopLogic == null)
            {
                s_FacebookDesktopLogic = new FacebookDesktopLogic();
            }

            return s_FacebookDesktopLogic;
        }

        public bool LoginAndInit()
        {
            /// Owner: design.patterns

            /// Use the FacebookService.Login method to display the login form to any user who wish to use this application.
            /// You can then save the result.AccessToken for future auto-connect to this user:
            LoginResult result = FacebookService.Login("1450160541956417"/*"352758402005372"*/, /// (desig patter's "Design Patterns Course App 2.4" app)

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
                this.m_IsLogIn = true;
                //fetchUserInfo();
            }
            else
            {
                this.m_IsLogIn = false;
                //MessageBox.Show(result.ErrorMessage);
            }

            return m_IsLogIn;
        }

        public List<string> GetLatestPhotosInAlbum(int i_AlbumNumber, int i_NumOfItems)
        {
            List<string> Photos = new List<string>();

            int numOfPhotos = 0;
            Album album = m_LoggedInUser.Albums[i_AlbumNumber];

            for (int i = 0; i < album.Photos.Count; i++)
            {
                Photos.Add(album.Photos[i].PictureNormalURL);
                numOfPhotos++;
                if (numOfPhotos > i_NumOfItems)
                {
                    break;
                }
            }


            return Photos;
        }
    }
}
