namespace WorldCup2022_MVC.ViewModels
{
    public class MatchKnockoutStageVM
    {
        public int homeGoals { get; set; }
        public int awayGoals { get; set; }

        public bool isDraw { get; set; }
        public int homeGoals_draw { get; set; }
        public int awayGoals_draw { get; set; }
        public string homePlaceInGroup { get; set; }
        public string awayPlaceInGroup { get; set; }
    }
}
