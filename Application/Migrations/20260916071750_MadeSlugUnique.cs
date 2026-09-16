using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Application.Migrations
{
    /// <inheritdoc />
    public partial class MadeSlugUnique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "ak_coffees_slug",
                table: "coffees");

            migrationBuilder.CreateIndex(
                name: "ix_coffees_slug",
                table: "coffees",
                column: "slug",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_coffees_slug",
                table: "coffees");

            migrationBuilder.AddUniqueConstraint(
                name: "ak_coffees_slug",
                table: "coffees",
                column: "slug");
        }
    }
}
