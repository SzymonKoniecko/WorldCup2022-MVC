namespace WorldCup2022_MVC.ViewModels
{
    public class SimulatedKnockoutPhaseToJsonVM
    {
        public MatchKnockoutStageVM[] matchesKSVM { get; set; }
        public WinnerVM[] winnerVMs { get; set; }
        public QuarterFinalTeamsVM[] quarterFinalTeamsVMs { get; set; }
        public SemiFinalTeamsVM[] semiFinalTeamsVMs { get; set; }
        public ThirdPlaceTeamsVM[] thirdPlaceTeamsVMs { get; set; }
        public FinalTeamsVM[] finalTeamsVMs { get; set; }
        public TeamVM[] teamVMs { get; set; }
    }
}
