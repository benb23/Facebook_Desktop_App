using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FacebookWrapper.ObjectModel;

namespace FacebookDesktopLogic
{
    public interface IFriendsSorter
    {
        User LoggedInUser { get; set; } //todo: ??

        List<Candidate> Candidates { get; set; }//todo: ??

        void SortFriends();
    }
}
