using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeServeHub.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class FirstMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TbUsers",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UserCurrentState = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbUsers", x => x.UserID);
                });

            migrationBuilder.CreateTable(
                name: "TbServiceProviders",
                columns: table => new
                {
                    ServiceProviderID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServiceProviderType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ServiceProviderAvailability = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ServiceProviderPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ServiceProviderCurrentState = table.Column<int>(type: "int", nullable: false),
                    ServiceProviderStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ServiceProviderEndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbServiceProviders", x => x.ServiceProviderID);
                    table.ForeignKey(
                        name: "FK_TbServiceProviders_TbUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "TbUsers",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "TbServices",
                columns: table => new
                {
                    ServiceID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServiceName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ServiceDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ServiceCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ServiceCurrentState = table.Column<int>(type: "int", nullable: false),
                    ServiceProviderID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbServices", x => x.ServiceID);
                    table.ForeignKey(
                        name: "FK_TbServices_TbServiceProviders_ServiceProviderID",
                        column: x => x.ServiceProviderID,
                        principalTable: "TbServiceProviders",
                        principalColumn: "ServiceProviderID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TbAppointments",
                columns: table => new
                {
                    AppointmentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppointmentDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AppointmentStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AppointmentCurrentState = table.Column<int>(type: "int", nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    ServiceProviderID = table.Column<int>(type: "int", nullable: false),
                    ServiceID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbAppointments", x => x.AppointmentID);
                    table.ForeignKey(
                        name: "FK_TbAppointments_TbServiceProviders_ServiceProviderID",
                        column: x => x.ServiceProviderID,
                        principalTable: "TbServiceProviders",
                        principalColumn: "ServiceProviderID");
                    table.ForeignKey(
                        name: "FK_TbAppointments_TbServices_ServiceID",
                        column: x => x.ServiceID,
                        principalTable: "TbServices",
                        principalColumn: "ServiceID");
                    table.ForeignKey(
                        name: "FK_TbAppointments_TbUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "TbUsers",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TbPaymentDetails",
                columns: table => new
                {
                    PaymentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PaymentMethod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaymentCurrentState = table.Column<int>(type: "int", nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    AppointmentID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbPaymentDetails", x => x.PaymentID);
                    table.ForeignKey(
                        name: "FK_TbPaymentDetails_TbAppointments_AppointmentID",
                        column: x => x.AppointmentID,
                        principalTable: "TbAppointments",
                        principalColumn: "AppointmentID");
                    table.ForeignKey(
                        name: "FK_TbPaymentDetails_TbUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "TbUsers",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TbAppointments_ServiceID",
                table: "TbAppointments",
                column: "ServiceID");

            migrationBuilder.CreateIndex(
                name: "IX_TbAppointments_ServiceProviderID",
                table: "TbAppointments",
                column: "ServiceProviderID");

            migrationBuilder.CreateIndex(
                name: "IX_TbAppointments_UserID",
                table: "TbAppointments",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_TbPaymentDetails_AppointmentID",
                table: "TbPaymentDetails",
                column: "AppointmentID");

            migrationBuilder.CreateIndex(
                name: "IX_TbPaymentDetails_UserID",
                table: "TbPaymentDetails",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_TbServiceProviders_UserID",
                table: "TbServiceProviders",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_TbServices_ServiceProviderID",
                table: "TbServices",
                column: "ServiceProviderID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TbPaymentDetails");

            migrationBuilder.DropTable(
                name: "TbAppointments");

            migrationBuilder.DropTable(
                name: "TbServices");

            migrationBuilder.DropTable(
                name: "TbServiceProviders");

            migrationBuilder.DropTable(
                name: "TbUsers");
        }
    }
}
