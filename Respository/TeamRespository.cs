using WorldCup2022_MVC.Contexts;
using WorldCup2022_MVC.Interfaces;
using WorldCup2022_MVC.Models;

namespace WorldCup2022_MVC.Respository
{
    public class TeamRespository : ITeamRespository
    {
        private readonly TeamContext _context;

        public TeamRespository(TeamContext context)
        {
            _context = context;
        }
        public IQueryable<Team> GetAllEntries()
        {
            return _context.Team.OrderByDescending(item => item.teamId);
        }
    }
}
