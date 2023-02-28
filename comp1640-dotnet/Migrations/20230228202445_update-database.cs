using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace comp1640_dotnet.Migrations
{
    public partial class updatedatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documents_Ideas_IdeaId",
                table: "Documents");

            migrationBuilder.DropForeignKey(
                name: "FK_Ideas_AcademicYears_AcademicYearId",
                table: "Ideas");

            migrationBuilder.DropForeignKey(
                name: "FK_Ideas_Categories_CategoryId",
                table: "Ideas");

            migrationBuilder.DropForeignKey(
                name: "FK_Ideas_Departments_DepartmentId",
                table: "Ideas");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Ideas_IdeaId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Reactions_Ideas_IdeaId",
                table: "Reactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Departments_DepartmentId",
                table: "Users");

            migrationBuilder.AlterColumn<string>(
                name: "DepartmentId",
                table: "Users",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "IdeaId",
                table: "Reactions",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "IdeaId",
                table: "Notifications",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsRead",
                table: "Notifications",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "DepartmentId",
                table: "Ideas",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CategoryId",
                table: "Ideas",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AcademicYearId",
                table: "Ideas",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ViewCount",
                table: "Ideas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "IdeaId",
                table: "Documents",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_Ideas_IdeaId",
                table: "Documents",
                column: "IdeaId",
                principalTable: "Ideas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ideas_AcademicYears_AcademicYearId",
                table: "Ideas",
                column: "AcademicYearId",
                principalTable: "AcademicYears",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ideas_Categories_CategoryId",
                table: "Ideas",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ideas_Departments_DepartmentId",
                table: "Ideas",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Ideas_IdeaId",
                table: "Notifications",
                column: "IdeaId",
                principalTable: "Ideas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reactions_Ideas_IdeaId",
                table: "Reactions",
                column: "IdeaId",
                principalTable: "Ideas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Departments_DepartmentId",
                table: "Users",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documents_Ideas_IdeaId",
                table: "Documents");

            migrationBuilder.DropForeignKey(
                name: "FK_Ideas_AcademicYears_AcademicYearId",
                table: "Ideas");

            migrationBuilder.DropForeignKey(
                name: "FK_Ideas_Categories_CategoryId",
                table: "Ideas");

            migrationBuilder.DropForeignKey(
                name: "FK_Ideas_Departments_DepartmentId",
                table: "Ideas");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Ideas_IdeaId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Reactions_Ideas_IdeaId",
                table: "Reactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Departments_DepartmentId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsRead",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "ViewCount",
                table: "Ideas");

            migrationBuilder.AlterColumn<string>(
                name: "DepartmentId",
                table: "Users",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "IdeaId",
                table: "Reactions",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "IdeaId",
                table: "Notifications",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "DepartmentId",
                table: "Ideas",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "CategoryId",
                table: "Ideas",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "AcademicYearId",
                table: "Ideas",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "IdeaId",
                table: "Documents",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_Ideas_IdeaId",
                table: "Documents",
                column: "IdeaId",
                principalTable: "Ideas",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Ideas_AcademicYears_AcademicYearId",
                table: "Ideas",
                column: "AcademicYearId",
                principalTable: "AcademicYears",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Ideas_Categories_CategoryId",
                table: "Ideas",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Ideas_Departments_DepartmentId",
                table: "Ideas",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Ideas_IdeaId",
                table: "Notifications",
                column: "IdeaId",
                principalTable: "Ideas",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reactions_Ideas_IdeaId",
                table: "Reactions",
                column: "IdeaId",
                principalTable: "Ideas",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Departments_DepartmentId",
                table: "Users",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id");
        }
    }
}
