using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeServeHub.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddTbUserType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserType",
                table: "TbUsers");

            migrationBuilder.CreateTable(
                name: "TbUserTypes",
                columns: table => new
                {
                    UserTypeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserTypeName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UserTypeCurrentState = table.Column<int>(type: "int", nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbUserTypes", x => x.UserTypeID);
                    table.ForeignKey(
                        name: "FK_TbUserTypes_TbUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "TbUsers",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TbUserTypes_UserID",
                table: "TbUserTypes",
                column: "UserID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TbUserTypes");

            migrationBuilder.AddColumn<string>(
                name: "UserType",
                table: "TbUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
