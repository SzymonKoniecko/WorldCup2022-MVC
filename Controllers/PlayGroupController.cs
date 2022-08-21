using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WorldCup2022_MVC.Interfaces;
using WorldCup2022_MVC.ViewModels;
using WorldCup2022_MVC.Session;
namespace WorldCup2022_MVC.Controllers
{
    public class PlayGroupController : Controller
    {
        //private readonly IGroupStageService _groupStageservice;
        private readonly ITeamService _teamservice;
        private readonly IGroupStageService _groupstageservice;
        private List<MatchVM> matches = new List<MatchVM>();
        public PlayGroupController(ITeamService teamservice, IGroupStageService groupstageservice)
        {
            _teamservice = teamservice;
            _groupstageservice = groupstageservice;
        }

        // GET: PlayGroupController
        public ActionResult PlayGroup([FromServices] ITeamService teamservice, [FromServices] IGroupStageService stageservice)
        {
            //HttpContext.Session.SetSession("Matches", Play());
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
            return View(alldata);
        }
        [HttpPost]
        public MatchVM Play()
        {
            Random random = new Random();
            int hOppor = random.Next(0, 10);
            int aOppor = random.Next(0, 10);
            float hEff = (float)(random.Next(0, 100) * 0.01);
            float aEff = (float)(random.Next(0, 100) * 0.01);

            var match = new MatchVM()
            {
                homeOpportunities = hOppor,
                awayOpportunities = aOppor,
                homeEfficiency = hEff,
                awayEfficiency = aEff,
                homeGoals = (int)Math.Floor(hOppor * hEff),
                awayGoals = (int)Math.Floor(aOppor * aEff),
                isHomeWinner = false
            };
            return match;
        }
    }
}
