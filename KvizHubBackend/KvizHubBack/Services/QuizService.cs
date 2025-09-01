using KvizHubBack.DTOs.Quiz;
using KvizHubBack.Models;
using KvizHubBack.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace KvizHubBack.Services
{
    public class QuizService : IQuizService
    {
        private readonly IQuizRepository _repo;

        public QuizService(IQuizRepository repo)
        {
            _repo = repo;
        }

        private Difficulty ToDifficulty(string val)
        {
            if (val.ToLower().Equals("easy")) return Difficulty.Easy;
            if (val.ToLower().Equals("medium")) return Difficulty.Medium;
            if (val.ToLower().Equals("hard")) return Difficulty.Hard;
            return 0;
        }

        public QuizDto Create(QuizCreateDto dto)
        {
            var quiz = new Quiz
            {
                Title = dto.Title,
                Description = dto.Description,
                Category = dto.Category,
                Difficulty = ToDifficulty(dto.Difficulty),
                TimeLimit = dto.TimeLimit
            };

            _repo.Add(quiz);

            return MapToDto(quiz);
        }

        public QuizDto Update(int id, QuizUpdateDto dto)
        {
            var quiz = _repo.GetById(id) ?? throw new Exception("Quiz not found");

            if (!string.IsNullOrEmpty(dto.Title))
                quiz.Title = dto.Title;
            if (!string.IsNullOrEmpty(dto.Description))
                quiz.Description = dto.Description;
            if (!string.IsNullOrEmpty(dto.Category))
                quiz.Category = dto.Category;
            if (!string.IsNullOrEmpty(dto.Difficulty))
                quiz.Difficulty = ToDifficulty(dto.Difficulty);
            if (dto.TimeLimit.HasValue)
                quiz.TimeLimit = dto.TimeLimit.Value;

            _repo.Update(quiz);

            return MapToDto(quiz);
        }

        public void Delete(int id)
        {
            var quiz = _repo.GetById(id) ?? throw new Exception("Quiz not found");
            _repo.Delete(quiz);
        }

        public QuizDto GetById(int id)
        {
            var quiz = _repo.GetById(id) ?? throw new Exception("Quiz not found");
            return MapToDto(quiz);
        }

        public IEnumerable<QuizDto> GetAll()
        {
            return _repo.GetAll().Select(MapToDto);
        }

        public IEnumerable<QuizDto> Filter(string? category, string? difficulty, string? search)
        {
            return _repo.Filter(category, difficulty, search).Select(MapToDto);
        }

        public IEnumerable<QuizResultDto> GetQuizResults(int quizId)
        {
            var attempts = _repo.GetQuizResults(quizId);

            return attempts.Select(a => new QuizResultDto
            {
                UserId = a.UserId,
                Username = a.User?.Username ?? "",
                Score = a.Score,
                Percentage = a.Percentage,
                FinishedAt = a.FinishedAt!.Value
            });
        }

        private QuizDto MapToDto(Quiz quiz)
        {
            return new QuizDto
            {
                Id = quiz.Id,
                Title = quiz.Title,
                Description = quiz.Description,
                Category = quiz.Category,
                Difficulty = quiz.Difficulty.ToString(),
                TimeLimit = quiz.TimeLimit,
                QuestionCount = quiz.Questions?.Count ?? 0
            };
        }
    }
}
