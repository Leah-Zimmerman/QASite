using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QASite.Data.Migrations
{
    public partial class AddLikedQuestions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuestionUsers_Questions_QuestionId",
                table: "QuestionUsers");

            migrationBuilder.AddColumn<int>(
                name: "QuestionUserQuestionId",
                table: "Questions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "QuestionUserUserId",
                table: "Questions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Questions_QuestionUserQuestionId_QuestionUserUserId",
                table: "Questions",
                columns: new[] { "QuestionUserQuestionId", "QuestionUserUserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_QuestionUsers_QuestionUserQuestionId_QuestionUserUserId",
                table: "Questions",
                columns: new[] { "QuestionUserQuestionId", "QuestionUserUserId" },
                principalTable: "QuestionUsers",
                principalColumns: new[] { "QuestionId", "UserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionUsers_Questions_QuestionId",
                table: "QuestionUsers",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_QuestionUsers_QuestionUserQuestionId_QuestionUserUserId",
                table: "Questions");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionUsers_Questions_QuestionId",
                table: "QuestionUsers");

            migrationBuilder.DropIndex(
                name: "IX_Questions_QuestionUserQuestionId_QuestionUserUserId",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "QuestionUserQuestionId",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "QuestionUserUserId",
                table: "Questions");

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionUsers_Questions_QuestionId",
                table: "QuestionUsers",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
