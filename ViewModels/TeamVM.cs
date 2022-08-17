using System.Collections.Generic;
using WorldCup2022_MVC.Models;

namespace WorldCup2022_MVC.ViewModels
{
    public class TeamVM
    {
        public int teamId { get; set; }
        public string name { get; set; }
        public string picture { get; set; }
        public string placeInGroup { get; set; }
    }
}
