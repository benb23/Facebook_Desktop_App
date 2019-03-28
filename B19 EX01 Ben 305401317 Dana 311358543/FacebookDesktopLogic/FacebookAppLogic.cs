using System.Collections.Generic;
using System.Windows.Forms;
using FacebookWrapper;
using FacebookWrapper.ObjectModel;

namespace FacebookAppLogic
{
    public class FacebookDesktopLogic
    {//352758402005372
        private readonly string r_AppID = "1450160541956417";
        public AppSettings AppSettings { get; set; }
        public LoginResult LoginResult { get; set; }

        private static FacebookDesktopLogic s_FacebookDesktopLogic = null;
        public List<string> LatestPhotos { get; set; }

        private FacebookCalendar m_Calendar = new FacebookCalendar();

        public FacebookCalendar Calendar
        {
            get { return m_Calendar; }
        }

        private FacebookCupid m_Cupid = new FacebookCupid();

        public FacebookCupid Cupid
        {
            get { return m_Cupid; }
        }

        private bool m_IsLogIn = false;
        private bool m_IsPostsLoaded = false;
        public string PictureNormalURL { get; set; }

        private List<Post> m_RecentPosts = new List<Post>();

        public List<Post> RecentPosts
        {
            get { return m_RecentPosts; }
        }

        public bool IsfriendListLoaded { get; set; }

        private FacebookObjectCollection<User> m_FriendsList = new FacebookObjectCollection<User>();
        
        public FacebookObjectCollection<User> FriendsList
        {
            set { this.m_FriendsList = value; }
            get { return this.m_FriendsList; }
        }

        private User m_LoggedInUser;

        public User LoggedInUser
        {
            get { return this.m_LoggedInUser; }
            set
            {
                this.m_LoggedInUser = value;
                Cupid.LoggedInUser = value;
                Calendar.LoggedInUser = value;
            }
        }

        public void FetchFriends()
        {
            if (!this.IsfriendListLoaded)
            {
                foreach (User friend in this.LoggedInUser.Friends)
                {
                    this.FriendsList.Add(friend);
                    friend.ReFetch(DynamicWrapper.eLoadOptions.Full);
                }

                if (this.LoggedInUser.Friends.Count == 0)
                {
                    MessageBox.Show("No Friends to retrieve :(");
                }

                this.IsfriendListLoaded = true;
            }
        }

        private FacebookDesktopLogic() { }

        public static FacebookDesktopLogic Instance
        {
            get
            {
                if (s_FacebookDesktopLogic == null)
                {
                    s_FacebookDesktopLogic = new FacebookDesktopLogic();
                }

                return s_FacebookDesktopLogic;
            }
        }

        public bool LoginAndInit()
        {
            /// Owner: design.patterns

            /// Use the FacebookService.Login method to display the login form to any user who wish to use this application.
            /// You can then save the result.AccessToken for future auto-connect to this user:
            LoginResult = FacebookService.Login(this.r_AppID/*"1450160541956417"/*"352758402005372"*/, /// (desig patter's "Design Patterns Course App 2.4" app)

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


            if (!string.IsNullOrEmpty(LoginResult.AccessToken))
            {
                LoggedInUser = LoginResult.LoggedInUser;
                this.m_IsLogIn = true;
            }
            else
            {
                this.m_IsLogIn = false;
            }

            return this.m_IsLogIn;
        }

        public void FetchRecentPosts(int i_NumOfPosts)
        {
            if (!this.m_IsPostsLoaded)
            {
                for (int i = 0; i < i_NumOfPosts; i++)
                {
                    this.m_RecentPosts.Add(LoggedInUser.Posts[i]);
                }

                this.m_IsPostsLoaded = true;
            }
        }

        private List<string> fetchLatestPhotosInAlbum(int i_AlbumNumber, int i_NumOfItems)
        {
            List<string> Photos = new List<string>();

            int numOfPhotos = 0;
            Album album = this.m_LoggedInUser.Albums[i_AlbumNumber];

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

        public int FetchLatestPhotos(int i_AlbumIndex, int i_NumOfPhotos)
        {
            LatestPhotos = fetchLatestPhotosInAlbum(i_AlbumIndex, i_NumOfPhotos);

            while (LatestPhotos.Count == 0)
            {
                i_AlbumIndex++;
                LatestPhotos = fetchLatestPhotosInAlbum(i_AlbumIndex, i_NumOfPhotos);
            }

            return i_AlbumIndex;
        }
    }
}
