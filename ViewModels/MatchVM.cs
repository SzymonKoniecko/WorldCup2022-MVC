﻿namespace WorldCup2022_MVC.ViewModels
{
    public class MatchVM
    {
        public int homeOpportunities { get; set; }
        public int awayOpportunities { get; set; }

        public float homeEfficiency { get; set; }
        public float awayEfficiency { get; set; }

        public int homeGoals { get; set; }
        public int awayGoals { get; set; }

        public bool isHomeWinner { get; set; }
        public string homePlaceInGroup { get; set; }
        public string awayPlaceInGroup { get; set; }
        public int placeInGlobalRanking_home { get; set; }
        public int placeInGlobalRanking_away { get; set; }
    }
}
