using WorldCup2022_MVC.Interfaces;

namespace WorldCup2022_MVC.Services
{
    public class PromotedTeamsService : IPromotedTeamsService
    {
        private readonly IPromotedTeamsRespository _respository;
        public PromotedTeamsService(IPromotedTeamsRespository respository)
        {
            _respository = respository;
        }
        public void SavePromotedTeams(string Id, string json)
        {
            _respository.SavePromotedTeams(Id, json);
        }
        public string GetAllPromotedTeams(string id)
        {
            var data = _respository.GetAllPromotedTeams(id);
            string json = data.Json;
            return json;
        }
    }
}
