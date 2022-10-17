using WorldCup2022_MVC.Interfaces;
using WorldCup2022_MVC.Models;
using WorldCup2022_MVC.Respository;
using WorldCup2022_MVC.ViewModels;

namespace WorldCup2022_MVC.Services
{
    public class KnockoutStageService : IKnockoutStageService
    {
        private readonly IKnockoutStageRespository _respository;
        public KnockoutStageService(IKnockoutStageRespository respository)
        {
            _respository = respository;
        }
        public KnockoutStageVM[] GetAllEntries()
        {
            var data = _respository.GetAllEntries();
            return GetAllData(data);
        }
        private KnockoutStageVM[] GetAllData(IQueryable<KnockoutStage> knockoutStages)
        {
            int i = 0;
            KnockoutStageVM[] data = new KnockoutStageVM[knockoutStages.Count()];
            foreach (var item in knockoutStages)
            {
                var match = new KnockoutStageVM()
                {
                    KnockoutStageId = item.KnockoutStageId,
                    home = item.home,
                    away = item.away
                };
                data[i] = match;
                i++;
            }
            return data;
        }
    }
}
