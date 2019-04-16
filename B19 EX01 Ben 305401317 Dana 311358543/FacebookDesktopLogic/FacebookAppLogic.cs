using System.Collections.Generic;
using FacebookWrapper;
using FacebookWrapper.ObjectModel;

namespace FacebookDesktopLogic
{
    public sealed class FacebookAppLogic
    {
        private static FacebookAppLogic s_FacebookDesktopLogic = null;
        private static object s_LockObj = new object();

        private FacebookAppLogic()
        {
        }

        private readonly string r_AppID = "352758402005372";

        public static FacebookAppLogic Instance
        {
            get
            {
                if (s_FacebookDesktopLogic == null)
                {
                    lock (s_LockObj)
                    {
                        if (s_FacebookDesktopLogic == null)
                        {
                            s_FacebookDesktopLogic = new FacebookAppLogic();
                        }
                    }
                }
                return s_FacebookDesktopLogic;
            }
        }

        public AppSettings AppSettings { get; set; }

        public LoginResult LoginResult { get; set; }

        //public UserCachingProxy  LogInUser { get; set; }

        private volatile FacebookCalendar m_Calendar;// = new FacebookCalendar();

        private bool m_IsLogIn = false;

        //private bool m_IsPostsLoaded = false;

        public string PictureNormalURL { get; set; }

        private List<Post> m_RecentPosts = new List<Post>();

        public FacebookCalendar Calendar
        {
            get
            {
                if (m_Calendar == null)
                {
                    m_Calendar = new FacebookCalendar();
                }
                return this.m_Calendar;
            }
        }

        private volatile FacebookCupid m_Cupid;// = new FacebookCupid();

        public FacebookCupid Cupid
        {
            get
            {
                if (m_Cupid == null)
                {
                    m_Cupid = new FacebookCupid();
                }

                return this.m_Cupid;
            }
        }

        public List<Post> RecentPosts
        {
            get { return this.m_RecentPosts; }
        }

        //public bool IsfriendListLoaded { get; set; }

        private FacebookObjectCollection<User> m_FriendsList = new FacebookObjectCollection<User>();

        public FacebookObjectCollection<User> FriendsList
        {
            get { return this.m_FriendsList; }
            set { this.m_FriendsList = value; }
        }

        private User m_LoggedInUser;

        public User LoggedInUser
        {
            get { return this.m_LoggedInUser; }
            set
            {
                this.m_LoggedInUser = value;
                this.Cupid.LoggedInUser = value;
                this.Calendar.LoggedInUser = value;
            }
        }

        public bool LoginAndInit()
        {
            /// Owner: design.patterns

            /// Use the FacebookService.Login method to display the login form to any user who wish to use this application.
            /// You can then save the result.AccessToken for future auto-connect to this user:
            /// //todo: change
            ///          
            LoginResult = FacebookService.Login(/*this.r_AppID*/"1450160541956417", /// (desig patter's "Design Patterns Course App 2.4" app)

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
                this.LoggedInUser = LoginResult.LoggedInUser;
                this.m_IsLogIn = true;
            }
            else
            {
                this.m_IsLogIn = false;
            }

            return this.m_IsLogIn;
        }

        public List<Candidate> CupidResult
        {
            get
            {
                return this.Cupid.CupidResult;
            }
        }

        public Candidate ChosenMatch
        {
            set
            {
                this.Cupid.ChosenMatch = value;
            }
        }

        public bool CheckFriends
        {
            set
            {
                this.Cupid.CheckFriends = value;
            }
        }

        public bool CheckEvents
        {
            set
            {
                this.Cupid.CheckEvents = false;
            }

        }

        public bool CheckGroups
        {
            set
            {
                this.Cupid.CheckGroups = false;
            }
        }

        public bool CheckCheckIns
        {
            set
            {
                FacebookAppLogic.Instance.Cupid.CheckCheckIns = true;
            }
        }
        
        public bool CheckLikedPages
        {
            set
            {
                FacebookAppLogic.Instance.Cupid.CheckLikedPages = true;
            }
        }

        public bool CheckHomeTown
        {
            set
            {
                FacebookAppLogic.Instance.Cupid.CheckHomeTown = true;
            }
        }

        public bool CheckFieldOfStudy
        {
            set
            {
                FacebookAppLogic.Instance.Cupid.CheckFieldOfStudy = true;
            }
        }

        public void postOnMatchWall(string i_Msg)
        {
            this.Cupid.postOnMatchWall(i_Msg);
        }

        public List<Candidate> FindMyMatch(User.eGender? i_Gender)
        {
            return this.Cupid.FindMyMatch(i_Gender);
        }

        public void WishHappyBirthday(int i_Index)
        {
            FacebookAppLogic.Instance.Calendar.WishHappyBirthday(i_Index);
        }

        public void GoToFacebookLink(int i_Index)
        {
            FacebookAppLogic.Instance.Calendar.GoToFacebookLink(i_Index);

        }
        public List<User> UpcomingBirthdaysUsers
        {
            get
            {
                return this.Calendar.UpcomingBirthdaysUsers;
            }
        }
        public List<string> fetchLatestPhotosInAlbum(int i_AlbumNumber, int i_NumOfItems)
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
    }
}
