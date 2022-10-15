using Microsoft.AspNetCore.Http;
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
        private List<MatchVM> matches = new List<MatchVM>();
        public PlayGroupController(ITeamService teamservice, IGroupStageService groupstageservice, IMatchesService matchesService)
        {
            _teamservice = teamservice;
            _groupstageservice = groupstageservice;
            _matchesService = matchesService;
        }

        [HttpGet]
        public ActionResult PlayGroup([FromServices] ITeamService teamservice, [FromServices] IGroupStageService stageservice, [FromServices] IMatchesService matchesService)
        {
            var id = generateID();
            MatchVM[] result = new MatchVM[49];
            var listOfPendingMatches = _groupstageservice.GetAllMatches();
            GroupStageVM[] arrayOfPendingMatches = new GroupStageVM[49];
            int indexToCopyListToArray = 0;
            foreach (var item in listOfPendingMatches)
            {
                arrayOfPendingMatches[indexToCopyListToArray] = item;
                indexToCopyListToArray++;
            }
            for (int i = 0; i < 48; i++)
            {
                result[i] = Play(i, arrayOfPendingMatches);
            }
            var alldata = new TeamsMatchesVM
            {
                listOfTeams = _teamservice.GetAllEntries(),
                listOfMatches = listOfPendingMatches,
                arrayOfResult = result
            };
            var json = JsonConvert.SerializeObject(alldata);
            _matchesService.SaveAllMatches(id, json);
            ViewBag.id = id;
            return View(alldata);
        }
        [HttpGet]
        public ActionResult PlayGroupById(string? id, [FromServices] IMatchesService matchesService)
        {
            string data = matchesService.GetAllMatches(id);
            TeamsMatchesVM matches = JsonConvert.DeserializeObject<TeamsMatchesVM>(data);
            ViewBag.id = id;
            return View(matches);
        }
        [HttpPost]
        private MatchVM Play(int i, GroupStageVM[] array_gs)
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
                homePlaceInGroup = array_gs[i].home,
                awayPlaceInGroup = array_gs[i].away
            };
            return match;
        }
        [HttpGet]
        //public ActionResult Table(string? id, [FromServices] ITeamService teamService,[FromServices] IMatchesService matchesService)
        //{
        //    string data = matchesService.GetAllMatches(id);
        //    GroupStageVM group = new GroupStageVM();
        //    ResultsGroupStageVM resultsGroupStage = new ResultsGroupStageVM();
        //    resultsGroupStage.ListTeamVM = teamService.GetAllEntries();
        //    resultsGroupStage.TeamsMatchesVM = JsonConvert.DeserializeObject<TeamsMatchesVM>(data);
        //    var results = DataForTable(resultsGroupStage);
        //    return View();
        //}

        //private Dictionary<TeamVM, StatisticsVM> DataForTable(ResultsGroupStageVM rgs)
        //{
        //    Dictionary<TeamVM, StatisticsVM> data = new Dictionary<TeamVM, StatisticsVM>();
        //    int[] idOfMatches = new int[3];
        //    int indexForArray = 0;
        //    int index = 0;
        //    foreach (var team in rgs.ListTeamVM)
        //    {
        //        foreach (var matchOrder in rgs.TeamsMatchesVM.listOfMatches)
        //        {
        //            if (team.placeInGroup == matchOrder.home || team.placeInGroup == matchOrder.away)
        //            {
        //                idOfMatches[indexForArray] = index;
        //                indexForArray++;
        //            }
        //            index++;
        //        }
        //        var statistics = StatisticsForTheTeam(rgs, idOfMatches, team.placeInGroup);
        //        data.Add(team, statistics);
        //    }

        //    return data;
        //}
        //private StatisticsVM StatisticsForTheTeam(ResultsGroupStageVM rgs, int[] array, string placeIngroup)
        //{
        //    StatisticsVM stats = new StatisticsVM();
        //    int points = 0;
        //    //int numOfPlayedMatch = 0;
        //    for (int i = 0; i < 3; i++)
        //    {
        //        if (rgs.TeamsMatchesVM.arrayOfResult[array[i]].homePlaceInGroup == placeIngroup)
        //        {
        //            stats.opportunities = stats.opportunities + rgs.TeamsMatchesVM.arrayOfResult[array[i]].homeOpportunities;
        //            stats.efficiency = stats.efficiency + rgs.TeamsMatchesVM.arrayOfResult[array[i]].homeEfficiency;
        //            stats.goals_lost = stats.goals_lost + rgs.TeamsMatchesVM.arrayOfResult[array[i]].awayGoals;
        //            stats.goals_scored = stats.goals_scored + rgs.TeamsMatchesVM.arrayOfResult[array[i]].homeGoals;
        //            if (rgs.TeamsMatchesVM.arrayOfResult[array[i]].homeGoals > rgs.TeamsMatchesVM.arrayOfResult[array[i]].awayGoals)
        //            {
        //                points = points + 3;
        //            }
        //            else if (rgs.TeamsMatchesVM.arrayOfResult[array[i]].homeGoals == rgs.TeamsMatchesVM.arrayOfResult[array[i]].awayGoals)
        //            {
        //                points = points + 1;
        //            }
        //        }
        //        else if (rgs.TeamsMatchesVM.arrayOfResult[array[i]].awayPlaceInGroup == placeIngroup)
        //        {
        //            stats.opportunities = stats.opportunities + rgs.TeamsMatchesVM.arrayOfResult[array[i]].awayOpportunities;
        //            stats.efficiency = stats.efficiency + rgs.TeamsMatchesVM.arrayOfResult[array[i]].awayEfficiency;
        //            stats.goals_lost = stats.goals_lost + rgs.TeamsMatchesVM.arrayOfResult[array[i]].homeGoals;
        //            stats.goals_scored = stats.goals_scored + rgs.TeamsMatchesVM.arrayOfResult[array[i]].awayGoals;
        //            if (rgs.TeamsMatchesVM.arrayOfResult[array[i]].homeGoals < rgs.TeamsMatchesVM.arrayOfResult[array[i]].awayGoals)
        //            {
        //                points = points + 3;
        //            }
        //            else if (rgs.TeamsMatchesVM.arrayOfResult[array[i]].homeGoals == rgs.TeamsMatchesVM.arrayOfResult[array[i]].awayGoals)
        //            {
        //                points = points + 1;
        //            }
        //        }
                    
        //    }
        //    stats.points = points;
        //    return stats;
        //}
        private string generateID()
        {
            Guid guid = Guid.NewGuid();
            string str = guid.ToString();
            return str;
        }
    }
}
