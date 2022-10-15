using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WorldCup2022_MVC.Interfaces;
using WorldCup2022_MVC.ViewModels;
using System.Linq;

namespace WorldCup2022_MVC.Controllers
{
    public class TableController : Controller
    {
        private readonly ITeamService _teamservice;
        private readonly IGroupStageService _groupstageservice;
        private readonly IMatchesService _matchesService;
        private List<MatchVM> matches = new List<MatchVM>();
        public TableController(ITeamService teamservice, IGroupStageService groupstageservice, IMatchesService matchesService)
        {
            _teamservice = teamservice;
            _groupstageservice = groupstageservice;
            _matchesService = matchesService;
        }
        [HttpGet]
        public ActionResult TableById(string id, [FromServices] ITeamService teamService, [FromServices] IMatchesService matchesService)
        {
            string data = matchesService.GetAllMatches(id);
            GroupStageVM group = new GroupStageVM();
            ResultsGroupStageVM resultsGroupStage = new ResultsGroupStageVM();
            resultsGroupStage.ListTeamVM = teamService.GetAllEntries();
            resultsGroupStage.TeamsMatchesVM = JsonConvert.DeserializeObject<TeamsMatchesVM>(data);
            var results = DataForTable(resultsGroupStage);
            ViewBag.id = id;

            var sorted_results = SortResults(results);
            var jsonOfWinners = TeamsToKnockoutStage(sorted_results);
            return View(sorted_results);
        }
        private Dictionary<TeamVM, StatisticsVM> DataForTable(ResultsGroupStageVM rgs)
        {
            Dictionary<TeamVM, StatisticsVM> data = new Dictionary<TeamVM, StatisticsVM>();
            int[] idOfMatches = new int[4];
            int indexForArray = 0;
            int index = 0;
            foreach (var team in rgs.ListTeamVM)
            {
                foreach (var matchOrder in rgs.TeamsMatchesVM.listOfMatches)
                {
                    if (team.placeInGroup == matchOrder.home || team.placeInGroup == matchOrder.away)
                    {
                        idOfMatches[indexForArray] = index;
                        indexForArray++;
                    }
                    index++;
                }
                var statistics = StatisticsForTheTeam(rgs, idOfMatches, team.placeInGroup);
                data.Add(team, statistics);
                indexForArray = 0;
                index = 0;
            }

            return data;
        }
        private StatisticsVM StatisticsForTheTeam(ResultsGroupStageVM rgs, int[] array, string placeIngroup)
        {
            StatisticsVM stats = new StatisticsVM();
            int points = 0;
            for (int i = 0; i < 3; i++)
            {
                if (rgs.TeamsMatchesVM.arrayOfResult[array[i]].homePlaceInGroup == placeIngroup)
                {
                    stats.opportunities = stats.opportunities + rgs.TeamsMatchesVM.arrayOfResult[array[i]].homeOpportunities;
                    stats.efficiency = (stats.efficiency + rgs.TeamsMatchesVM.arrayOfResult[array[i]].homeEfficiency) / 2;
                    stats.goals_lost = stats.goals_lost + rgs.TeamsMatchesVM.arrayOfResult[array[i]].awayGoals;
                    stats.goals_scored = stats.goals_scored + rgs.TeamsMatchesVM.arrayOfResult[array[i]].homeGoals;
                    stats.goal_balance = stats.goals_scored - stats.goals_lost;
                    if (rgs.TeamsMatchesVM.arrayOfResult[array[i]].homeGoals > rgs.TeamsMatchesVM.arrayOfResult[array[i]].awayGoals)
                    {
                        points = points + 3;
                    }
                    else if (rgs.TeamsMatchesVM.arrayOfResult[array[i]].homeGoals == rgs.TeamsMatchesVM.arrayOfResult[array[i]].awayGoals)
                    {
                        points = points + 1;
                    }
                }
                else if (rgs.TeamsMatchesVM.arrayOfResult[array[i]].awayPlaceInGroup == placeIngroup)
                {
                    stats.opportunities = stats.opportunities + rgs.TeamsMatchesVM.arrayOfResult[array[i]].awayOpportunities;
                    stats.efficiency = (stats.efficiency + rgs.TeamsMatchesVM.arrayOfResult[array[i]].awayEfficiency) / 2;
                    stats.goals_lost = stats.goals_lost + rgs.TeamsMatchesVM.arrayOfResult[array[i]].homeGoals;
                    stats.goals_scored = stats.goals_scored + rgs.TeamsMatchesVM.arrayOfResult[array[i]].awayGoals;
                    stats.goal_balance = stats.goals_scored - stats.goals_lost;
                    if (rgs.TeamsMatchesVM.arrayOfResult[array[i]].homeGoals < rgs.TeamsMatchesVM.arrayOfResult[array[i]].awayGoals)
                    {
                        points = points + 3;
                    }
                    else if (rgs.TeamsMatchesVM.arrayOfResult[array[i]].homeGoals == rgs.TeamsMatchesVM.arrayOfResult[array[i]].awayGoals)
                    {
                        points = points + 1;
                    }
                }

            }
            double x = Math.Round(stats.efficiency, 2);
            stats.efficiency = x;
            stats.points = points;
            return stats;
        }
        private Dictionary<TeamVM, StatisticsVM> SortResults(Dictionary<TeamVM, StatisticsVM> dictonary)
        {
            Dictionary<TeamVM, StatisticsVM> sorted = new Dictionary<TeamVM, StatisticsVM>();
            int x = 1;
            for (int i = 1; i < 33; i = x + 1)
            {
                var first = dictonary.FirstOrDefault(r =>
                r.Key.teamId.Equals(i)
                );
                var the_best = first;

                var second = dictonary.FirstOrDefault(r =>
                    r.Key.teamId.Equals(i+1)
                );
                var worse = second;

                var third = dictonary.FirstOrDefault(r =>
                    r.Key.teamId.Equals(i+2)
                );
                var good = third;
                x = i + 3;
                var fourth = dictonary.FirstOrDefault(r =>
                    r.Key.teamId.Equals(x)
                );
                var the_worst = fourth;

                if (first.Value.points < second.Value.points)
                {
                    the_best = second;
                    worse = first;
                }
                else if (first.Value.points == second.Value.points && first.Value.goal_balance < second.Value.goal_balance)
                {
                    the_best = second;
                    worse = first;
                }

                if (third.Value.points < fourth.Value.points)
                {
                    good = fourth;
                    the_worst = third;
                }
                else if (third.Value.points == fourth.Value.points && third.Value.goal_balance < fourth.Value.goal_balance)
                {
                    good = fourth;
                    the_worst = third;
                }
                if (the_best.Value.points < good.Value.points)
                {
                    var tmp = the_best;
                    the_best = good;
                    good = tmp;
                }
                else if (the_best.Value.points == good.Value.points && the_best.Value.goal_balance < good.Value.goal_balance)
                {
                    var tmp = the_best;
                    the_best = good;
                    good = tmp;
                }
                if (worse.Value.points > good.Value.points)
                {
                    var tmp = good;
                    good = worse;
                    worse = tmp;
                }
                else if (worse.Value.points == good.Value.points && good.Value.goal_balance < worse.Value.goal_balance)
                {
                    var tmp = good;
                    good = worse;
                    worse = tmp;
                }
                if (the_worst.Value.points > good.Value.points)
                {
                    var tmp = good;
                    good = the_worst;
                    the_worst = tmp;
                }
                else if (the_worst.Value.points == good.Value.points && good.Value.goal_balance < the_worst.Value.goal_balance)
                {
                    var tmp = good;
                    good = the_worst;
                    the_worst = tmp;
                }
                if (worse.Value.points < the_worst.Value.points)
                {
                    var tmp = the_worst;
                    the_worst = worse;
                    worse = tmp;
                }
                else if (worse.Value.points == the_worst.Value.points && worse.Value.goal_balance < the_worst.Value.goal_balance)
                {
                    var tmp = the_worst;
                    the_worst = worse;
                    worse = tmp;
                }



                sorted.Add((TeamVM)the_best.Key, (StatisticsVM)the_best.Value);
                sorted.Add((TeamVM)good.Key, (StatisticsVM)good.Value);
                sorted.Add((TeamVM)worse.Key, (StatisticsVM)worse.Value);
                sorted.Add((TeamVM)the_worst.Key, (StatisticsVM)the_worst.Value);
            }
            return sorted;
        }
        private string generateID()
        {
            Guid guid = Guid.NewGuid();
            string str = guid.ToString();
            return str;
        }
        private string TeamsToKnockoutStage(Dictionary<TeamVM, StatisticsVM> dictionary)
        {
            int i = 1;
            int index = 0;
            TeamVM[] teams = new TeamVM[dictionary.Count];
            foreach (var item in dictionary)
            {
                if (i != 3 && i != 4 && i != 7 && i != 8 && i != 11 && i != 12 && i != 15 && i != 16 && i != 19 && i != 20 && i != 23 && i != 24 && i != 27 && i != 28 && i != 31 && i != 32)
                {
                    teams[index] = item.Key;
                    index++;
                }
                i++;
            }
            var json = JsonConvert.SerializeObject(teams);
            return json;
        }
    }
}
