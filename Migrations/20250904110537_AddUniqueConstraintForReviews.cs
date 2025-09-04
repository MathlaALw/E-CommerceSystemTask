using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_CommerceSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueConstraintForReviews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Reviews_PID",
                table: "Reviews");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_PID_UID",
                table: "Reviews",
                columns: new[] { "PID", "UID" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Reviews_PID_UID",
                table: "Reviews");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_PID",
                table: "Reviews",
                column: "PID");
        }
    }
}
