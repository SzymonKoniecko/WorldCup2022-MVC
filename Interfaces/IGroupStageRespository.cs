using WorldCup2022_MVC.Models;

namespace WorldCup2022_MVC.Interfaces
{
    public interface IGroupStageRespository
    {
        public IQueryable<GroupStage> GetAllMatches();
    }
}
