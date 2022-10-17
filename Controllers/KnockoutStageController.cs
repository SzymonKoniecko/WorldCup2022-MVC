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
            var knockoutStageVMs = _knockoutStageservice.GetAllEntries();
            var json_teams = _promotedteamsservice.GetAllPromotedTeams(id);
            var teams = JsonConvert.DeserializeObject<TeamVM[]>(json_teams);
            MatchKnockoutStageVM[] matchesKSVM = new MatchKnockoutStageVM[15];
            string[] placesInKnockoutStage = new string[32];
            WinnerVM[] winnersKSVM = new WinnerVM[15];
            int j = 0;
            for (int i = 0; i < 16; i++)
            {
                placesInKnockoutStage[j] = knockoutStageVMs[i].home;
                j++;
                placesInKnockoutStage[j] = knockoutStageVMs[i].away;
                j++;
            }
            for (int i = 0; i < 15; i++)
            { // 1/16
                (matchesKSVM[i], winnersKSVM[i]) = SimulateMatchForKnockoutPhase(i, knockoutStageVMs, placesInKnockoutStage);
            }
            /// 1/8
            QuarterFinalTeamsVM[] quarterfinal_teamVMs = new QuarterFinalTeamsVM[8];
            for (int i = 0; i < 8; i++)
            {
                string teamPlaceInTheGroup = winnersKSVM[i].teamPlaceInGroup;
                string newPlaceInTheGroup = winnersKSVM[i].newPlaceInKnockoutStage;
                for (int k = 0; k < 16; k++)
                {
                    if (teamPlaceInTheGroup == teams[k].placeInGroup)
                    {
                        var team = new QuarterFinalTeamsVM()
                        {
                            teamId = teams[k].teamId,
                            name = teams[k].name,
                            picture = teams[k].picture,
                            placeInGroup = newPlaceInTheGroup
                        };
                        quarterfinal_teamVMs[i] = team;
                    }
                }
            }
            ViewBag.quarterteams = quarterfinal_teamVMs;
            SemiFinalTeamsVM[] semifinal_teamVMs = new SemiFinalTeamsVM[4];
            for (int i = 0; i < 4; i++)
            {
                string teamPlaceInTheGroup = winnersKSVM[i + 8].teamPlaceInGroup;
                string newPlaceInTheGroup = winnersKSVM[i + 8].newPlaceInKnockoutStage;
                for (int k = 0; k < 8; k++)
                {
                    if (teamPlaceInTheGroup == quarterfinal_teamVMs[k].placeInGroup)
                    {
                        var team = new SemiFinalTeamsVM()
                        {
                            teamId = quarterfinal_teamVMs[k].teamId,
                            name = quarterfinal_teamVMs[k].name,
                            picture = quarterfinal_teamVMs[k].picture,
                            placeInGroup = newPlaceInTheGroup
                        };
                        semifinal_teamVMs[i] = team;
                    }
                }
            }
            ViewBag.semifinalteams = semifinal_teamVMs;
            FinalTeamsVM[] final_teamVMs = new FinalTeamsVM[2];
            for (int i = 0; i < 2; i++)
            {
                string teamPlaceInTheGroup = winnersKSVM[i + 12].teamPlaceInGroup;
                string newPlaceInTheGroup = winnersKSVM[i + 12].newPlaceInKnockoutStage;
                for (int k = 0; k < 4; k++)
                {
                    if (teamPlaceInTheGroup == semifinal_teamVMs[k].placeInGroup)
                    {
                        var team = new FinalTeamsVM()
                        {
                            teamId = semifinal_teamVMs[k].teamId,
                            name = semifinal_teamVMs[k].name,
                            picture = semifinal_teamVMs[k].picture,
                            placeInGroup = newPlaceInTheGroup
                        };
                        final_teamVMs[i] = team;
                    }
                }
            }
            ViewBag.final = final_teamVMs;
            ViewBag.teams = teams;
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
