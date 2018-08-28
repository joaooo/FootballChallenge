using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FootballManager.Models
{
    [Serializable]
    public class FootballTeam
    {
        public FootballTeam(string teamName, string coachName)
        {
            this.TeamName = teamName;
            this.CoachName = coachName;
        }

        #region Variables
        public Guid Id;
        public string TeamName;
        public string CoachName;
        public int MatchesWon;
        public int MatchesLost;
        #endregion
    }
}