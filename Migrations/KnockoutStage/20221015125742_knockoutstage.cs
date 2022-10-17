using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorldCup2022_MVC.Migrations.KnockoutStage
{
    public partial class knockoutstage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KnocoutStage",
                columns: table => new
                {
                    KnockoutStageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    home = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    away = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KnocoutStage", x => x.KnockoutStageId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KnocoutStage");
        }
    }
}
