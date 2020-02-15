using Microsoft.EntityFrameworkCore.Migrations;

namespace MiddleMan.Data.Migrations
{
    public partial class OfferValidationsAndIsApDecFeatured : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Offers",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatorId",
                table: "Offers",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CategoryId",
                table: "Offers",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "Offers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeclined",
                table: "Offers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsFeatured",
                table: "Offers",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "Offers");

            migrationBuilder.DropColumn(
                name: "IsDeclined",
                table: "Offers");

            migrationBuilder.DropColumn(
                name: "IsFeatured",
                table: "Offers");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Offers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "CreatorId",
                table: "Offers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "CategoryId",
                table: "Offers",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
