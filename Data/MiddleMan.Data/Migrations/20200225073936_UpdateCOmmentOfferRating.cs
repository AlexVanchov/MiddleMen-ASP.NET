using Microsoft.EntityFrameworkCore.Migrations;

namespace MiddleMan.Data.Migrations
{
    public partial class UpdateCOmmentOfferRating : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RatingGiven",
                table: "Comments");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RatingGiven",
                table: "Comments",
                type: "int",
                nullable: true);
        }
    }
}
