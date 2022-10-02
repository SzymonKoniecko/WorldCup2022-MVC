using WorldCup2022_MVC.Models;

namespace WorldCup2022_MVC.Interfaces
{
    public interface IMatchesRespository
    {
        public void SaveAllMatches(string Id, string Json);
    }
}
