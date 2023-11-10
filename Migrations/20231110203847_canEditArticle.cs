using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogASP.Migrations
{
    /// <inheritdoc />
    public partial class canEditArticle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Articles",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Articles");
        }
    }
}
