using WorldCup2022_MVC.Models;
namespace WorldCup2022_MVC.Interfaces
{
    public interface ITeamRepository
    {
        public IQueryable<Team> GetAllEntries();
    }
}
