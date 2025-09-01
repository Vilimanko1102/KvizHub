using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KvizHubBack.Migrations
{
    public partial class AddSequencesAndTriggers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Users
            migrationBuilder.Sql(@"CREATE SEQUENCE USERS_SEQ START WITH 1 INCREMENT BY 1 NOCACHE NOCYCLE");
            migrationBuilder.Sql(@"
                CREATE OR REPLACE TRIGGER USERS_TRG
                BEFORE INSERT ON ""Users""
                FOR EACH ROW
                WHEN (NEW.""Id"" IS NULL)
                BEGIN
                    SELECT USERS_SEQ.NEXTVAL INTO :NEW.""Id"" FROM dual;
                END;
            ");

            // Quiz
            migrationBuilder.Sql(@"CREATE SEQUENCE QUIZZES_SEQ START WITH 1 INCREMENT BY 1 NOCACHE NOCYCLE");
            migrationBuilder.Sql(@"
                CREATE OR REPLACE TRIGGER QUIZZES_TRG
                BEFORE INSERT ON ""Quizzes""
                FOR EACH ROW
                WHEN (NEW.""Id"" IS NULL)
                BEGIN
                    SELECT QUIZZES_SEQ.NEXTVAL INTO :NEW.""Id"" FROM dual;
                END;
            ");

            // Question
            migrationBuilder.Sql(@"CREATE SEQUENCE QUESTIONS_SEQ START WITH 1 INCREMENT BY 1 NOCACHE NOCYCLE");
            migrationBuilder.Sql(@"
                CREATE OR REPLACE TRIGGER QUESTIONS_TRG
                BEFORE INSERT ON ""Questions""
                FOR EACH ROW
                WHEN (NEW.""Id"" IS NULL)
                BEGIN
                    SELECT QUESTIONS_SEQ.NEXTVAL INTO :NEW.""Id"" FROM dual;
                END;
            ");

            // Answer
            migrationBuilder.Sql(@"CREATE SEQUENCE ANSWERS_SEQ START WITH 1 INCREMENT BY 1 NOCACHE NOCYCLE");
            migrationBuilder.Sql(@"
                CREATE OR REPLACE TRIGGER ANSWERS_TRG
                BEFORE INSERT ON ""Answers""
                FOR EACH ROW
                WHEN (NEW.""Id"" IS NULL)
                BEGIN
                    SELECT ANSWERS_SEQ.NEXTVAL INTO :NEW.""Id"" FROM dual;
                END;
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP TRIGGER USERS_TRG");
            migrationBuilder.Sql(@"DROP SEQUENCE USERS_SEQ");

            migrationBuilder.Sql(@"DROP TRIGGER QUIZZES_TRG");
            migrationBuilder.Sql(@"DROP SEQUENCE QUIZZES_SEQ");

            migrationBuilder.Sql(@"DROP TRIGGER QUESTIONS_TRG");
            migrationBuilder.Sql(@"DROP SEQUENCE QUESTIONS_SEQ");

            migrationBuilder.Sql(@"DROP TRIGGER ANSWERS_TRG");
            migrationBuilder.Sql(@"DROP SEQUENCE ANSWERS_SEQ");
        }
    }
}
