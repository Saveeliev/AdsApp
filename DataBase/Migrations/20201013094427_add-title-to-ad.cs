using Microsoft.EntityFrameworkCore.Migrations;

namespace AdsApp.Migrations
{
    public partial class addtitletoad : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Ads",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "Ads");
        }
    }
}
