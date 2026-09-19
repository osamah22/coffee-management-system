using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Application.Migrations
{
    /// <inheritdoc />
    public partial class AddCoffeeOptions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "price_in_cents",
                table: "coffees");

            migrationBuilder.DropColumn(
                name: "type",
                table: "coffees");

            migrationBuilder.CreateTable(
                name: "coffee_options",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    coffee_id = table.Column<Guid>(type: "uuid", nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false),
                    size = table.Column<int>(type: "integer", nullable: false),
                    price_in_cents = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_coffee_options", x => x.id);
                    table.ForeignKey(
                        name: "fk_coffee_options_coffees_coffee_id",
                        column: x => x.coffee_id,
                        principalTable: "coffees",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_coffee_options_coffee_id_type_size",
                table: "coffee_options",
                columns: new[] { "coffee_id", "type", "size" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "coffee_options");

            migrationBuilder.AddColumn<long>(
                name: "price_in_cents",
                table: "coffees",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "type",
                table: "coffees",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
