using Microsoft.EntityFrameworkCore.Migrations;

namespace ListsAPI.Migrations
{
    public partial class AddBackgroundImageFileNameToListsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageSource",
                table: "Lists");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Lists",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Lists",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BackgroundImageFileName",
                table: "Lists",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BackgroundImageFilePath",
                table: "Lists",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BackgroundImageFileName",
                table: "Lists");

            migrationBuilder.DropColumn(
                name: "BackgroundImageFilePath",
                table: "Lists");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Lists",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Lists",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<string>(
                name: "ImageSource",
                table: "Lists",
                nullable: true);
        }
    }
}