using Microsoft.EntityFrameworkCore.Migrations;

namespace ListsAPI.Migrations
{
    public partial class MakePositionNullableOnTasks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Position",
                table: "Tasks",
                nullable: true,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Position",
                table: "Tasks",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}