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
            MatchKnockoutStageVM[] matchesKSVM = new MatchKnockoutStageVM[8];
            string[] winners = new string[8];
            string[] placesInKnockoutStage = new string[32];
            WinnerVM[] winnersKSVM = new WinnerVM[16];
            int j = 0;
            for (int i = 0; i < 16; i++)
            {
                placesInKnockoutStage[j] = matches[i].home;
                j++;
                placesInKnockoutStage[j] = matches[i].away;
                j++;
            }
            for (int i = 0; i < 8; i++)
            {
                (matchesKSVM[i], winnersKSVM[i]) = SimulateMatchForKnockoutPhase(i, matches, placesInKnockoutStage);
            }
            for (int i = 0; i < 8; i++)
            {
                Console.WriteLine(winnersKSVM[i].teamPlaceInGroup + " " + winnersKSVM[i].newPlaceInKnockoutStage);
            }
            return View(matchesKSVM);
        }
        private (MatchKnockoutStageVM, WinnerVM) SimulateMatchForKnockoutPhase(int i, KnockoutStageVM[] knockoutStageVMs, string[] places)
        {
            Random random = new Random();
            int hOppor = random.Next(0, 7);
            int aOppor = random.Next(0, 7);
            float hEff = (float)(random.Next(0, 100) * 0.01);
            float aEff = (float)(random.Next(0, 100) * 0.01);
            int homeGoals = (int)Math.Floor(hOppor * hEff);
            int awayGoals = (int)Math.Floor(aOppor * aEff);
            int homeGoals_draw = 0;
            int awayGoals_draw = 0;
            bool isDraw = false;
            string winner = "";
            string newPlaceInKnockoutStage = "";
            if (homeGoals == awayGoals)
            {
                isDraw = true;
                (homeGoals_draw, awayGoals_draw) = PenaltyKicks();
                if (homeGoals_draw > awayGoals_draw)
                {
                    winner = knockoutStageVMs[i].home;
                    newPlaceInKnockoutStage = places[i + 16];
                }
                else
                {
                    winner = knockoutStageVMs[i].away;
                    newPlaceInKnockoutStage = places[i + 16];
                }
            }
            else
            {
                if (homeGoals > awayGoals)
                {
                    winner = knockoutStageVMs[i].home;
                    newPlaceInKnockoutStage = places[i + 16];
                }
                else
                {
                    winner = knockoutStageVMs[i].away;
                    newPlaceInKnockoutStage = places[i + 16];
                }
                isDraw = false;
            }
            var match = new MatchKnockoutStageVM()
            {
                homeGoals = (int)Math.Floor(hOppor * hEff),
                awayGoals = (int)Math.Floor(aOppor * aEff),
                isDraw = isDraw,
                homePlaceInGroup = knockoutStageVMs[i].home,
                awayPlaceInGroup = knockoutStageVMs[i].away,
                homeGoals_draw = homeGoals_draw,
                awayGoals_draw = awayGoals_draw,
            };
            var winner_team = new WinnerVM() {
                teamPlaceInGroup = winner,
                newPlaceInKnockoutStage = newPlaceInKnockoutStage
            };

            return (match, winner_team);
        }
        private (int, int) PenaltyKicks()
        {
            Random random = new Random();
            float hEff = (float)(random.Next(0, 100) * 0.01);
            float aEff = (float)(random.Next(0, 100) * 0.01);
            int homeGoals_draw = (int)Math.Floor(5 * hEff);
            int awayGoals_draw = (int)Math.Floor(5 * aEff);
            return (homeGoals_draw, awayGoals_draw);
        }
    }
}
