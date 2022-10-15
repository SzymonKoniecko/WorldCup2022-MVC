using WorldCup2022_MVC.ViewModels;

namespace WorldCup2022_MVC.Interfaces
{
    public interface IKnockoutStageService
    {
        public KnockoutStageVM[] GetAllEntries();
    }
}
