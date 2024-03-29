﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WorldCup2022_MVC.Interfaces;
using WorldCup2022_MVC.ViewModels;
using WorldCup2022_MVC.Session;
using Newtonsoft.Json;
namespace WorldCup2022_MVC.Controllers
{
    public class PlayGroupController : Controller
    {
        //private readonly IGroupStageService _groupStageservice;
        private readonly ITeamService _teamservice;
        private readonly IGroupStageService _groupstageservice;
        private readonly IMatchesService _matchesService;
        private readonly IPromotedTeamsService _promotedTeamsService;
        private List<MatchVM> matches = new List<MatchVM>();
        public PlayGroupController(ITeamService teamservice, IGroupStageService groupstageservice, IMatchesService matchesService, IPromotedTeamsService promotedTeamsService)
        {
            _teamservice = teamservice;
            _groupstageservice = groupstageservice;
            _matchesService = matchesService;
            _promotedTeamsService = promotedTeamsService;
        }

        [HttpGet]
        public ActionResult PlayGroup()
        {
            var id = generateID();
            bool iteration = true;
            while(iteration)
            {
                var data = _matchesService.GetAllMatches(id);
                var data2 = _promotedTeamsService.GetAllPromotedTeams(id);
                if (data == "null" && data2 == "null")
                {
                    iteration = false;
                }
                id = generateID();
            }
            MatchVM[] result = new MatchVM[49];
            var listOfPendingMatches = _groupstageservice.GetAllMatches();
            var teams = _teamservice.GetAllEntries();
            GroupStageVM[] arrayOfPendingMatches = new GroupStageVM[49];
            int indexToCopyListToArray = 0;
            foreach (var item in listOfPendingMatches)
            {
                arrayOfPendingMatches[indexToCopyListToArray] = item;
                indexToCopyListToArray++;
            }
            for (int i = 0; i < 48; i++)
            {
                result[i] = SimulateMatchForGroupPhase(arrayOfPendingMatches[i], teams);
            }
            var alldata = new TeamsMatchesVM
            {
                listOfTeams = teams,
                listOfMatches = listOfPendingMatches,
                arrayOfResult = result
            };
            var json = JsonConvert.SerializeObject(alldata);
            _matchesService.SaveAllMatches(id, json);
            ViewBag.id = id;
            ViewBag.alldata = alldata;
            return View(alldata);
        }
        [HttpGet]
        public ActionResult PlayGroupById(string? id)
        {
            string data = _matchesService.GetAllMatches(id);
            TeamsMatchesVM matches = JsonConvert.DeserializeObject<TeamsMatchesVM>(data);
            ViewBag.id = id;
            return View(matches);
        }
        private MatchVM SimulateMatchForGroupPhase(GroupStageVM groupStage, List<TeamVM> team)
        {
            TeamVM home_team = new TeamVM();
            TeamVM away_team = new TeamVM();
            foreach (var item in team)
            {
                if (item.placeInGroup == groupStage.home)
                {
                    home_team = item;
                }
            }
            foreach (var item in team)
            {
                if (item.placeInGroup == groupStage.away)
                {
                    away_team = item;
                }
            }
            Random random = new Random();
            int totalStrength = home_team.placeInGlobalRanking + away_team.placeInGlobalRanking;
            float homeStrength = (float)away_team.placeInGlobalRanking/ totalStrength;
            float awayStrength = (float)home_team.placeInGlobalRanking/ totalStrength;
            float homeOpportunities = random.Next(0, 7);
            float awayOpportunities = random.Next(0, 7);
            int homeGoals = (int)Math.Floor(homeOpportunities * homeStrength);
            int awayGoals = (int)Math.Floor(awayOpportunities * awayStrength);
            bool isHomeWinner = false;
            
            if (homeGoals > awayGoals)
            {
                isHomeWinner = true;
            }
            else
            {
                isHomeWinner = false;
            }
            var match = new MatchVM()
            {
                homeOpportunities = (int)homeOpportunities,
                awayOpportunities = (int)awayOpportunities,
                homeEfficiency = homeStrength,
                awayEfficiency = awayStrength,
                homeGoals = homeGoals,
                awayGoals = awayGoals,
                isHomeWinner = isHomeWinner,
                homePlaceInGroup = home_team.placeInGroup,
                awayPlaceInGroup = away_team.placeInGroup
            };
            return match;
        }
        
        private string generateID()
        {
            Guid guid = Guid.NewGuid();
            string str = guid.ToString();
            return str;
        }
    }
}
