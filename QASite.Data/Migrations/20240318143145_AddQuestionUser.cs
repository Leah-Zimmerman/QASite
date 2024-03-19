using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QASite.Data.Migrations
{
    public partial class AddQuestionUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuestionUser_Questions_QuestionId",
                table: "QuestionUser");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionUser_Users_UserId",
                table: "QuestionUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_QuestionUser",
                table: "QuestionUser");

            migrationBuilder.RenameTable(
                name: "QuestionUser",
                newName: "QuestionUsers");

            migrationBuilder.RenameIndex(
                name: "IX_QuestionUser_UserId",
                table: "QuestionUsers",
                newName: "IX_QuestionUsers_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_QuestionUsers",
                table: "QuestionUsers",
                columns: new[] { "QuestionId", "UserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionUsers_Questions_QuestionId",
                table: "QuestionUsers",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionUsers_Users_UserId",
                table: "QuestionUsers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuestionUsers_Questions_QuestionId",
                table: "QuestionUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionUsers_Users_UserId",
                table: "QuestionUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_QuestionUsers",
                table: "QuestionUsers");

            migrationBuilder.RenameTable(
                name: "QuestionUsers",
                newName: "QuestionUser");

            migrationBuilder.RenameIndex(
                name: "IX_QuestionUsers_UserId",
                table: "QuestionUser",
                newName: "IX_QuestionUser_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_QuestionUser",
                table: "QuestionUser",
                columns: new[] { "QuestionId", "UserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionUser_Questions_QuestionId",
                table: "QuestionUser",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionUser_Users_UserId",
                table: "QuestionUser",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
