using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogASP.Migrations
{
    /// <inheritdoc />
    public partial class articlesupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Articles");

            migrationBuilder.AddColumn<bool>(
                name: "isDisabled",
                table: "Articles",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isDisabled",
                table: "Articles");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Articles",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
