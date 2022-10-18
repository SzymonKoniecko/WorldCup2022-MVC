using WorldCup2022_MVC.Contexts;
using WorldCup2022_MVC.Interfaces;
using WorldCup2022_MVC.Models;

namespace WorldCup2022_MVC.Respository
{
    public class SimulatedKnockoutPhaseRespository : ISimulatedKnockoutPhaseRespository
    {
        private readonly SimulatedKnockoutPhaseContext _context;
        public SimulatedKnockoutPhaseRespository(SimulatedKnockoutPhaseContext context)
        {
            _context = context;
        }
        public void SaveSimulatedKnockoutPhase(string Id, string json)
        {

            var simulatedKnockoutPhase = GetAllSimulatedKnockoutPhase(Id);
            if (simulatedKnockoutPhase == null)
            {
                var allmatches = new SimulatedKnockoutPhase()
                {
                    Id = Id,
                    Json = json,
                    CreatedDate = DateTime.Now,
                };
                _context.Add(allmatches);
                _context.SaveChanges();
            }
        }
        public SimulatedKnockoutPhase GetAllSimulatedKnockoutPhase(string id)
        {
            return _context.SimulatedKnockoutPhase.FirstOrDefault(m => m.Id == id);
        }
    }
}
