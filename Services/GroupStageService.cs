using WorldCup2022_MVC.Interfaces;
using WorldCup2022_MVC.Models;
using WorldCup2022_MVC.Respository;
using WorldCup2022_MVC.ViewModels;

namespace WorldCup2022_MVC.Services
{
    public class GroupStageService : IGroupStageService
    {
        private readonly IGroupStageRespository _respository;

        public GroupStageService(IGroupStageRespository respository)
        {
            _respository = respository;
        }
        private List<GroupStageVM> GetAllData(IQueryable<GroupStage> matches)
        {
            List<GroupStageVM> data = new List<GroupStageVM>();
            foreach (var item in matches)
            {
                var elements = new GroupStageVM()
                {
                    groupStageId = item.groupStageId,
                    home = item.home,
                    away = item.away
                };
                data.Add(elements);
            }
            return data;
        }
        public List<GroupStageVM> GetAllMatches()
        {
            var matches = _respository.GetAllMatches();
            return GetAllData(matches);
        }
    }
}
