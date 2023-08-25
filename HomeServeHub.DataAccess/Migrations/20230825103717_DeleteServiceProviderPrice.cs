using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeServeHub.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class DeleteServiceProviderPrice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ServiceProviderPrice",
                table: "TbServiceProviders");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ServiceProviderPrice",
                table: "TbServiceProviders",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
