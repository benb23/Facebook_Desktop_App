using System;
using System.Collections.Generic;
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

        }

        public void initUpcomingBirthdaysUsersList()
        {
            
        }

    }
}
