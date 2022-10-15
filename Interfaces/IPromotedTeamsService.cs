namespace WorldCup2022_MVC.Interfaces
{
    public interface IPromotedTeamsService
    {
        public void SavePromotedTeams(string Id, string json);
        public string GetAllPromotedTeams(string id);
    }
}
