using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SqliteRepository.Migrations
{
    public partial class TextWrapping : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "TextWrapping",
                table: "Memos",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TextWrapping",
                table: "Memos");
        }
    }
}
