using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorldCup2022_MVC.Migrations.GroupStage
{
    public partial class groupstage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GroupStage",
                columns: table => new
                {
                    groupStageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    home = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    away = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupStage", x => x.groupStageId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GroupStage");
        }
    }
}
