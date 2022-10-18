using WorldCup2022_MVC.Interfaces;
using WorldCup2022_MVC.Models;
using WorldCup2022_MVC.ViewModels;
using WorldCup2022_MVC.Models;
using WorldCup2022_MVC.Contexts;
namespace WorldCup2022_MVC.Services
{
    public class TeamService : ITeamService
    {
        private readonly ITeamRespository _TeamRepository;
        public TeamService(ITeamRespository teamRepository)
        {
            _TeamRepository = teamRepository;
        }
        private List<TeamVM> DataToList(IQueryable<Team> Data)
        {
            List<TeamVM> list = new List<TeamVM>();
            foreach (var item in Data)
            {
                var elementsVM = new TeamVM()
                {
                    teamId = item.teamId,
                    name = item.name,
                    picture = item.picture,
                    placeInGroup = item.placeInGroup,
                    placeInGlobalRanking = item.placeInGlobalRanking
                };
                list.Add(elementsVM);
            }
            return list;
        }
        public List<TeamVM> GetAllEntries()
        {
            var EntriesList = _TeamRepository.GetAllEntries();
            return DataToList(EntriesList);
        }
    }
}
