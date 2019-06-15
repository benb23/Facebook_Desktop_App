using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using FacebookWrapper.ObjectModel;

namespace FacebookDesktopLogic
{
    public class FacebookCupid
    {
        public Candidate ChosenMatch { get; set; }

        private User m_LoggedInUser;

        public User LoggedInUser
        {
            get { return m_LoggedInUser; }
            set
            {
                m_LoggedInUser = value;
                Sorter.LoggedInUser = value;
            }
        }

        private List<Candidate> m_CupidResult = new List<Candidate>(3);

        public List<Candidate> CupidResult
        {
            get { return this.m_CupidResult; }
            set { this.m_CupidResult = value; }
        }

        private List<Candidate> m_Candidates = new List<Candidate>();

        private IFriendsSorter m_Sorter = new FriendsSorterByMutualFields();
        public IFriendsSorter Sorter
        {
            get { return m_Sorter; }
            set
            {
                m_Sorter = value;
                m_Sorter.LoggedInUser = this.LoggedInUser;
            }
        }

        public FacebookObjectCollection<User> FriendsList { get; set; }


        public FacebookCupid()
        {
        }

        private void filterRelevantCandidatesByGender(User.eGender? i_Gender)
        {
            foreach (User friend in this.FriendsList)
            {
                if (i_Gender == null || friend.Gender == i_Gender)
                {
                    this.m_Candidates.Add(new Candidate() { User = friend, Score = 0 });
                }
            }

            if(this.m_Candidates.Count == 0)
            {
                throw new Exception();
            }
        }

        public void postOnMatchWall(string i_Msg)
        {
                this.ChosenMatch.User.PostStatus(i_Msg);
                MessageBox.Show("Post published successfully.", ":)", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public List<Candidate> FindMyMatch(User.eGender? i_Gender)
        {
            this.filterAndScoreCndidates(i_Gender);
            return m_CupidResult;
        }

        private void filterAndScoreCndidates(User.eGender? i_checkedGender)
        {
            this.filterRelevantCandidatesByGender(i_checkedGender);
            this.Sorter.SortFriends();
            this.setCupidResult();
        }

        private void setCupidResult()
        {
            if (this.m_Candidates.Count != 0)
            {
                this.CupidResult.Add(Sorter.Candidates.Last());
                Sorter.Candidates.RemoveAt(Sorter.Candidates.Count - 1);
                this.CupidResult.Add(Sorter.Candidates.Last());
                Sorter.Candidates.RemoveAt(Sorter.Candidates.Count - 1);
                this.CupidResult.Add(Sorter.Candidates.Last());
            }
        }
    }
}
