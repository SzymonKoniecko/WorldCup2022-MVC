using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WorldCup2022_MVC.Interfaces;
using WorldCup2022_MVC.ViewModels;

namespace WorldCup2022_MVC.Controllers
{
    public class PlayGroupController : Controller
    {
        //private readonly IGroupStageService _groupStageservice;
        private readonly ITeamService _teamservice;
        private readonly IGroupStageService _groupstageservice;
        public PlayGroupController(ITeamService teamservice, IGroupStageService groupstageservice)
        {
            _teamservice = teamservice;
            _groupstageservice = groupstageservice;
        }

        // GET: PlayGroupController
        public ActionResult PlayGroup([FromServices] ITeamService teamservice, [FromServices] IGroupStageService stageservice)
        {
            var alldata = new TeamsMatchesVM
            {
                listOfTeams = _teamservice.GetAllEntries(),
                listOfMatches = _groupstageservice.GetAllMatches()
            };
            return View(alldata);
        }

        // GET: PlayGroupController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PlayGroupController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PlayGroupController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PlayGroupController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PlayGroupController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
