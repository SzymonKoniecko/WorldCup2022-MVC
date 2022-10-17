using System.ComponentModel.DataAnnotations;

namespace WorldCup2022_MVC.ViewModels
{
    public class SemiFinalTeamsVM
    {
        public int teamId { get; set; }
        [Display(Name = "Country")]
        public string name { get; set; }
        [Display(Name = "Flag")]
        public string picture { get; set; }
        [Display(Name = "Position in group")]
        public string placeInGroup { get; set; }
        public int placeInGlobalRanking { get; set; }
    }
}
