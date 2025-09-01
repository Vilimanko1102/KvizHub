using KvizHubBack.DTOs.Question;
using KvizHubBack.Models;
using KvizHubBack.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KvizHubBack.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly IQuestionRepository _repo;

        public QuestionService(IQuestionRepository repo)
        {
            _repo = repo;
        }

        public QuestionDto Create(QuestionCreateDto dto)
        {
            if (!Enum.TryParse<QuestionType>(dto.Type, true, out var type))
                throw new Exception("Invalid question type");

            var question = new Question
            {
                QuizId = dto.QuizId,
                Text = dto.Text,
                Type = type,
                Points = dto.Points
            };

            _repo.Add(question);
            return MapToDto(question);
        }

        public QuestionDto Update(int id, QuestionUpdateDto dto)
        {
            var question = _repo.GetById(id) ?? throw new Exception("Question not found");

            if (!string.IsNullOrEmpty(dto.Text))
                question.Text = dto.Text;

            if (!string.IsNullOrEmpty(dto.Type))
            {
                if (!Enum.TryParse<QuestionType>(dto.Type, true, out var type))
                    throw new Exception("Invalid question type");
                question.Type = type;
            }

            if (dto.Points.HasValue)
                question.Points = dto.Points.Value;

            _repo.Update(question);
            return MapToDto(question);
        }

        public void Delete(int id)
        {
            var question = _repo.GetById(id) ?? throw new Exception("Question not found");
            _repo.Delete(question);
        }

        public QuestionDto GetById(int id)
        {
            var question = _repo.GetById(id) ?? throw new Exception("Question not found");
            return MapToDto(question);
        }

        public IEnumerable<QuestionDto> GetAll()
        {
            return _repo.GetAll().Select(MapToDto);
        }

        public IEnumerable<QuestionDto> GetByQuizId(int quizId)
        {
            return _repo.GetByQuizId(quizId).Select(MapToDto);
        }

        private QuestionDto MapToDto(Question q)
        {
            return new QuestionDto
            {
                Id = q.Id,
                QuizId = q.QuizId,
                Text = q.Text,
                Type = q.Type.ToString(),
                Points = q.Points
            };
        }
    }
}
