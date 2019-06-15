using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FacebookWrapper.ObjectModel;

namespace FacebookDesktopLogic
{
    public class FriendsSorterByMutualFields: IFriendsSorter
    {
        public List<Candidate> Candidates { get; set; }
        public User LoggedInUser { get; set; }

        Dictionary<string, int> ScoreValues = new Dictionary<string, int>(); 

        public FriendsSorterByMutualFields()
        {
            initScoreValues();
        }

        private void scoreCandidateByMutualFriends(Candidate i_Candidate)
        {
            FacebookObjectCollection<User> candidateFriends = i_Candidate.User.Friends;

            if (candidateFriends != null)
            {
                foreach (User friend in candidateFriends)
                {
                    if (this.LoggedInUser.Friends.Contains(friend))
                    {
                        i_Candidate.Score += this.ScoreValues["Friends"];
                    }
                }
            }
        }

        private void scoreCandidateByMutualEvents(Candidate i_Candidate)
        {
            FacebookObjectCollection<Event> candidateEvents = i_Candidate.User.Events;

            if (candidateEvents != null)
            {
                foreach (Event candidateEvent in candidateEvents)
                {
                    if (this.LoggedInUser.Events.Contains(candidateEvent))
                    {
                        i_Candidate.Score += this.ScoreValues["Events"];
                    }
                }
            }
        }

        private void scoreCandidateByMutualGroups(Candidate i_Candidate)
        {
            FacebookObjectCollection<Group> candidateGroups = i_Candidate.User.Groups;

            if (candidateGroups != null)
            {
                foreach (Group group in candidateGroups)
                {
                    if (this.LoggedInUser.Groups.Contains(group))
                    {
                        i_Candidate.Score += this.ScoreValues["Groups"];
                    }
                }
            }
        }

        private void scoreCandidateByMutualCheckIns(Candidate i_Candidate)
        {
            FacebookObjectCollection<Checkin> candidateCheckins = i_Candidate.User.Checkins;

            if (candidateCheckins != null)
            {
                foreach (Checkin checkIn in candidateCheckins)
                {
                    if (this.LoggedInUser.Checkins.Contains(checkIn))
                    {
                        i_Candidate.Score += this.ScoreValues["CheckIns"];
                    }
                }
            }
        }

        private void scoreCandidateByMutualLikedPages(Candidate i_Candidate)
        {
            FacebookObjectCollection<Page> candidateLikedPages = i_Candidate.User.LikedPages;

            if (candidateLikedPages != null)
            {
                foreach (Page page in candidateLikedPages)
                {
                    if (this.LoggedInUser.LikedPages.Contains(page))
                    {
                        i_Candidate.Score += this.ScoreValues["LikedPages"];
                    }
                }
            }
        }

        private void scoreCandidateByHomeTown(Candidate i_Candidate)
        {
            City candidateCity = i_Candidate.User.Hometown;
            if (candidateCity != null)
            {
                if (candidateCity == this.LoggedInUser.Hometown)
                {
                    i_Candidate.Score += this.ScoreValues["HomeTown"];
                }
            }
            else
            {
                throw new Exception();
            }
        }

        private void scoreCandidateByFieldOfStudy(Candidate i_Candidate)
        {
            Education[] candidateEducation = i_Candidate.User.Educations;

            if (candidateEducation != null)
            {
                foreach (Education education in candidateEducation)
                {
                    if (this.LoggedInUser.Educations.Contains(candidateEducation[0]))
                    {
                        i_Candidate.Score += this.ScoreValues["HomeTown"];
                    }
                }
            }
        }

        public void SortFriends()
        {
            foreach (Candidate candidate in this.Candidates)
            {
                this.scoreCandidateByMutualFriends(candidate);

                this.scoreCandidateByMutualGroups(candidate);

                this.scoreCandidateByFieldOfStudy(candidate);

                this.scoreCandidateByHomeTown(candidate);

                this.scoreCandidateByMutualLikedPages(candidate);

                this.scoreCandidateByMutualCheckIns(candidate);

                this.scoreCandidateByMutualEvents(candidate);
            }
        
            Candidates = this.Candidates.OrderBy(p => p.Score).ToList();
        }

        private void initScoreValues()
        {
            this.ScoreValues.Add("Friends", 10);
            this.ScoreValues.Add("Events", 4);
            this.ScoreValues.Add("Groups", 3);
            this.ScoreValues.Add("CheckIns", 1);
            this.ScoreValues.Add("LikedPages", 6);
            this.ScoreValues.Add("HomeTown", 5);
            this.ScoreValues.Add("FieldOfStudy", 8);
        }
    }
}
