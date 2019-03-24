using System;
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
            foreach (User friend in LoggedInUser.Friends)
            {
                //if (friend.Gender == i_Gender)
                {
                    m_Candidates.Add( new Candidate(){User = friend, Score = 0 });
                }
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
            foreach(User friend in i_Candidate.User.Friends)
            {
                if(LoggedInUser.Friends.Contains(friend))
                {
                    i_Candidate.Score += m_Score["Friends"];
                }
            }
        }

        public void scoreCandidateAccordingToMutualEvents(Candidate i_Candidate)
        {
            foreach (Event candidateEvent in i_Candidate.User.Events)
            {
                if (LoggedInUser.Events.Contains(candidateEvent))
                {
                    i_Candidate.Score += m_Score["Events"];
                }
            }
        }

        public void scoreCandidateAccordingToMutualGroups(Candidate i_Candidate)
        {
            foreach (Group group in i_Candidate.User.Groups)
            {
                if (LoggedInUser.Groups.Contains(group))
                {
                    i_Candidate.Score += m_Score["Groups"];
                }
            }
        }

        public void scoreCandidateAccordingToMutualCheckIns(Candidate i_Candidate)
        {
            foreach (Checkin checkIn in i_Candidate.User.Checkins)
            {
                if (LoggedInUser.Checkins.Contains(checkIn))
                {
                    i_Candidate.Score += m_Score["CheckIns"];
                }
            }
        }

        public void scoreCandidateAccordingToMutualLikedPages(Candidate i_Candidate)
        {
            foreach (Page page in i_Candidate.User.LikedPages)
            {
                if (LoggedInUser.LikedPages.Contains(page))
                {
                    i_Candidate.Score += m_Score["LikedPages"];
                }
            }
        }

        public void scoreCandidateAccordingToHomeTown(Candidate i_Candidate)
        {
            if(i_Candidate.User.Hometown == LoggedInUser.Hometown)
            {
                i_Candidate.Score += m_Score["HomeTown"];
            }
        }

        public void scoreCandidateAccordingToFieldOfStudy(Candidate i_Candidate)
        {
            foreach (Education education in i_Candidate.User.Educations)
            {
                if (LoggedInUser.Educations.Contains(i_Candidate.User.Educations[0]))
                {
                    i_Candidate.Score += m_Score["HomeTown"];
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
            sortedCandidates.RemoveAt(sortedCandidates.Capacity - 1);
            m_CupidResult.Add(sortedCandidates.Last());
            sortedCandidates.RemoveAt(sortedCandidates.Capacity - 1);
            m_CupidResult.Add(sortedCandidates.Last());
            sortedCandidates.RemoveAt(sortedCandidates.Capacity - 1);
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
