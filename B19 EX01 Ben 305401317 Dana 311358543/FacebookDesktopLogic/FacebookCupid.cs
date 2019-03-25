using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FacebookWrapper;
using FacebookWrapper.ObjectModel;

namespace FacebookAppLogic
{
    public class FacebookCupid
    {
        public User LoggedInUser { get; set; }
        private List<Candidate> m_CupidResult = new List<Candidate>(3);
        private static FacebookCupid s_FacebookCupid = null;
        //private FacebookObjectCollection<User> m_FriendsList = new FacebookObjectCollection<User>();
        private List<Candidate> m_Candidates = new List<Candidate>();
        private Dictionary<string, int> m_Score = new Dictionary<string, int>();
        public bool CheckFriends { get; set; }
        public bool CheckEvents { get; set; }
        public bool CheckGroups { get; set; }
        public bool CheckCheckIns { get; set; }
        public bool CheckLikedPages { get; set; }
        public bool CheckHomeTown { get; set; }
        public bool CheckFieldOfStudy { get; set; }

        private FacebookCupid() { }

        public List<Candidate> Candidates
        {
            get { return m_Candidates; }
        }

        private void initScoreValues()
        {
            //todo: from file?
            m_Score.Add("Friends", 1);
            m_Score.Add("Events", 1);
            m_Score.Add("Groups", 1);
            m_Score.Add("CheckIns", 1);
            m_Score.Add("LikedPages", 1);
            m_Score.Add("HomeTown", 1);
            m_Score.Add("FieldOfStudy", 1);
        }

        public void filterRelevantCandidatesByGender(User.eGender i_Gender)
        {
            try
            {
                foreach (User friend in LoggedInUser.Friends)
                {
                    //if (friend.Gender != null && friend.Gender == i_Gender)
                    {
                        m_Candidates.Add(new Candidate() { User = friend, Score = 0 });
                    }
                }

                if(m_Candidates.Count == 0)
                {
                    MessageBox.Show("There was a problem filtering by gender", "Gender Problem", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch
            {
                MessageBox.Show("There was a problem loading the Gender", "Gender Problem", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void scoreCandidates()
        {
            initScoreValues();//todo: init here?

            foreach(Candidate candidate in m_Candidates)
            {
                if(CheckFriends)
                {
                    scoreCandidateAccordingToMutualFriends(candidate);
                }

                if(CheckGroups)
                {
                    scoreCandidateAccordingToMutualGroups(candidate);
                }

                if(CheckFieldOfStudy)
                {
                    scoreCandidateAccordingToFieldOfStudy(candidate);
                }

                if(CheckHomeTown)
                {
                    scoreCandidateAccordingToHomeTown(candidate);
                }

                if(CheckLikedPages)
                {
                    scoreCandidateAccordingToMutualLikedPages(candidate);
                }

                if(CheckCheckIns)
                {
                    scoreCandidateAccordingToMutualCheckIns(candidate);
                }

                if(CheckEvents)
                {
                    scoreCandidateAccordingToMutualEvents(candidate);
                }
            }
        }

        public void scoreCandidateAccordingToMutualFriends(Candidate i_Candidate)
        {
            try
            {
                FacebookObjectCollection<User> candidateFriends = i_Candidate.User.Friends;

                if (candidateFriends != null)
                {
                    foreach (User friend in candidateFriends)
                    {
                        if (LoggedInUser.Friends.Contains(friend))
                        {
                            i_Candidate.Score += m_Score["Friends"];
                        }
                    }
                }
            }
            catch
            {
                MessageBox.Show("There was a problem loading the Friends", "Friends Problem", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void scoreCandidateAccordingToMutualEvents(Candidate i_Candidate)
        {
            try
            {
                FacebookObjectCollection<Event> candidateEvents = i_Candidate.User.Events;

                if (candidateEvents != null)
                {
                    foreach (Event candidateEvent in candidateEvents)
                    {
                        if (LoggedInUser.Events.Contains(candidateEvent))
                        {
                            i_Candidate.Score += m_Score["Events"];
                        }
                    }
                }
            }
            catch
            {
                MessageBox.Show("There was a problem loading the Events", "Events Problem", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
}

        public void scoreCandidateAccordingToMutualGroups(Candidate i_Candidate)
        {
            try
            {
                FacebookObjectCollection<Group> candidateGroups = i_Candidate.User.Groups;

                if (candidateGroups != null)
                {
                    foreach (Group group in candidateGroups)
                    {
                        if (LoggedInUser.Groups.Contains(group))
                        {
                            i_Candidate.Score += m_Score["Groups"];
                        }
                    }
                }
            }
            catch
            {
                MessageBox.Show("There was a problem loading the Groups", "Groups Problem", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void scoreCandidateAccordingToMutualCheckIns(Candidate i_Candidate)
        {
            try
            {
                FacebookObjectCollection<Checkin> candidateCheckins = i_Candidate.User.Checkins;

                if (candidateCheckins != null)
                {
                    foreach (Checkin checkIn in candidateCheckins)
                    {
                        if (LoggedInUser.Checkins.Contains(checkIn))
                        {
                            i_Candidate.Score += m_Score["CheckIns"];
                        }
                    }
                }
            }
            catch
            {
                MessageBox.Show("There was a problem loading the CheckIns", "CheckIns Problem", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void scoreCandidateAccordingToMutualLikedPages(Candidate i_Candidate)
        {
            try
            {
                FacebookObjectCollection<Page> candidateLikedPages = i_Candidate.User.LikedPages;

                if (candidateLikedPages != null)
                {
                    foreach (Page page in candidateLikedPages)
                    {
                        if (LoggedInUser.LikedPages.Contains(page))
                        {
                            i_Candidate.Score += m_Score["LikedPages"];
                        }
                    }
                }
            }
            catch
            {
                MessageBox.Show("There was a problem loading the pages", "Pages Problem", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void scoreCandidateAccordingToHomeTown(Candidate i_Candidate)
        {
            City candidateCity = i_Candidate.User.Hometown;

            if (candidateCity != null && candidateCity == LoggedInUser.Hometown)
            {
                i_Candidate.Score += m_Score["HomeTown"];
            }
        }

        public void scoreCandidateAccordingToFieldOfStudy(Candidate i_Candidate)
        {
            Education[] candidateEducation = i_Candidate.User.Educations;

            if (candidateEducation != null)
            {
                foreach (Education education in candidateEducation)
                {
                    if (LoggedInUser.Educations.Contains(candidateEducation[0]))
                    {
                        i_Candidate.Score += m_Score["HomeTown"];
                    }
                }
            }
        }

        public List<Candidate> FindMyMatch(User.eGender i_checkedGender)
        {
            filterRelevantCandidatesByGender(i_checkedGender);
            scoreCandidates();
            //sort
            List<Candidate> sortedCandidates = m_Candidates.OrderBy(p => p.Score).ToList();

            m_CupidResult.Add(sortedCandidates.Last());
            sortedCandidates.RemoveAt(sortedCandidates.Count - 1);
            m_CupidResult.Add(sortedCandidates.Last());
            sortedCandidates.RemoveAt(sortedCandidates.Count - 1);
            m_CupidResult.Add(sortedCandidates.Last());

            return m_CupidResult;
        }


        public static FacebookCupid GetFacebookCupid()
        {
            //todo: lock
            if (s_FacebookCupid == null)
            {
                s_FacebookCupid = new FacebookCupid();
            }

            return s_FacebookCupid;
        }
    }
}
