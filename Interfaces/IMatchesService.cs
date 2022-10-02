using WorldCup2022_MVC.ViewModels;

namespace WorldCup2022_MVC.Interfaces
{
    public interface IMatchesService
    {
        public void SaveAllMatches(string Id, string json);
    }
}
