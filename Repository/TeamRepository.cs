using WorldCup2022_MVC.Contexts;
using WorldCup2022_MVC.Interfaces;
using WorldCup2022_MVC.Models;
using Microsoft.EntityFrameworkCore;

namespace WorldCup2022_MVC.Respository
{
    public class TeamRepository : ITeamRepository
    {
        private readonly TeamContext _context;

        public TeamRepository(TeamContext context)
        {
            _context = context;
        }
        public bool Add(Team team)
        {
            _context.Add(team);
            try
            {
                return true;
            }
            catch (Exception)
            {

                throw new NotSupportedException
                {

                };
            }
        }
        public async Task<Team> GetByIdAsync(int id)
        {
            return await _context.Team.Include(t => t.teamId).FirstOrDefaultAsync(f => f.teamId == id);
        }
        public IQueryable<Team> GetAllEntries()
        {
            return _context.Team.OrderByDescending(item => item.teamId);
        }
    }
}
