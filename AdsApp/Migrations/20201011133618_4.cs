using Microsoft.EntityFrameworkCore.Migrations;

namespace AdsApp.Migrations
{
    public partial class _4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Number",
                table: "Ads");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Number",
                table: "Ads",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
