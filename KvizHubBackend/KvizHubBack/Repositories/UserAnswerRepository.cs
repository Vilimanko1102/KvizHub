using KvizHubBack.Data;
using KvizHubBack.Models;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace KvizHubBack.Repositories
{
    public class UserAnswerRepository : IUserAnswerRepository
    {
        private readonly KvizHubContext _context;
        private readonly string _connectionString;

        public UserAnswerRepository(KvizHubContext context, IConfiguration configuration)
        {
            _context = context;
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public void Add(UserAnswer answer)
        {
            using var connection = new OracleConnection(_connectionString);
            connection.Open();

            using var transaction = connection.BeginTransaction();
            using var command = connection.CreateCommand();
            command.Transaction = transaction;

            command.CommandText = @"
            INSERT INTO ""UserAnswers""
                (""QuizAttemptId"", ""QuestionId"", ""SelectedAnswerIdsCsv"", ""TextAnswer"", ""IsCorrect"")
            VALUES
                (:QuizAttemptId, :QuestionId, :SelectedAnswerIdsCsv, :TextAnswer, :IsCorrect)";

            // parametri
            command.Parameters.Add(new OracleParameter("QuizAttemptId", OracleDbType.Int32)
            {
                Value = answer.QuizAttemptId
            });

            command.Parameters.Add(new OracleParameter("QuestionId", OracleDbType.Int32)
            {
                Value = answer.QuestionId
            });

            command.Parameters.Add(new OracleParameter("SelectedAnswerIdsCsv", OracleDbType.Varchar2)
            {
                Value = (object?)answer.SelectedAnswerIdsCsv ?? DBNull.Value
            });

            command.Parameters.Add(new OracleParameter("TextAnswer", OracleDbType.Varchar2)
            {
                Value = (object?)answer.TextAnswer ?? DBNull.Value
            });

            command.Parameters.Add(new OracleParameter("IsCorrect", OracleDbType.Int16)
            {
                Value = answer.IsCorrect ? 1 : 0
            });

            command.ExecuteNonQuery();
            transaction.Commit();
        }



        public void Update(UserAnswer answer)
        {
            _context.UserAnswers.Update(answer);
            _context.SaveChanges();
        }

        public void Delete(UserAnswer answer)
        {
            _context.UserAnswers.Remove(answer);
            _context.SaveChanges();
        }

        public UserAnswer? GetById(int id)
        {
            var userAnswers = _context.UserAnswers
                .FromSqlRaw(@"SELECT * FROM ""UserAnswers"" WHERE ""Id"" = :id AND ROWNUM = 1",
                    new OracleParameter("id", id))
                .AsEnumerable();
            return userAnswers.FirstOrDefault();
        }

        public IEnumerable<UserAnswer> GetByQuizAttempt(int quizAttemptId)
        {
            return _context.UserAnswers
                .Where(ua => ua.QuizAttemptId == quizAttemptId)
                .ToList();
        }
    }
}
