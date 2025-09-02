using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace KvizHubBack.Migrations
{
    public partial class InitialCreateTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Users table
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Username = table.Column<string>(maxLength: 2000, nullable: false),
                    PasswordHash = table.Column<string>(maxLength: 2000, nullable: false),
                    Email = table.Column<string>(maxLength: 2000, nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    AvatarUrl = table.Column<string>(maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            // Quizzes table
            migrationBuilder.CreateTable(
                name: "Quizzes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Title = table.Column<string>(maxLength: 2000, nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Difficulty = table.Column<int>(nullable: false),
                    TimeLimit = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quizzes", x => x.Id);
                });

            // Questions table
            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Text = table.Column<string>(maxLength: 2000, nullable: false),
                    QuestionType = table.Column<int>(nullable: false),
                    QuizId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Questions_Quizzes",
                        column: x => x.QuizId,
                        principalTable: "Quizzes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            // Answers table
            migrationBuilder.CreateTable(
                name: "Answers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Text = table.Column<string>(maxLength: 2000, nullable: false),
                    IsCorrect = table.Column<int>(type: "NUMBER", nullable: false),
                    QuestionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Answers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Answers_Questions",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            // QuizAttempts table
            migrationBuilder.CreateTable(
                name: "QuizAttempts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    QuizId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    Score = table.Column<int>(nullable: false),
                    Percentage = table.Column<float>(nullable: false),
                    StartedAt = table.Column<DateTime>(nullable: false),
                    FinishedAt = table.Column<DateTime>(nullable: true),
                    TimeSpent = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizAttempts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuizAttempts_Quizzes",
                        column: x => x.QuizId,
                        principalTable: "Quizzes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuizAttempts_Users",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            // UserAnswers table
            migrationBuilder.CreateTable(
    name: "UserAnswers",
    columns: table => new
    {
        Id = table.Column<int>(nullable: false),
        QuizAttemptId = table.Column<int>(nullable: false),
        QuestionId = table.Column<int>(nullable: false),
        SelectedAnswerIds = table.Column<string>(type: "CLOB", nullable: true), // CSV string
        TextAnswer = table.Column<string>(type: "CLOB", nullable: true),
        IsCorrect = table.Column<int>(type: "NUMBER", nullable: false) // 0/1
    },
    constraints: table =>
    {
        table.PrimaryKey("PK_UserAnswers", x => x.Id);
        table.ForeignKey(
            name: "FK_UserAnswers_QuizAttempts",
            column: x => x.QuizAttemptId,
            principalTable: "QuizAttempts",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
        table.ForeignKey(
            name: "FK_UserAnswers_Questions",
            column: x => x.QuestionId,
            principalTable: "Questions",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    });

            // Indexes
            migrationBuilder.CreateIndex(
                name: "IX_Questions_QuizId",
                table: "Questions",
                column: "QuizId");

            migrationBuilder.CreateIndex(
                name: "IX_Answers_QuestionId",
                table: "Answers",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizAttempts_QuizId",
                table: "QuizAttempts",
                column: "QuizId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizAttempts_UserId",
                table: "QuizAttempts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAnswers_QuizAttemptId",
                table: "UserAnswers",
                column: "QuizAttemptId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAnswers_QuestionId",
                table: "UserAnswers",
                column: "QuestionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "UserAnswers");
            migrationBuilder.DropTable(name: "QuizAttempts");
            migrationBuilder.DropTable(name: "Answers");
            migrationBuilder.DropTable(name: "Questions");
            migrationBuilder.DropTable(name: "Users");
            migrationBuilder.DropTable(name: "Quizzes");
        }
    }
}
