using FootballManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FootballManager.Controllers
{
    public class FootballController: Controller
    {
        #region GetFootballTeamById
        [HttpGet]
        public FootballTeam GetFootballTeamById(string Id)
        {
            return Business.FootballManager.Instance.GetFootballTeamById(Id);
        }
        #endregion

        #region CreateFootballTeam
        [HttpPost]
        public Result CreateFootballTeam(string teamName, string coachName)
        {
            Result res = new Result();
            try
            {
                Business.FootballManager.Instance.CreateTeam(teamName, coachName);
                res.Success = true;
            }

            catch (Exception ex)
            {
                res.ErrorMessage = ex.Message;
            }
            return res;
        }
        #endregion

        #region GetAllTeamNamesAndIds 
        [HttpGet]
        public List<Tuple<string, string>> GetAllTeamNamesAndIds()
        {
            return Business.FootballManager.Instance.GetAllTeamNamesAndIds();
        }
        #endregion

        #region GetFootballTeam
        [HttpGet]
        public FootballTeam GetFootballTeam(string teamName, string teamCoach)
        {
            return Business.FootballManager.Instance.GetFootballTeam(teamName, teamCoach);
        }
        #endregion

        #region DeleteTeam
        [HttpDelete]
        public void DeleteTeam(FootballTeam teamToDelete)
        {
            Result res = new Result();
            try
            {
                if (teamToDelete.Id != null)
                {
                    Business.FootballManager.Instance.DeleteTeam(teamToDelete.Id);
                }
                else
                {
                    Business.FootballManager.Instance.DeleteTeam(teamToDelete.TeamName, teamToDelete.CoachName);
                }
                
                res.Success = true;
            }
            
            catch (Exception ex)
            {
                res.ErrorMessage = ex.Message;
            }
        }
        #endregion

        #region UpdateTeamInfo
        [HttpPost]
        public void UpdateTeamInfo(FootballTeam updatedInfo)
        {
            Result res = new Result();
            try
            {
                Business.FootballManager.Instance.UpdateTeamInfo(updatedInfo);
                res.Success = true;
            }
            
            catch (Exception ex)
            {
                res.ErrorMessage = ex.Message;
            }
        }
        #endregion
    }
}