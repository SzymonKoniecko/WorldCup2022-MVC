using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
        private readonly IPromotedTeamsService _promotedteamsservice;
        public KnockoutStageController(ITeamService teamservice, IGroupStageService groupstageservice, IMatchesService matchesService, IKnockoutStageService knockoutStageService, IPromotedTeamsService promotedTeamsService)
        {
            _teamservice = teamservice;
            _groupstageservice = groupstageservice;
            _matchesService = matchesService;
            _knockoutStageservice = knockoutStageService;
            _promotedteamsservice = promotedTeamsService;
        }
        public ActionResult KnockoutStageById(string? id)
        {
            ViewBag.id = id;
            var matches = _knockoutStageservice.GetAllEntries();
            var json_teams = _promotedteamsservice.GetAllPromotedTeams(id);
            var teams = JsonConvert.DeserializeObject<TeamVM[]>(json_teams);
            ViewBag.teams = teams;
            MatchVM[] matchesVM = new MatchVM[16];
            for (int i = 0; i < 8; i++)
            {
                matchesVM[i] = SimulateMatchForKnockoutPhase(i, matches);
            }
            return View(matchesVM);
        }
        private MatchVM SimulateMatchForKnockoutPhase(int i, KnockoutStageVM[] knockoutStageVMs)
        {
            Random random = new Random();
            int hOppor = random.Next(0, 7);
            int aOppor = random.Next(0, 7);
            float hEff = (float)(random.Next(0, 100) * 0.01);
            float aEff = (float)(random.Next(0, 100) * 0.01);
            int homeGoals = (int)Math.Floor(hOppor * hEff);
            int awayGoals = (int)Math.Floor(aOppor * aEff);
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
                homeOpportunities = hOppor,
                awayOpportunities = aOppor,
                homeEfficiency = hEff,
                awayEfficiency = aEff,
                homeGoals = (int)Math.Floor(hOppor * hEff),
                awayGoals = (int)Math.Floor(aOppor * aEff),
                isHomeWinner = isHomeWinner,
                homePlaceInGroup = knockoutStageVMs[i].home,
                awayPlaceInGroup = knockoutStageVMs[i].away
            };
            return match;
        }
    }
}
