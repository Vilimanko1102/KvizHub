using KvizHubBack.DTOs.QuizAttempt;
using KvizHubBack.DTOs.UserAnswer;
using KvizHubBack.Models;
using KvizHubBack.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace KvizHubBack.Services
{
    public class QuizAttemptService : IQuizAttemptService
    {
        private readonly IQuizAttemptRepository _repo;

        public QuizAttemptService(IQuizAttemptRepository repo)
        {
            _repo = repo;
        }

        public QuizAttemptDto Create(QuizAttemptCreateDto dto)
        {
            var attempt = new QuizAttempt
            {
                QuizId = dto.QuizId,
                UserId = dto.UserId,
                StartedAt = System.DateTime.UtcNow
            };

            _repo.Add(attempt);

            return new QuizAttemptDto
            {
                Id = attempt.Id,
                QuizId = attempt.QuizId,
                UserId = attempt.UserId,
                QuizTitle = attempt.Quiz?.Title ?? "",
                StartedAt = attempt.StartedAt
            };
        }

        public QuizAttemptDto Update(int id, QuizAttemptUpdateDto dto)
        {
            var attempt = _repo.GetById(id) ?? throw new System.Exception("Attempt not found");

            attempt.Score = dto.Score;
            attempt.Percentage = dto.Percentage;
            attempt.TimeSpent = dto.TimeSpent;
            attempt.FinishedAt = System.DateTime.UtcNow;

            _repo.Update(attempt);

            return new QuizAttemptDto
            {
                Id = attempt.Id,
                QuizId = attempt.QuizId,
                UserId = attempt.UserId,
                Score = attempt.Score,
                Percentage = attempt.Percentage,
                TimeSpent = attempt.TimeSpent,
                StartedAt = attempt.StartedAt,
                FinishedAt = attempt.FinishedAt,
                QuizTitle = attempt.Quiz?.Title ?? "",
                UserAnswers = attempt.UserAnswers?.Select(ua => new UserAnswerDto
                {
                    Id = ua.Id,
                    QuestionId = ua.QuestionId,
                    SelectedAnswerIds = ua.SelectedAnswerIds,
                    TextAnswer = ua.TextAnswer,
                    IsCorrect = ua.IsCorrect
                })
            };
        }

        public void Delete(int id)
        {
            var attempt = _repo.GetById(id) ?? throw new System.Exception("Attempt not found");
            _repo.Delete(attempt);
        }

        public QuizAttemptDto GetById(int id)
        {
            var attempt = _repo.GetById(id) ?? throw new System.Exception("Attempt not found");

            return new QuizAttemptDto
            {
                Id = attempt.Id,
                QuizId = attempt.QuizId,
                UserId = attempt.UserId,
                Score = attempt.Score,
                Percentage = attempt.Percentage,
                TimeSpent = attempt.TimeSpent,
                StartedAt = attempt.StartedAt,
                FinishedAt = attempt.FinishedAt,
                QuizTitle = attempt.Quiz?.Title ?? "",
                UserAnswers = attempt.UserAnswers?.Select(ua => new UserAnswerDto
                {
                    Id = ua.Id,
                    QuestionId = ua.QuestionId,
                    SelectedAnswerIds = ua.SelectedAnswerIds,
                    TextAnswer = ua.TextAnswer,
                    IsCorrect = ua.IsCorrect
                })
            };
        }

        public IEnumerable<QuizAttemptDto> GetByUserId(int userId)
        {
            return _repo.GetByUserId(userId)
                .Select(a => new QuizAttemptDto
                {
                    Id = a.Id,
                    QuizId = a.QuizId,
                    UserId = a.UserId,
                    Score = a.Score,
                    Percentage = a.Percentage,
                    TimeSpent = a.TimeSpent,
                    StartedAt = a.StartedAt,
                    FinishedAt = a.FinishedAt,
                    QuizTitle = a.Quiz?.Title ?? "",
                    UserAnswers = a.UserAnswers?.Select(ua => new UserAnswerDto
                    {
                        Id = ua.Id,
                        QuestionId = ua.QuestionId,
                        SelectedAnswerIds = ua.SelectedAnswerIds,
                        TextAnswer = ua.TextAnswer,
                        IsCorrect = ua.IsCorrect
                    })
                });
        }

        public IEnumerable<QuizAttemptDto> GetByQuizId(int quizId)
        {
            return _repo.GetByQuizId(quizId)
                .Select(a => new QuizAttemptDto
                {
                    Id = a.Id,
                    QuizId = a.QuizId,
                    UserId = a.UserId,
                    Score = a.Score,
                    Percentage = a.Percentage,
                    TimeSpent = a.TimeSpent,
                    StartedAt = a.StartedAt,
                    FinishedAt = a.FinishedAt,
                    QuizTitle = a.Quiz?.Title ?? "",
                    UserAnswers = a.UserAnswers?.Select(ua => new UserAnswerDto
                    {
                        Id = ua.Id,
                        QuestionId = ua.QuestionId,
                        SelectedAnswerIds = ua.SelectedAnswerIds,
                        TextAnswer = ua.TextAnswer,
                        IsCorrect = ua.IsCorrect
                    })
                });
        }
    }
}
