using WorldCup2022_MVC.Contexts;
using WorldCup2022_MVC.Interfaces;
using WorldCup2022_MVC.Models;

namespace WorldCup2022_MVC.Respository
{
    public class PromotedTeamsRespository : IPromotedTeamsRespository
    {
        private readonly PromotedTeamsContext _context;
        public PromotedTeamsRespository(PromotedTeamsContext context)
        {
            _context = context;
        }
        public void SavePromotedTeams(string Id, string json)
        {
            var allmatches = new PromotedTeams()
            {
                Id = Id,
                Json = json,
                CreatedDate = DateTime.Now,
            };
            _context.Add(allmatches);
            _context.SaveChanges();
        }
        public PromotedTeams GetAllPromotedTeams(string id)
        {
            return _context.PromotedTeams.FirstOrDefault(m => m.Id == id);
        }
    }
}
