using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace portfolium.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueConstraintToStockSymbol : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Stock_Symbol",
                table: "Stock",
                column: "Symbol",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Stock_Symbol",
                table: "Stock");
        }
    }
}
