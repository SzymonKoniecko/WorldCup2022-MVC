using WorldCup2022_MVC.Models;

namespace WorldCup2022_MVC.Interfaces
{
    public interface IKnockoutStageRespository
    {
        public IQueryable<KnockoutStage> GetAllEntries();
    }
}
