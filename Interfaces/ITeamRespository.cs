using WorldCup2022_MVC.Models;
namespace WorldCup2022_MVC.Interfaces
{
    public interface ITeamRespository
    {
        public IQueryable<Team> GetAllEntries();
    }
}
