using WorldCup2022_MVC.Interfaces;

namespace WorldCup2022_MVC.Services
{
    public class SimulatedKnockoutPhaseService : ISimulatedKnockoutPhaseService
    {
        private readonly ISimulatedKnockoutPhaseRespository _respository;
        public SimulatedKnockoutPhaseService(ISimulatedKnockoutPhaseRespository respository)
        {
            _respository = respository;
        }
        public void SaveSimulatedKnockoutPhase(string Id, string json)
        {
            _respository.SaveSimulatedKnockoutPhase(Id, json);
        }
        public string GetAllSimulatedKnockoutPhase(string id)
        {
            var data = _respository.GetAllSimulatedKnockoutPhase(id);
            string json = "null";
            if (data != null)
            {
                json = data.Json;
            }
            return json;
        }
    }
}
