using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KvizHubBack.Migrations
{
    public partial class InitialCreation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Quizzes
            migrationBuilder.CreateTable(
                name: "Quizzes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    Title = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Qz", x => x.Id); // Kratko ime PK
                });

            // Users
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    Username = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    PasswordHash = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    Email = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_U", x => x.Id); // Kratko ime PK
                });

            // Questions
            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    Text = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    QuizId = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Q", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Q_Qz", // Kratko ime FK
                        column: x => x.QuizId,
                        principalTable: "Quizzes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            // Answers
            migrationBuilder.CreateTable(
                name: "Answers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    Text = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    IsCorrect = table.Column<int>(type: "NUMBER(1)", nullable: false), // Zamena za bool
                    QuestionId = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_A", x => x.Id);
                    table.ForeignKey(
                        name: "FK_A_Q", // Kratko ime FK
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            // Indeksi
            migrationBuilder.CreateIndex(
                name: "IX_A_QId",
                table: "Answers",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Q_QzId",
                table: "Questions",
                column: "QuizId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "Answers");
            migrationBuilder.DropTable(name: "Users");
            migrationBuilder.DropTable(name: "Questions");
            migrationBuilder.DropTable(name: "Quizzes");
        }
    }
}
