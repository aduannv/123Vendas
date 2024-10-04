using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _123Vendas.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SaleItemIsCanceledEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCanceled",
                table: "saleitems",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCanceled",
                table: "saleitems");
        }
    }
}
