using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WorldCup2022_MVC.Interfaces;

namespace WorldCup2022_MVC.Controllers
{
    public class PlayGroupController : Controller
    {
        private readonly IGroupStageService _groupStageservice;
        private readonly ITeamService _teamservice;
        public PlayGroupController(IGroupStageService groupStageservice, ITeamService teamservice)
        {
            _groupStageservice = groupStageservice;
            _teamservice = teamservice;
        }

        // GET: PlayGroupController
        public ActionResult PlayGroup()
        {
            return View();
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
