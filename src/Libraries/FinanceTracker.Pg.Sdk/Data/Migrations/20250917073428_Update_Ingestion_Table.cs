using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceTracker.Pg.Sdk.Data.Migrations
{
    /// <inheritdoc />
    public partial class Update_Ingestion_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Extension",
                table: "Ingestions",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Extension",
                table: "Ingestions");
        }
    }
}
