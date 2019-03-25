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
        private List<User> m_UpcomingBirthdaysUsers = new List<User>();
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

        //public static Calendar GetCalendar()
        //{
        //    //todo: lock
        //    if (s_Calendar == null)
        //    {
        //        s_Calendar = new Calendar();
        //    }

        //    return s_Calendar;
        //}

        public void wishHappyBirthday(int i_Index)
        {
            LoggedInUser.PostStatus("Happy Birthday" + m_UpcomingBirthdaysUsers[i_Index].Name);
        }

        public void initUpcomingBirthdaysUsersList()
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
            }
            catch
            {
                MessageBox.Show("There was a problem loading friends birthdays from Facebook ", " Problem", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

    }
}
