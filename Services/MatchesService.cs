using WorldCup2022_MVC.Interfaces;
using WorldCup2022_MVC.Models;
using WorldCup2022_MVC.ViewModels;

namespace WorldCup2022_MVC.Services
{
    public class MatchesService : IMatchesService
    {
        private readonly IMatchesRespository _respository;

        public MatchesService(IMatchesRespository respository)
        {
            _respository = respository;
        }
        public void SaveAllMatches(string Id, string json)
        {
            _respository.SaveAllMatches(Id, json);
        }
        
        public string GetAllMatches(string id)
        {
            var data = _respository.GetAllMatches(id);
            string json = data.Json;
            return json;
        }
    }
}

