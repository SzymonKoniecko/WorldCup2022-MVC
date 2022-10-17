using WorldCup2022_MVC.Contexts;
using WorldCup2022_MVC.Interfaces;
using WorldCup2022_MVC.Models;

namespace WorldCup2022_MVC.Respository
{
    public class KnockoutStageRespository : IKnockoutStageRespository
    {
        private readonly KnockoutStageContext _context;
        public KnockoutStageRespository(KnockoutStageContext context)
        {
            _context = context;
        }
        public IQueryable<KnockoutStage> GetAllEntries()
        {
            return _context.KnocoutStage.OrderBy(k => k.KnockoutStageId);
        }
    }
}
