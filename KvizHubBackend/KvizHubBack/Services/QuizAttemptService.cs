using KvizHubBack.DTOs.QuizAttempt;
using KvizHubBack.DTOs.UserAnswer;
using KvizHubBack.Models;
using KvizHubBack.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace KvizHubBack.Services
{
    public class QuizAttemptSubmitDto
    {
        public int QuizId { get; set; }
        public int UserId { get; set; }
        public List<UserAnswerSubmitDto> UserAnswers { get; set; } = new();

        public int TimeSpent { get; set; }
    }

    public class UserAnswerSubmitDto
    {
        public int QuestionId { get; set; }
        public List<int>? SelectedAnswerIds { get; set; }
        public string? TextAnswer { get; set; }
    }

    public class QuizAttemptService : IQuizAttemptService
    {
        private readonly IQuizAttemptRepository _QArepo;
        private readonly IQuestionRepository _Qrepo;
        private readonly IUserAnswerRepository _UArepo;

        public QuizAttemptService(IQuizAttemptRepository QArepo, IQuestionRepository Qrepo, IUserAnswerRepository UArepo)
        {
            _QArepo = QArepo;
            _Qrepo = Qrepo;
            _UArepo = UArepo;
        }

        public QuizAttemptDto SubmitAttempt(QuizAttemptSubmitDto dto)
        {
            // 1. Kreiraj novi QuizAttempt
            var attempt = new QuizAttempt
            {
                QuizId = dto.QuizId,
                UserId = dto.UserId,
                StartedAt = DateTime.UtcNow,
            };

            _QArepo.Add(attempt);

            int totalPoints = 0;
            int earnedPoints = 0;

            // 2. Iteriraj kroz odgovore korisnika i popuni UserAnswers
            foreach (var uaDto in dto.UserAnswers)
            {
                var question = _Qrepo.GetById(uaDto.QuestionId);
                if (question == null) continue;

                int questionPoints = question.Points;
                totalPoints += questionPoints;

                bool isCorrect = false;

                switch (question.Type)
                {
                    case QuestionType.SingleChoice:
                    case QuestionType.TrueFalse:
                        var correctAnswer = question.Answers.FirstOrDefault(a => a.IsCorrect);
                        isCorrect = uaDto.SelectedAnswerIds != null &&
                                    uaDto.SelectedAnswerIds.Count == 1 &&
                                    uaDto.SelectedAnswerIds[0] == correctAnswer?.Id;
                        break;

                    case QuestionType.MultipleChoice:
                        var correctAnswerIds = question.Answers.Where(a => a.IsCorrect)
                                                               .Select(a => a.Id)
                                                               .ToList();
                        if (uaDto.SelectedAnswerIds != null)
                        {
                            isCorrect = !correctAnswerIds.Except(uaDto.SelectedAnswerIds).Any() &&
                                        !uaDto.SelectedAnswerIds.Except(correctAnswerIds).Any();
                        }
                        break;

                    case QuestionType.FillIn:
                        isCorrect = question.Answers.Any(a =>
                            a.Text.ToLower().Trim() == (uaDto.TextAnswer ?? "").ToLower().Trim());
                        break;
                }

                if (isCorrect) earnedPoints += questionPoints;

                // Pretvori SelectedAnswerIds u CSV string
                string? selectedCsv = uaDto.SelectedAnswerIds == null ? null : string.Join(',', uaDto.SelectedAnswerIds);

                // Dodaj odgovor u navigaciono svojstvo
                _UArepo.Add(new UserAnswer
                {
                    QuestionId = question.Id,
                    QuizAttemptId = attempt.Id,
                    SelectedAnswerIdsCsv = selectedCsv,  // upisujemo CSV string
                    TextAnswer = uaDto.TextAnswer,
                    IsCorrect = isCorrect
                });
            }

            // 3. Izračunaj score i percentage
            attempt.Score = earnedPoints;
            attempt.Percentage = totalPoints > 0 ? (float)earnedPoints / totalPoints * 100 : 0;
            attempt.FinishedAt = DateTime.UtcNow;
            attempt.TimeSpent = dto.TimeSpent; // sekundama, po potrebi sa fronta

            // 4. Sačuvaj sve odjednom (i UserAnswers će se upisati jer su navigaciono svojstvo)
            _QArepo.Update(attempt);

            // 5. Vrati DTO
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
                QuizTitle = attempt.Quiz?.Title ?? ""
            };
        }



        public QuizAttemptDto Update(int id, QuizAttemptUpdateDto dto)
        {
            var attempt = _QArepo.GetById(id) ?? throw new System.Exception("Attempt not found");

            attempt.Score = dto.Score;
            attempt.Percentage = dto.Percentage;
            attempt.TimeSpent = dto.TimeSpent;
            attempt.FinishedAt = System.DateTime.UtcNow;

            _QArepo.Update(attempt);

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
            var attempt = _QArepo.GetById(id) ?? throw new System.Exception("Attempt not found");
            _QArepo.Delete(attempt);
        }

        public QuizAttemptDto GetById(int id)
        {
            var attempt = _QArepo.GetById(id) ?? throw new System.Exception("Attempt not found");

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
            return _QArepo.GetByUserId(userId)
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
            return _QArepo.GetByQuizId(quizId)
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
