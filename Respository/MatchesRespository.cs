using WorldCup2022_MVC.Contexts;
using WorldCup2022_MVC.Interfaces;
using WorldCup2022_MVC.Models;
using WorldCup2022_MVC.ViewModels;

namespace WorldCup2022_MVC.Respository
{
    public class MatchesRespository : IMatchesRespository
    {
        private readonly MatchesContext _context;

        public MatchesRespository(MatchesContext context)
        {
            _context = context;
        }

        public void SaveAllMatches(string Id, string json)
        {
            var allmatches = new Matches()
            {
                Id = Id,
                Json = json,
                CreatedDate = DateTime.Now,
            };
            _context.Add(allmatches);
            _context.SaveChanges();
        }
        public Matches GetAllMatches(string id)
        {
            return _context.Matches.FirstOrDefault(m => m.Id == id);
        }
    }
}
