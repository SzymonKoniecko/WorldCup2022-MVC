using WorldCup2022_MVC.Models;

namespace WorldCup2022_MVC.Interfaces
{
    public interface IPromotedTeamsRespository
    {
        public void SavePromotedTeams(string Id, string Json);
        public PromotedTeams GetAllPromotedTeams(string id);
    }
}
