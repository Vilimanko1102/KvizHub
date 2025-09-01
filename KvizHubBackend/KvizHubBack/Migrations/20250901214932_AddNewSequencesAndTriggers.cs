using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KvizHubBack.Migrations
{
    /// <inheritdoc />
    public partial class AddNewSequencesAndTriggers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE SEQUENCE QUIZATTEMPTS_SEQ START WITH 1 INCREMENT BY 1 NOCACHE NOCYCLE");
            migrationBuilder.Sql(@"
                CREATE OR REPLACE TRIGGER QUIZATTEMPTS_TRG
                BEFORE INSERT ON ""QuizAttempts""
                FOR EACH ROW
                WHEN (NEW.""Id"" IS NULL)
                BEGIN
                    SELECT QUIZATTEMPTS_SEQ.NEXTVAL INTO :NEW.""Id"" FROM dual;
                END;
            ");

            // UserAnswers
            migrationBuilder.Sql(@"CREATE SEQUENCE USERANSWERS_SEQ START WITH 1 INCREMENT BY 1 NOCACHE NOCYCLE");
            migrationBuilder.Sql(@"
                CREATE OR REPLACE TRIGGER USERANSWERS_TRG
                BEFORE INSERT ON ""UserAnswers""
                FOR EACH ROW
                WHEN (NEW.""Id"" IS NULL)
                BEGIN
                    SELECT USERANSWERS_SEQ.NEXTVAL INTO :NEW.""Id"" FROM dual;
                END;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP TRIGGER QUIZATTEMPTS_TRG");
            migrationBuilder.Sql(@"DROP SEQUENCE QUIZATTEMPTS_SEQ");

            migrationBuilder.Sql(@"DROP TRIGGER USERANSWERS_TRG");
            migrationBuilder.Sql(@"DROP SEQUENCE USERANSWERS_SEQ");
        }
    }
}
