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
        private readonly ISimulatedKnockoutPhaseService _simulatedKnockoutPhaseService;
        public KnockoutStageController(ITeamService teamservice, IGroupStageService groupstageservice, IMatchesService matchesService, IKnockoutStageService knockoutStageService, IPromotedTeamsService promotedTeamsService, ISimulatedKnockoutPhaseService simulatedKnockoutPhaseService)
        {
            _teamservice = teamservice;
            _groupstageservice = groupstageservice;
            _matchesService = matchesService;
            _knockoutStageservice = knockoutStageService;
            _promotedteamsservice = promotedTeamsService;
            _simulatedKnockoutPhaseService = simulatedKnockoutPhaseService;
        }
        public ActionResult KnockoutStageById(string? id)
        {
            var alreadySimulatedKnockoutPhase = _simulatedKnockoutPhaseService.GetAllSimulatedKnockoutPhase(id);
            if (alreadySimulatedKnockoutPhase != null)
            {
                SimulatedKnockoutPhaseToJsonVM simulated = JsonConvert.DeserializeObject<SimulatedKnockoutPhaseToJsonVM>(alreadySimulatedKnockoutPhase);
                WinnerVM[] winnersKSVM = new WinnerVM[16];
                winnersKSVM = simulated.winnerVMs;
                ViewBag.quarterteams = simulated.quarterFinalTeamsVMs;
                ViewBag.semifinalteams = simulated.semiFinalTeamsVMs;
                ViewBag.thirdplace = simulated.thirdPlaceTeamsVMs;
                ViewBag.final = simulated.finalTeamsVMs;
                ViewBag.teams = simulated.teamVMs;
                return View(simulated.matchesKSVM);
            }
            else
            {
                ViewBag.id = id;
                var knockoutStageVMs = _knockoutStageservice.GetAllEntries();
                var json_teams = _promotedteamsservice.GetAllPromotedTeams(id);
                var teams = JsonConvert.DeserializeObject<TeamVM[]>(json_teams);
                MatchKnockoutStageVM[] matchesKSVM = new MatchKnockoutStageVM[16];
                string[] placesInKnockoutStage = new string[32];
                WinnerVM[] winnersKSVM = new WinnerVM[16];
                int j = 0;
                for (int i = 0; i < 16; i++)
                {
                    placesInKnockoutStage[j] = knockoutStageVMs[i].home;
                    j++;
                    placesInKnockoutStage[j] = knockoutStageVMs[i].away;
                    j++;
                }
                int index = 0;
                // 1/16
                int numOfMatchesInThisStage = 8;
                while (index < numOfMatchesInThisStage)
                {
                    (matchesKSVM[index], winnersKSVM[index]) = SimulateMatchForFirstKnockoutPhase(index, 16, knockoutStageVMs, placesInKnockoutStage, teams);
                    index++;
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
                                placeInGroup = newPlaceInTheGroup,
                                placeInGlobalRanking = teams[k].placeInGlobalRanking,
                            };
                            quarterfinal_teamVMs[i] = team;
                        }
                    }
                }
                numOfMatchesInThisStage = numOfMatchesInThisStage + 4;
                while (index < numOfMatchesInThisStage)
                {
                    (matchesKSVM[index], winnersKSVM[index]) = SimulateMatchForQuarterfinal(index, 8, knockoutStageVMs, placesInKnockoutStage, quarterfinal_teamVMs);
                    index++;
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
                                placeInGroup = newPlaceInTheGroup,
                                placeInGlobalRanking = quarterfinal_teamVMs[k].placeInGlobalRanking
                            };
                            semifinal_teamVMs[i] = team;
                        }
                    }
                }
                numOfMatchesInThisStage = numOfMatchesInThisStage + 2;
                LoserOfSemiFinalVM[] loserOfSemiFinalVMs = new LoserOfSemiFinalVM[2];
                int indexForLosers = 0;
                while (index < numOfMatchesInThisStage)
                {
                    (matchesKSVM[index], winnersKSVM[index], loserOfSemiFinalVMs[indexForLosers]) = SimulateMatchForSemifinal(index, 4, knockoutStageVMs, placesInKnockoutStage, semifinal_teamVMs);
                    index++;
                    indexForLosers++;
                }
                ViewBag.semifinalteams = semifinal_teamVMs;
                FinalTeamsVM[] final_teamVMs = new FinalTeamsVM[2];
                ThirdPlaceTeamsVM[] thirdPlaceMatchVMs = new ThirdPlaceTeamsVM[2];
                for (int i = 0; i < 2; i++)
                {
                    string teamPlaceInTheGroup_winner = winnersKSVM[i + 12].teamPlaceInGroup;
                    string newPlaceInTheGroup_winner = winnersKSVM[i + 12].newPlaceInKnockoutStage;
                    string teamPlaceInTheGroup_loser = loserOfSemiFinalVMs[i].teamPlaceInGroup;
                    string newPlaceInTheGroup_loser = loserOfSemiFinalVMs[i].newPlaceInKnockoutStage;
                    for (int k = 0; k < 4; k++)
                    {
                        if (teamPlaceInTheGroup_winner == semifinal_teamVMs[k].placeInGroup)
                        {
                            var team = new FinalTeamsVM()
                            {
                                teamId = semifinal_teamVMs[k].teamId,
                                name = semifinal_teamVMs[k].name,
                                picture = semifinal_teamVMs[k].picture,
                                placeInGroup = newPlaceInTheGroup_winner,
                                placeInGlobalRanking = semifinal_teamVMs[k].placeInGlobalRanking
                            };
                            final_teamVMs[i] = team;
                        }
                        else if (teamPlaceInTheGroup_loser == semifinal_teamVMs[k].placeInGroup)
                        {
                            var team = new ThirdPlaceTeamsVM()
                            {
                                teamId = semifinal_teamVMs[k].teamId,
                                name = semifinal_teamVMs[k].name,
                                picture = semifinal_teamVMs[k].picture,
                                placeInGroup = newPlaceInTheGroup_loser,
                                placeInGlobalRanking = semifinal_teamVMs[k].placeInGlobalRanking
                            };
                            thirdPlaceMatchVMs[i] = team;
                        }
                    }
                }
                (matchesKSVM[index], winnersKSVM[index]) = SimulateMatchForThirdPlace(index, 2, knockoutStageVMs[15], placesInKnockoutStage, thirdPlaceMatchVMs);
                (matchesKSVM[index + 1], winnersKSVM[index + 1]) = SimulateMatchForFinal(index + 1, 2, knockoutStageVMs[14], placesInKnockoutStage, final_teamVMs);

                ViewBag.thirdplace = thirdPlaceMatchVMs;
                ViewBag.final = final_teamVMs;
                ViewBag.teams = teams;
                var simulatedPhaseToJson = new SimulatedKnockoutPhaseToJsonVM()
                {
                    matchesKSVM = matchesKSVM,
                    winnerVMs = winnersKSVM,
                    quarterFinalTeamsVMs = quarterfinal_teamVMs,
                    semiFinalTeamsVMs = semifinal_teamVMs,
                    thirdPlaceTeamsVMs = thirdPlaceMatchVMs,
                    finalTeamsVMs = final_teamVMs,
                    teamVMs = teams
                };
                var simulatedPhaseJson = JsonConvert.SerializeObject(simulatedPhaseToJson);
                _simulatedKnockoutPhaseService.SaveSimulatedKnockoutPhase(id, simulatedPhaseJson);
                return View(matchesKSVM);
            }
        }
        private (MatchKnockoutStageVM, WinnerVM) SimulateMatchForFirstKnockoutPhase(int i, int numOfTeamsInThisStage, KnockoutStageVM[] knockoutStageVMs, string[] places, TeamVM[] teams)
        {
            TeamVM home_team = new TeamVM();
            TeamVM away_team = new TeamVM();
            for (int j = 0; j < numOfTeamsInThisStage; j++)
            {
                if (knockoutStageVMs[i].home == teams[j].placeInGroup)
                {
                    home_team = teams[j];
                }
            }
            for (int j = 0; j < numOfTeamsInThisStage; j++)
            {
                if (knockoutStageVMs[i].away == teams[j].placeInGroup)
                {
                    away_team = teams[j];
                }
            }
            Random random = new Random();
            int totalStrength = home_team.placeInGlobalRanking + away_team.placeInGlobalRanking;
            float homeStrength = (float)away_team.placeInGlobalRanking / totalStrength;
            float awayStrength = (float)home_team.placeInGlobalRanking / totalStrength;
            float homeOpportunities = random.Next(0, 7);
            float awayOpportunities = random.Next(0, 7);
            int homeGoals = (int)Math.Floor(homeOpportunities * homeStrength);
            int awayGoals = (int)Math.Floor(awayOpportunities * awayStrength);
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
                homeGoals = homeGoals,
                awayGoals = awayGoals,
                isDraw = isDraw,
                homePlaceInGroup = knockoutStageVMs[i].home,
                awayPlaceInGroup = knockoutStageVMs[i].away,
                homeGoals_draw = homeGoals_draw,
                awayGoals_draw = awayGoals_draw,
                placeInGlobalRanking_home = home_team.placeInGlobalRanking,
                placeInGlobalRanking_away = away_team.placeInGlobalRanking,
            };
            var winner_team = new WinnerVM() {
                teamPlaceInGroup = winner,
                newPlaceInKnockoutStage = newPlaceInKnockoutStage
            };

            return (match, winner_team);
        }
        private (MatchKnockoutStageVM, WinnerVM) SimulateMatchForQuarterfinal(int i, int numOfTeamsInThisStage, KnockoutStageVM[] knockoutStageVMs, string[] places, QuarterFinalTeamsVM[] teams)
        {
            QuarterFinalTeamsVM home_team = new QuarterFinalTeamsVM();
            QuarterFinalTeamsVM away_team = new QuarterFinalTeamsVM();
            for (int j = 0; j < numOfTeamsInThisStage; j++)
            {
                if (knockoutStageVMs[i].home == teams[j].placeInGroup)
                {
                    home_team = teams[j];
                }
            }
            for (int j = 0; j < numOfTeamsInThisStage; j++)
            {
                if (knockoutStageVMs[i].away == teams[j].placeInGroup)
                {
                    away_team = teams[j];
                }
            }
            Random random = new Random();
            int totalStrength = home_team.placeInGlobalRanking + away_team.placeInGlobalRanking;
            float homeStrength = (float)away_team.placeInGlobalRanking / totalStrength;
            float awayStrength = (float)home_team.placeInGlobalRanking / totalStrength;
            float homeOpportunities = random.Next(0, 7);
            float awayOpportunities = random.Next(0, 7);
            int homeGoals = (int)Math.Floor(homeOpportunities * homeStrength);
            int awayGoals = (int)Math.Floor(awayOpportunities * awayStrength);
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
                homeGoals = homeGoals,
                awayGoals = awayGoals,
                isDraw = isDraw,
                homePlaceInGroup = knockoutStageVMs[i].home,
                awayPlaceInGroup = knockoutStageVMs[i].away,
                homeGoals_draw = homeGoals_draw,
                awayGoals_draw = awayGoals_draw,
                placeInGlobalRanking_home = home_team.placeInGlobalRanking,
                placeInGlobalRanking_away = away_team.placeInGlobalRanking,
            };
            var winner_team = new WinnerVM()
            {
                teamPlaceInGroup = winner,
                newPlaceInKnockoutStage = newPlaceInKnockoutStage
            };

            return (match, winner_team);
        }
        private (MatchKnockoutStageVM, WinnerVM, LoserOfSemiFinalVM) SimulateMatchForSemifinal(int i, int numOfTeamsInThisStage, KnockoutStageVM[] knockoutStageVMs, string[] places, SemiFinalTeamsVM[] teams)
        {
            SemiFinalTeamsVM home_team = new SemiFinalTeamsVM();
            SemiFinalTeamsVM away_team = new SemiFinalTeamsVM();
            for (int j = 0; j < numOfTeamsInThisStage; j++)
            {
                if (knockoutStageVMs[i].home == teams[j].placeInGroup)
                {
                    home_team = teams[j];
                }
            }
            for (int j = 0; j < numOfTeamsInThisStage; j++)
            {
                if (knockoutStageVMs[i].away == teams[j].placeInGroup)
                {
                    away_team = teams[j];
                }
            }
            Random random = new Random();
            int totalStrength = home_team.placeInGlobalRanking + away_team.placeInGlobalRanking;
            float homeStrength = (float)away_team.placeInGlobalRanking / totalStrength;
            float awayStrength = (float)home_team.placeInGlobalRanking / totalStrength;
            float homeOpportunities = random.Next(0, 7);
            float awayOpportunities = random.Next(0, 7);
            int homeGoals = (int)Math.Floor(homeOpportunities * homeStrength);
            int awayGoals = (int)Math.Floor(awayOpportunities * awayStrength);
            int homeGoals_draw = 0;
            int awayGoals_draw = 0;
            bool isDraw = false;
            string winner = "";
            string newPlaceInKnockoutStageForWinner = "";
            string loser = "";
            string newPlaceInKnockoutStageForLoser = "";
            if (homeGoals == awayGoals)
            {
                isDraw = true;
                (homeGoals_draw, awayGoals_draw) = PenaltyKicks();
                if (homeGoals_draw > awayGoals_draw)
                {
                    winner = knockoutStageVMs[i].home;
                    newPlaceInKnockoutStageForWinner = places[i + 16];
                    loser = knockoutStageVMs[i].away;
                    newPlaceInKnockoutStageForLoser = places[i + 18];
                }
                else
                {
                    winner = knockoutStageVMs[i].away;
                    newPlaceInKnockoutStageForWinner = places[i + 16];
                    loser = knockoutStageVMs[i].home;
                    newPlaceInKnockoutStageForLoser = places[i + 18];
                }
            }
            else
            {
                if (homeGoals > awayGoals)
                {
                    winner = knockoutStageVMs[i].home;
                    newPlaceInKnockoutStageForWinner = places[i + 16];
                    loser = knockoutStageVMs[i].away;
                    newPlaceInKnockoutStageForLoser = places[i + 18];
                }
                else
                {
                    winner = knockoutStageVMs[i].away;
                    newPlaceInKnockoutStageForWinner = places[i + 16];
                    loser = knockoutStageVMs[i].home;
                    newPlaceInKnockoutStageForLoser = places[i + 18];
                }
                isDraw = false;
            }
            var match = new MatchKnockoutStageVM()
            {
                homeGoals = homeGoals,
                awayGoals = awayGoals,
                isDraw = isDraw,
                homePlaceInGroup = knockoutStageVMs[i].home,
                awayPlaceInGroup = knockoutStageVMs[i].away,
                homeGoals_draw = homeGoals_draw,
                awayGoals_draw = awayGoals_draw,
                placeInGlobalRanking_home = home_team.placeInGlobalRanking,
                placeInGlobalRanking_away = away_team.placeInGlobalRanking,
            };
            var winner_team = new WinnerVM()
            {
                teamPlaceInGroup = winner,
                newPlaceInKnockoutStage = newPlaceInKnockoutStageForWinner
            };
            var loser_team = new LoserOfSemiFinalVM()
            {
                teamPlaceInGroup = loser,
                newPlaceInKnockoutStage = newPlaceInKnockoutStageForLoser
            };
            return (match, winner_team, loser_team);
        }
        private (MatchKnockoutStageVM, WinnerVM) SimulateMatchForThirdPlace(int i, int numOfTeamsInThisStage, KnockoutStageVM knockoutStageVMs, string[] places, ThirdPlaceTeamsVM[] teams)
        {
            ThirdPlaceTeamsVM home_team = new ThirdPlaceTeamsVM();
            ThirdPlaceTeamsVM away_team = new ThirdPlaceTeamsVM();
            for (int j = 0; j < numOfTeamsInThisStage; j++)
            {
                if (knockoutStageVMs.home == teams[j].placeInGroup)
                {
                    home_team = teams[j];
                }
            }
            for (int j = 0; j < numOfTeamsInThisStage; j++)
            {
                if (knockoutStageVMs.away == teams[j].placeInGroup)
                {
                    away_team = teams[j];
                }
            }
            Random random = new Random();
            int totalStrength = home_team.placeInGlobalRanking + away_team.placeInGlobalRanking;
            float homeStrength = (float)away_team.placeInGlobalRanking / totalStrength;
            float awayStrength = (float)home_team.placeInGlobalRanking / totalStrength;
            float homeOpportunities = random.Next(0, 7);
            float awayOpportunities = random.Next(0, 7);
            int homeGoals = (int)Math.Floor(homeOpportunities * homeStrength);
            int awayGoals = (int)Math.Floor(awayOpportunities * awayStrength);
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
                    winner = knockoutStageVMs.home;
                    newPlaceInKnockoutStage = places[i + 16];
                }
                else
                {
                    winner = knockoutStageVMs.away;
                    newPlaceInKnockoutStage = places[i + 16];
                }
            }
            else
            {
                if (homeGoals > awayGoals)
                {
                    winner = knockoutStageVMs.home;
                    newPlaceInKnockoutStage = places[i + 16];
                }
                else
                {
                    winner = knockoutStageVMs.away;
                    newPlaceInKnockoutStage = places[i + 16];
                }
                isDraw = false;
            }
            var match = new MatchKnockoutStageVM()
            {
                homeGoals = homeGoals,
                awayGoals = awayGoals,
                isDraw = isDraw,
                homePlaceInGroup = knockoutStageVMs.home,
                awayPlaceInGroup = knockoutStageVMs.away,
                homeGoals_draw = homeGoals_draw,
                awayGoals_draw = awayGoals_draw,
                placeInGlobalRanking_home = home_team.placeInGlobalRanking,
                placeInGlobalRanking_away = away_team.placeInGlobalRanking,
            };
            var winner_team = new WinnerVM()
            {
                teamPlaceInGroup = winner,
                newPlaceInKnockoutStage = newPlaceInKnockoutStage
            };

            return (match, winner_team);
        }
        private (MatchKnockoutStageVM, WinnerVM) SimulateMatchForFinal(int i, int numOfTeamsInThisStage, KnockoutStageVM knockoutStageVMs, string[] places, FinalTeamsVM[] teams)
        {
            FinalTeamsVM home_team = new FinalTeamsVM();
            FinalTeamsVM away_team = new FinalTeamsVM();
            for (int j = 0; j < numOfTeamsInThisStage; j++)
            {
                if (knockoutStageVMs.home == teams[j].placeInGroup)
                {
                    home_team = teams[j];
                }
            }
            for (int j = 0; j < numOfTeamsInThisStage; j++)
            {
                if (knockoutStageVMs.away == teams[j].placeInGroup)
                {
                    away_team = teams[j];
                }
            }
            Random random = new Random();
            int totalStrength = home_team.placeInGlobalRanking + away_team.placeInGlobalRanking;
            float homeStrength = (float)away_team.placeInGlobalRanking / totalStrength;
            float awayStrength = (float)home_team.placeInGlobalRanking / totalStrength;
            float homeOpportunities = random.Next(0, 7);
            float awayOpportunities = random.Next(0, 7);
            int homeGoals = (int)Math.Floor(homeOpportunities * homeStrength);
            int awayGoals = (int)Math.Floor(awayOpportunities * awayStrength);
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
                    winner = knockoutStageVMs.home;
                    newPlaceInKnockoutStage = places[i + 16];
                }
                else
                {
                    winner = knockoutStageVMs.away;
                    newPlaceInKnockoutStage = places[i + 16];
                }
            }
            else
            {
                if (homeGoals > awayGoals)
                {
                    winner = knockoutStageVMs.home;
                    newPlaceInKnockoutStage = places[i + 16];
                }
                else
                {
                    winner = knockoutStageVMs.away;
                    newPlaceInKnockoutStage = places[i + 16];
                }
                isDraw = false;
            }
            var match = new MatchKnockoutStageVM()
            {
                homeGoals = homeGoals,
                awayGoals = awayGoals,
                isDraw = isDraw,
                homePlaceInGroup = knockoutStageVMs.home,
                awayPlaceInGroup = knockoutStageVMs.away,
                homeGoals_draw = homeGoals_draw,
                awayGoals_draw = awayGoals_draw,
                placeInGlobalRanking_home = home_team.placeInGlobalRanking,
                placeInGlobalRanking_away = away_team.placeInGlobalRanking,
            };
            var winner_team = new WinnerVM()
            {
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
            while (homeGoals_draw == awayGoals_draw)
            {
                hEff = (float)(random.Next(0, 100) * 0.01);
                aEff = (float)(random.Next(0, 100) * 0.01);
                homeGoals_draw = (int)Math.Floor(5 * hEff);
                awayGoals_draw = (int)Math.Floor(5 * aEff);
            }
            return (homeGoals_draw, awayGoals_draw);
        }
    }
}
