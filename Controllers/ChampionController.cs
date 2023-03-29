using Microsoft.AspNetCore.Mvc;
using WorldCup2022_MVC.Interfaces;
using WorldCup2022_MVC.ViewModels;
using WorldCup2022_MVC.Controllers;

namespace WorldCup2022_MVC.Controllers
{
    public class ChampionController : Controller
    {
        private readonly ITeamService _teamservice;
        private readonly IGroupStageService _groupstageservice;
        private readonly IMatchesService _matchesService;
        private readonly IKnockoutStageService _knockoutStageservice;
        private readonly IPromotedTeamsService _promotedteamsservice;
        private readonly ISimulatedKnockoutPhaseService _simulatedKnockoutPhaseService;
        public ChampionController(ITeamService teamservice, IGroupStageService groupstageservice, IMatchesService matchesService, IKnockoutStageService knockoutStageService, IPromotedTeamsService promotedTeamsService, ISimulatedKnockoutPhaseService simulatedKnockoutPhaseService)
        {
            _teamservice = teamservice;
            _groupstageservice = groupstageservice;
            _matchesService = matchesService;
            _knockoutStageservice = knockoutStageService;
            _promotedteamsservice = promotedTeamsService;
            _simulatedKnockoutPhaseService = simulatedKnockoutPhaseService;
        }
        public IActionResult ChooseYourChampion()
        {
            var teams = _teamservice.GetAllEntries();
            teams.Reverse();
            return View(teams);
        }
        public IActionResult Simulator(int? teamId)
        {
            SimulationTimeVM simulationTimeVM = new();
            simulationTimeVM.StartTimeOfSimulation = DateTime.Now;
            var teams = _teamservice.GetAllEntries();
            TeamVM teamVM = new TeamVM();
            foreach (var item in teams)
            {
                if (item.teamId == teamId)
                {
                    teamVM = item;
                }
            }
            bool founded_simulation = true;
            string id_for_simulation = "";
            int index = 0;
            while (founded_simulation)
            {
                PlayGroupController playGroupController = new PlayGroupController(_teamservice, _groupstageservice, _matchesService, _promotedteamsservice);
                playGroupController.PlayGroup();
                id_for_simulation = playGroupController.ViewBag.id;
                var alldata = playGroupController.ViewBag.alldata;
                TableController tableController = new TableController(_teamservice, _groupstageservice, _matchesService, _promotedteamsservice);
                tableController.TableById(id_for_simulation);
                KnockoutStageController knockoutStageController = new KnockoutStageController(_teamservice, _groupstageservice, _matchesService, _knockoutStageservice, _promotedteamsservice, _simulatedKnockoutPhaseService);
                knockoutStageController.KnockoutStageById(id_for_simulation);
                var finalTeams = knockoutStageController.ViewBag.final;
                var matches = knockoutStageController.ViewBag.matches;
                if (matches[15].homeGoals > matches[15].awayGoals)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        if (finalTeams[i].placeInGroup == matches[15].homePlaceInGroup)
                        {
                            if (finalTeams[i].teamId == teamVM.teamId)
                            {
                                founded_simulation = false;
                                ViewBag.id_for_simulation = id_for_simulation;
                            }
                        }
                    }
                }
                else if (matches[15].homeGoals < matches[15].awayGoals)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        if (finalTeams[i].placeInGroup == matches[15].awayPlaceInGroup)
                        {
                            if (finalTeams[i].teamId == teamVM.teamId)
                            {
                                founded_simulation = false;
                                ViewBag.id_for_simulation = id_for_simulation;
                            }
                        }
                    }
                }
                else if (matches[15].homeGoals == matches[15].awayGoals)
                {
                    if (matches[15].homeGoals_draw > matches[15].awayGoals_draw)
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            if (finalTeams[i].placeInGroup == matches[15].homePlaceInGroup)
                            {
                                if (finalTeams[i].teamId == teamVM.teamId)
                                {
                                    founded_simulation = false;
                                    ViewBag.id_for_simulation = id_for_simulation;
                                }
                            }
                        }
                    }
                    else if (matches[15].homeGoals_draw < matches[15].awayGoals_draw)
                        for (int i = 0; i < 2; i++)
                        {
                            if (finalTeams[i].placeInGroup == matches[15].awayPlaceInGroup)
                            {
                                if (finalTeams[i].teamId == teamVM.teamId)
                                {
                                    founded_simulation = false;
                                    ViewBag.id_for_simulation = id_for_simulation;
                                }
                            }
                        }
                }
                index++;
            }
            simulationTimeVM.EndTimeOfSimulation = DateTime.Now;
            ViewBag.SimulationTime = simulationTimeVM;
            ViewBag.team = teamVM;
            ViewBag.index = index;
            teams.Reverse();
            return View(teams);
        }
    }
}
