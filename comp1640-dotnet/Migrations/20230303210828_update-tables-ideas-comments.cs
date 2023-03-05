using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace comp1640_dotnet.Migrations
{
    public partial class updatetablesideascomments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsLatest",
                table: "Ideas",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsLatest",
                table: "Comments",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsLatest",
                table: "Ideas");

            migrationBuilder.DropColumn(
                name: "IsLatest",
                table: "Comments");
        }
    }
}
