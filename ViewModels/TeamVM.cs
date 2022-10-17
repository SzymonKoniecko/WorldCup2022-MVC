using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WorldCup2022_MVC.Models;

namespace WorldCup2022_MVC.ViewModels
{
    public class TeamVM
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
