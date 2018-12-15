using Microsoft.EntityFrameworkCore.Migrations;

namespace ListsAPI.Migrations
{
    public partial class AddImageSourceToListsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageSource",
                table: "Lists",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageSource",
                table: "Lists");
        }
    }
}