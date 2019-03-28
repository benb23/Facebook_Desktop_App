using System;
using System.Collections.Generic;
using System.Windows.Forms;
using FacebookWrapper.ObjectModel;

namespace FacebookAppLogic
{
    public class FacebookCalendar
    {
        private bool m_IsEventsLoaded = false;
        private bool m_IsBirthdaysLoaded = false;
        private List<User> m_UpcomingBirthdaysUsers = new List<User>();

        public List<User> UpcomingBirthdaysUsers
        {
            get { return this.m_UpcomingBirthdaysUsers; }
        }

        private List<Event> m_UpcomingEvents = new List<Event>();

        public List<Event> UpcomingEvents
        {
            get { return this.m_UpcomingEvents; }
        }

        public FacebookObjectCollection<User> FriendsList { get; set; }

        public User LoggedInUser { get; set; }

        public void FetchEvents()
        {
            if (!this.m_IsEventsLoaded)
            {
                    foreach (Event eventItem in LoggedInUser.Events)
                    {
                        if (eventItem.StartTime.Value.Month == DateTime.Now.Month)
                        {
                            this.m_UpcomingEvents.Add(eventItem);
                        }
                    }

                    this.m_IsEventsLoaded = true;
             }
        }

        public void WishHappyBirthday(int i_Index)
        {
            LoggedInUser.PostStatus("Happy Birthday" + this.m_UpcomingBirthdaysUsers[i_Index].Name);
        }

        public void GoToFacebookLink(int i_Index)
        {
            if (this.m_UpcomingEvents.Count!=0 && this.m_UpcomingEvents[i_Index] != null)
            {
                string link = this.m_UpcomingEvents[i_Index].LinkToFacebook;
                System.Diagnostics.Process.Start(link);
            }
            else
            {
                MessageBox.Show("No event selected! ", " Problem", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void FetchBirthdays()
        {
            if (!this.m_IsBirthdaysLoaded)
            {
                foreach (User friend in FriendsList)
                {
                    if (DateTime.Parse(friend.Birthday).Month == DateTime.Now.Month)
                    {
                        this.m_UpcomingBirthdaysUsers.Add(friend);
                    }
                }

                this.m_IsBirthdaysLoaded = true;
            }
        }

    }
}
