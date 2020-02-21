using Microsoft.EntityFrameworkCore.Migrations;

namespace MiddleMan.Data.Migrations
{
    public partial class OfferUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OfferUserRates",
                columns: table => new
                {
                    OfferId = table.Column<string>(nullable: false),
                    UserId = table.Column<string>(nullable: false),
                    Rate = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfferUserRates", x => new { x.OfferId, x.UserId });
                    table.ForeignKey(
                        name: "FK_OfferUserRates_Offers_OfferId",
                        column: x => x.OfferId,
                        principalTable: "Offers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OfferUserRates_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OfferUserRates_UserId",
                table: "OfferUserRates",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OfferUserRates");
        }
    }
}
