using WorldCup2022_MVC.Contexts;
using WorldCup2022_MVC.Interfaces;
using WorldCup2022_MVC.Models;
using WorldCup2022_MVC.ViewModels;

namespace WorldCup2022_MVC.Respository
{
    public class GroupStageRepository : IGroupStageRepository
    {
        private readonly GroupStageContext _context;

        public GroupStageRepository(GroupStageContext context)
        {
            _context = context;
        }
        public IQueryable<GroupStage>GetAllMatches()
        {
            return _context.GroupStage.OrderBy(gs => gs.groupStageId);
        }
    }
}
