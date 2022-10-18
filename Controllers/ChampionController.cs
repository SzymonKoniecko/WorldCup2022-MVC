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
        public ChampionController(ITeamService teamservice, IGroupStageService groupstageservice, IMatchesService matchesService, IKnockoutStageService knockoutStageService, IPromotedTeamsService promotedTeamsService)
        {
            _teamservice = teamservice;
            _groupstageservice = groupstageservice;
            _matchesService = matchesService;
            _knockoutStageservice = knockoutStageService;
            _promotedteamsservice = promotedTeamsService;
        }
        public IActionResult ChooseYourChampion()
        {
            var teams = _teamservice.GetAllEntries();
            return View(teams);
        }
        public IActionResult Simulator(int? teamId)
        {
            var teams = _teamservice.GetAllEntries();
            TeamVM teamVM = new TeamVM();
            foreach (var item in teams)
            {
                if (item.teamId == teamId)
                {
                    teamVM = item;
                }
            }
            bool founded_simulation = false;
            string id_for_simulation = "";
            int index = 0;
            while (founded_simulation)
            {
                PlayGroupController playGroupController = new PlayGroupController(_teamservice, _groupstageservice, _matchesService);
                playGroupController.PlayGroup(_teamservice, _groupstageservice, _matchesService);
                playGroupController.ViewBag.id = id_for_simulation;
                TableController tableController = new TableController(_teamservice, _groupstageservice, _matchesService, _promotedteamsservice);
                tableController.TableById(id_for_simulation, _teamservice, _matchesService, _promotedteamsservice);
                KnockoutStageController knockoutStageController = new KnockoutStageController(_teamservice, _groupstageservice, _matchesService, _knockoutStageservice, _promotedteamsservice);
                knockoutStageController.KnockoutStageById(id_for_simulation);
                for (int i = 0; i < 2; i++)
                {
                    if (knockoutStageController.ViewBag.final[i].teamId == teamVM.teamId)
                    {
                        founded_simulation = true;
                    }
                    else
                    {
                        index++;
                    }
                }
            }
            ViewBag.index = index;
            ViewBag.id_for_simulation = id_for_simulation;
            return View();
        }
    }
}
