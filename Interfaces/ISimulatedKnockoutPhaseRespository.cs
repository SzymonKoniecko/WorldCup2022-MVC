using WorldCup2022_MVC.Models;

namespace WorldCup2022_MVC.Interfaces
{
    public interface ISimulatedKnockoutPhaseRespository
    {
        public void SaveSimulatedKnockoutPhase(string Id, string json);
        public SimulatedKnockoutPhase GetAllSimulatedKnockoutPhase(string id);
    }
}
