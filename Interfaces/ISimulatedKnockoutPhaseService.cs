namespace WorldCup2022_MVC.Interfaces
{
    public interface ISimulatedKnockoutPhaseService
    {
        public void SaveSimulatedKnockoutPhase(string Id, string json);
        public string GetAllSimulatedKnockoutPhase(string id);
    }
}
