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

        // GET: PlayGroupController
        public ActionResult PlayGroup([FromServices] ITeamService teamservice, [FromServices] IGroupStageService stageservice, [FromServices] IMatchesService matchesService)
        {

            var id = generateID();
            MatchVM[] result = new MatchVM[49];
            for (int i = 0; i < 49; i++)
            {
                result[i] = Play();
            }
            var alldata = new TeamsMatchesVM
            {
                listOfTeams = _teamservice.GetAllEntries(),
                listOfMatches = _groupstageservice.GetAllMatches(),
                arrayOfResult = result
            };
            var json = JsonConvert.SerializeObject(alldata);
            _matchesService.SaveAllMatches(id, json);
            return View(alldata);
        }
        public ActionResult PlayGroupById(string? id, [FromServices] IMatchesService matchesService)
        {
            string data = matchesService.GetAllMatches(id);
            TeamsMatchesVM matches = JsonConvert.DeserializeObject<TeamsMatchesVM>(data);
            return View(matches);
        }
        [HttpPost]
        public MatchVM Play()
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
                isHomeWinner = isHomeWinner
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
