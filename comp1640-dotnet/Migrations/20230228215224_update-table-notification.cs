using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace comp1640_dotnet.Migrations
{
    public partial class updatetablenotification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Ideas_IdeaId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Ideas_IdeaId",
                table: "Notifications");

            migrationBuilder.AlterColumn<string>(
                name: "IdeaId",
                table: "Notifications",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "IdeaId",
                table: "Comments",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Ideas_IdeaId",
                table: "Comments",
                column: "IdeaId",
                principalTable: "Ideas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Ideas_IdeaId",
                table: "Notifications",
                column: "IdeaId",
                principalTable: "Ideas",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Ideas_IdeaId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Ideas_IdeaId",
                table: "Notifications");

            migrationBuilder.AlterColumn<string>(
                name: "IdeaId",
                table: "Notifications",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "IdeaId",
                table: "Comments",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Ideas_IdeaId",
                table: "Comments",
                column: "IdeaId",
                principalTable: "Ideas",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Ideas_IdeaId",
                table: "Notifications",
                column: "IdeaId",
                principalTable: "Ideas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
