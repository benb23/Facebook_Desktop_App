using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FacebookWrapper;
using FacebookWrapper.ObjectModel;

namespace FacebookAppLogic
{
    public class Calendar
    {
        private bool m_IsEventsLoaded = false;
        private bool m_IsBirthdaysLoaded = false;
        private List<User> m_UpcomingBirthdaysUsers = new List<User>();

        public List<User> UpcomingBirthdaysUsers
        {
            get { return m_UpcomingBirthdaysUsers; }
        }

        private List<Event> m_UpcomingEvents = new List<Event>();

        public List<Event> UpcomingEvents
        {
            get { return m_UpcomingEvents; }
        }

        public FacebookObjectCollection<User> FriendsList { get; set; }

        public User LoggedInUser { get; set; }

        private static Calendar s_Calendar = null;

        private Calendar() { }

        public static Calendar instance
        {
            get
            {
                if (s_Calendar == null)
                {
                    s_Calendar = new Calendar();
                }

                return s_Calendar;
            }
        }

        public void fetchEvents()
        {
            if (!m_IsEventsLoaded)
            {
                try
                {
                    foreach (Event eventItem in LoggedInUser.Events)
                    {
                        if (eventItem.StartTime.Value.Month == DateTime.Now.Month)
                        {
                            m_UpcomingEvents.Add(eventItem);
                        }
                    }

                    //if (m_UpcomingEvents.Count == 0)
                    //{
                    //    MessageBox.Show("No Events to retrieve :(");
                    //}

                    m_IsEventsLoaded = true;
                }
                catch
                {
                    MessageBox.Show("There was a problem loading Events from Facebook ", " Problem", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        public void wishHappyBirthday(int i_Index)
        {
            LoggedInUser.PostStatus("Happy Birthday" + m_UpcomingBirthdaysUsers[i_Index].Name);
        }

        public void goToFacebookLink(int i_Index)
        {
            try
            {
                if (m_UpcomingEvents.Count!=0 && m_UpcomingEvents[i_Index] != null)
                {
                    string link = m_UpcomingEvents[i_Index].LinkToFacebook;
                    System.Diagnostics.Process.Start(link);
                }
                else
                {
                    MessageBox.Show("No event selected! ", " Problem", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch
            {
                MessageBox.Show("There was a problem loading link to Facebook event ", " Problem", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void fetchBirthdays()
        {
            if (!m_IsBirthdaysLoaded)
            {
                try
                {
                    foreach (User friend in FriendsList)
                    {
                        if (DateTime.Parse(friend.Birthday).Month == DateTime.Now.Month)
                        {
                            m_UpcomingBirthdaysUsers.Add(friend);
                        }
                    }

                    m_IsBirthdaysLoaded = true;
                }
                catch
                {
                    MessageBox.Show("There was a problem loading friends birthdays from Facebook ", " Problem", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

    }
}
