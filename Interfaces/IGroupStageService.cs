using WorldCup2022_MVC.ViewModels;

namespace WorldCup2022_MVC.Interfaces
{
    public interface IGroupStageService
    {
        public List<GroupStageVM> GetAllMatches();
    }
}
