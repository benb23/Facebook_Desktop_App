using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using FacebookWrapper.ObjectModel;

namespace FacebookAppLogic
{
    public class FacebookCupid
    {
        public Candidate ChosenMatch { get; set; }
        public User LoggedInUser { get; set; }

        private List<Candidate> m_CupidResult = new List<Candidate>(3);

        public List<Candidate> CupidResult
        {
            get { return this.m_CupidResult; }
            set { this.m_CupidResult = value; }
        }

        private List<Candidate> m_Candidates = new List<Candidate>();
        private Dictionary<string, int> m_Score = new Dictionary<string, int>();
        public FacebookObjectCollection<User> FriendsList { get; set; }
        public bool CheckFriends { get; set; }
        public bool CheckEvents { get; set; }
        public bool CheckGroups { get; set; }
        public bool CheckCheckIns { get; set; }
        public bool CheckLikedPages { get; set; }
        public bool CheckHomeTown { get; set; }
        public bool CheckFieldOfStudy { get; set; }

        public FacebookCupid()
        {
            initScoreValues();
        }

        private void initScoreValues()
        {
            this.m_Score.Add("Friends", 10);
            this.m_Score.Add("Events", 4);
            this.m_Score.Add("Groups", 3);
            this.m_Score.Add("CheckIns", 1);
            this.m_Score.Add("LikedPages", 6);
            this.m_Score.Add("HomeTown", 5);
            this.m_Score.Add("FieldOfStudy", 8);
        }

        public void filterRelevantCandidatesByGender(User.eGender? i_Gender)
        {
            foreach (User friend in FriendsList)
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

        public void scoreCandidates()
        {
            foreach (Candidate candidate in this.m_Candidates)
            {
                if (CheckFriends)
                {
                    scoreCandidateAccordingToMutualFriends(candidate);
                }

                if (CheckGroups)
                {
                    scoreCandidateAccordingToMutualGroups(candidate);
                }

                if (CheckFieldOfStudy)
                {
                    scoreCandidateAccordingToFieldOfStudy(candidate);
                }

                if (CheckHomeTown)
                {
                    scoreCandidateAccordingToHomeTown(candidate);
                }

                if (CheckLikedPages)
                {
                    scoreCandidateAccordingToMutualLikedPages(candidate);
                }

                if (CheckCheckIns)
                {
                    scoreCandidateAccordingToMutualCheckIns(candidate);
                }

                if (CheckEvents)
                {
                    scoreCandidateAccordingToMutualEvents(candidate);
                }
            }
        }

        public void scoreCandidateAccordingToMutualFriends(Candidate i_Candidate)
        {
            FacebookObjectCollection<User> candidateFriends = i_Candidate.User.Friends;

            if (candidateFriends != null)
            {
                foreach (User friend in candidateFriends)
                {
                    if (LoggedInUser.Friends.Contains(friend))
                    {
                        i_Candidate.Score += this.m_Score["Friends"];
                    }
                }
            }
        }

        public void scoreCandidateAccordingToMutualEvents(Candidate i_Candidate)
        {

            FacebookObjectCollection<Event> candidateEvents = i_Candidate.User.Events;

            if (candidateEvents != null)
            {
                foreach (Event candidateEvent in candidateEvents)
                {
                    if (LoggedInUser.Events.Contains(candidateEvent))
                    {
                        i_Candidate.Score += this.m_Score["Events"];
                    }
                }
            }
}

        public void scoreCandidateAccordingToMutualGroups(Candidate i_Candidate)
        {
            FacebookObjectCollection<Group> candidateGroups = i_Candidate.User.Groups;

            if (candidateGroups != null)
            {
                foreach (Group group in candidateGroups)
                {
                    if (LoggedInUser.Groups.Contains(group))
                    {
                        i_Candidate.Score += this.m_Score["Groups"];
                    }
                }
            }
        }

        public void scoreCandidateAccordingToMutualCheckIns(Candidate i_Candidate)
        {
            FacebookObjectCollection<Checkin> candidateCheckins = i_Candidate.User.Checkins;

            if (candidateCheckins != null)
            {
                foreach (Checkin checkIn in candidateCheckins)
                {
                    if (LoggedInUser.Checkins.Contains(checkIn))
                    {
                        i_Candidate.Score += this.m_Score["CheckIns"];
                    }
                }
            }
        }

        public void scoreCandidateAccordingToMutualLikedPages(Candidate i_Candidate)
        {

            FacebookObjectCollection<Page> candidateLikedPages = i_Candidate.User.LikedPages;

            if (candidateLikedPages != null)
            {
                foreach (Page page in candidateLikedPages)
                {
                    if (LoggedInUser.LikedPages.Contains(page))
                    {
                        i_Candidate.Score += this.m_Score["LikedPages"];
                    }
                }
            }
        }

        public void scoreCandidateAccordingToHomeTown(Candidate i_Candidate)
        {

            City candidateCity = i_Candidate.User.Hometown;
            if (candidateCity != null)
            {
                if (candidateCity == LoggedInUser.Hometown)
                {
                    i_Candidate.Score += this.m_Score["HomeTown"];
                }
            }
            else
            {
                throw new Exception();
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
                        i_Candidate.Score += this.m_Score["HomeTown"];
                    }
                }
            }
        }

        public void postOnMatchWall(string i_Msg)
        {
                ChosenMatch.User.PostStatus(i_Msg);
                MessageBox.Show("Post published successfully.", ":)", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void FindMyMatch(User.eGender? i_Gender)
        {
            this.FriendsList = FacebookDesktopLogic.instance.FriendsList;          
            this.filterAndScoreCndidates(i_Gender);
        }

        private void filterAndScoreCndidates(User.eGender? i_checkedGender)
        {
            filterRelevantCandidatesByGender(i_checkedGender);
            scoreCandidates();
            setCupidResult();
        }

        private void setCupidResult()
        {
            if (this.m_Candidates.Count != 0)
            {
                List<Candidate> sortedCandidates = this.m_Candidates.OrderBy(p => p.Score).ToList();
                CupidResult.Add(sortedCandidates.Last());
                sortedCandidates.RemoveAt(sortedCandidates.Count - 1);
                CupidResult.Add(sortedCandidates.Last());
                sortedCandidates.RemoveAt(sortedCandidates.Count - 1);
                CupidResult.Add(sortedCandidates.Last());
            }
        }
    }
}
