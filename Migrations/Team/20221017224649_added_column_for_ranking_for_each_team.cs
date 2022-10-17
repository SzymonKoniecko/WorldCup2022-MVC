using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorldCup2022_MVC.Migrations.Team
{
    public partial class added_column_for_ranking_for_each_team : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "placeInGlobalRanking",
                table: "Team",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "placeInGlobalRanking",
                table: "Team");
        }
    }
}
