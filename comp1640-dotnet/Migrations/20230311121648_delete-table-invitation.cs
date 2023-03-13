using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace comp1640_dotnet.Migrations
{
    public partial class deletetableinvitation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invitations_Users_InviteUserId",
                table: "Invitations");

            migrationBuilder.DropIndex(
                name: "IX_Invitations_InviteUserId",
                table: "Invitations");

            migrationBuilder.DropColumn(
                name: "InviteUserId",
                table: "Invitations");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InviteUserId",
                table: "Invitations",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Invitations_InviteUserId",
                table: "Invitations",
                column: "InviteUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Invitations_Users_InviteUserId",
                table: "Invitations",
                column: "InviteUserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
