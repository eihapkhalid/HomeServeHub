using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeServeHub.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class CreateTbReview : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TbReviews",
                columns: table => new
                {
                    ReviewID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    ServiceProviderID = table.Column<int>(type: "int", nullable: false),
                    TbServiceProviderServiceProviderID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbReviews", x => x.ReviewID);
                    table.ForeignKey(
                        name: "FK_TbReviews_TbServiceProviders_ServiceProviderID",
                        column: x => x.ServiceProviderID,
                        principalTable: "TbServiceProviders",
                        principalColumn: "ServiceProviderID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TbReviews_TbServiceProviders_TbServiceProviderServiceProviderID",
                        column: x => x.TbServiceProviderServiceProviderID,
                        principalTable: "TbServiceProviders",
                        principalColumn: "ServiceProviderID");
                    table.ForeignKey(
                        name: "FK_TbReviews_TbUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "TbUsers",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TbReviews_ServiceProviderID",
                table: "TbReviews",
                column: "ServiceProviderID");

            migrationBuilder.CreateIndex(
                name: "IX_TbReviews_TbServiceProviderServiceProviderID",
                table: "TbReviews",
                column: "TbServiceProviderServiceProviderID");

            migrationBuilder.CreateIndex(
                name: "IX_TbReviews_UserID",
                table: "TbReviews",
                column: "UserID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TbReviews");
        }
    }
}
