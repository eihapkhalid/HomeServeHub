using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeServeHub.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class CreateTbNotification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TbNotifications",
                columns: table => new
                {
                    NotificationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    ServiceProviderID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbNotifications", x => x.NotificationId);
                    table.ForeignKey(
                        name: "FK_TbNotifications_TbServiceProviders_UserID",
                        column: x => x.UserID,
                        principalTable: "TbServiceProviders",
                        principalColumn: "ServiceProviderID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TbNotifications_TbUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "TbUsers",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TbNotifications_UserID",
                table: "TbNotifications",
                column: "UserID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TbNotifications");
        }
    }
}
