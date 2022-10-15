using Microsoft.AspNetCore.Mvc;
using WorldCup2022_MVC.Interfaces;
using WorldCup2022_MVC.ViewModels;

namespace WorldCup2022_MVC.Controllers
{
    public class KnockoutStageController : Controller
    {
        private readonly ITeamService _teamservice;
        private readonly IGroupStageService _groupstageservice;
        private readonly IMatchesService _matchesService;
        private readonly IKnockoutStageService _knockoutStageservice;
        public KnockoutStageController(ITeamService teamservice, IGroupStageService groupstageservice, IMatchesService matchesService, IKnockoutStageService knockoutStageService)
        {
            _teamservice = teamservice;
            _groupstageservice = groupstageservice;
            _matchesService = matchesService;
            _knockoutStageservice = knockoutStageService;
        }
        public ActionResult KnockoutStageById(string? id, [FromServices] ITeamService teamservice, [FromServices] IKnockoutStageService knockoutStageService)
        {
            ViewBag.id = id;
            var matches = _knockoutStageservice.GetAllEntries();
            var teams = WinnersOfTheGroup();
            return View();
        }
        private TeamVM[] WinnersOfTheGroup()
        {
            TeamVM[] teamsInKnockoutStage = new TeamVM[16];

            return teamsInKnockoutStage;
        }
    }
}
