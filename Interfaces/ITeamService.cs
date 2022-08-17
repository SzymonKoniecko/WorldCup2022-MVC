using WorldCup2022_MVC.ViewModels;

namespace WorldCup2022_MVC.Interfaces
{
    public interface ITeamService
    {
        public List<TeamVM> GetAllEntries();
    }
}
