using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeServeHub.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTbReview2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserCurrentState",
                table: "TbReviews",
                newName: "ReviewCurrentState");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ReviewCurrentState",
                table: "TbReviews",
                newName: "UserCurrentState");
        }
    }
}
