using Microsoft.EntityFrameworkCore.Migrations;

namespace MiddleMan.Data.Migrations
{
    public partial class userDeletedPropOnOffer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Rate",
                table: "OfferUserRates",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<bool>(
                name: "IsRemovedByUser",
                table: "Offers",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRemovedByUser",
                table: "Offers");

            migrationBuilder.AlterColumn<int>(
                name: "Rate",
                table: "OfferUserRates",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
