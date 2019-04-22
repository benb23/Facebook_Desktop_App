using System;
using System.Collections.Generic;
using System.Windows.Forms;
using FacebookWrapper.ObjectModel;

namespace FacebookDesktopLogic
{
    public class FacebookCalendar
    {
        private List<User> m_UpcomingBirthdaysUsers = new List<User>();

        public List<User> UpcomingBirthdaysUsers
        {
            get { return this.m_UpcomingBirthdaysUsers; }
        }

        private List<Event> m_UpcomingEvents = new List<Event>();

        public User LoggedInUser { get; set; }

        public void WishHappyBirthday(int i_Index)
        {
            this.LoggedInUser.PostStatus("Happy Birthday" + this.m_UpcomingBirthdaysUsers[i_Index].Name);
        }

        public void GoToFacebookLink(int i_Index)
        {
            if (this.m_UpcomingEvents.Count != 0 && this.m_UpcomingEvents[i_Index] != null)
            {
                string link = this.m_UpcomingEvents[i_Index].LinkToFacebook;
                System.Diagnostics.Process.Start(link);
            }
            else
            {
                MessageBox.Show("No event selected! ", " Problem", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
