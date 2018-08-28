using FootballManager.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FootballManager.Business
{
    public class FootballManager
    {
        #region Variables
        private FootballManager() { }
        private static FootballManager instance;

        public static FootballManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new FootballManager();
                }
                return instance;
            }
        }
        #endregion

        #region Storage
        public ConcurrentDictionary<Guid, FootballTeam> TeamsByID { get; } = new ConcurrentDictionary<Guid, FootballTeam>();

        public ConcurrentDictionary<Tuple<string, string>, FootballTeam> TeamsNameAndCoach { get; } = new ConcurrentDictionary<Tuple<string, string>, FootballTeam>();
        #endregion

        #region CreateTeamOperation
        public void CreateTeam(string teamName, string coachName)
        {
            FootballTeam team = new FootballTeam(teamName, coachName);
            team.Id = Guid.NewGuid();

            if (!TeamsByID.TryAdd(team.Id, team))
            {
                throw new Exception("Error: The team was created but it could not be inserted into the Storage");
            }

            if (!TeamsNameAndCoach.TryAdd(new Tuple<string, string>(team.TeamName, team.CoachName), team))
            {
                TeamsByID.TryRemove(team.Id, out team);
                throw new Exception("Error: The team was created but it could not be inserted into the Storage");
            }
        }
        #endregion

        #region GetTeamOperations
        public FootballTeam GetFootballTeamById(string Id)
        {
            FootballTeam team = null;
            TeamsByID.TryGetValue(Guid.Parse(Id), out team);

            return team;
        }

        public FootballTeam GetFootballTeam(string teamName, string coachName)
        {
            FootballTeam team = null;

            //assuming its not possible to have teams with the same team name and the same coach name
            TeamsNameAndCoach.TryGetValue(new Tuple<string, string>(teamName, coachName), out team);

            return team;
        }

        public List<Tuple<string, string>> GetAllTeamNamesAndIds()
        {
            List<Tuple<string, string>> res = new List<Tuple<string, string>>();

            foreach (var team in TeamsByID)
            {
                res.Add(new Tuple<string, string>(team.Value.Id.ToString(), team.Value.TeamName));
            }
            return res;
        }
        #endregion

        #region UpdateTeamOperation
        public void UpdateTeamInfo(FootballTeam updatedInfo)
        {
            FootballTeam newTeam = GetFootballTeamById(updatedInfo.Id.ToString());
            FootballTeam oldTeam = newTeam;

            if (updatedInfo.TeamName != null)
            {
                newTeam.TeamName = updatedInfo.TeamName;
            }

            if (updatedInfo.CoachName != null)
            {
                newTeam.CoachName = updatedInfo.CoachName;
            }

            if (updatedInfo.MatchesWon != -1)
            {
                newTeam.MatchesWon = updatedInfo.MatchesWon;
            }

            if (updatedInfo.MatchesLost != -1)
            {
                newTeam.MatchesLost = updatedInfo.MatchesLost;
            }

            if (!TeamsByID.TryUpdate(updatedInfo.Id, newTeam, oldTeam))
            {
                throw new Exception("Error: Unable to update the team information");
            }
        }
        #endregion

        #region DeleteOperations
        public void DeleteTeam(Guid Id)
        {
            PerformDeleteOperation(GetFootballTeamById(Id.ToString()));
        }

        public void DeleteTeam(string teamName, string coachName)
        {
            PerformDeleteOperation(GetFootballTeam(teamName, coachName));
        }

        private void PerformDeleteOperation(FootballTeam toDelete)
        {
            FootballTeam team;
            if (!TeamsByID.TryRemove(toDelete.Id, out team))
            {
                throw new Exception("Error: Unable to delete team");
            }

            if (!TeamsNameAndCoach.TryRemove(new Tuple<string, string>(toDelete.TeamName, toDelete.CoachName), out team))
            {
                TeamsByID.TryAdd(toDelete.Id, toDelete);
                throw new Exception("Error: Unable to delete team");
            }
        }
        #endregion
    }
}