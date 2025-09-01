using KvizHubBack.DTOs.Answer;
using KvizHubBack.Models;
using KvizHubBack.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace KvizHubBack.Services
{
    public class AnswerService : IAnswerService
    {
        private readonly IAnswerRepository _repo;

        public AnswerService(IAnswerRepository repo)
        {
            _repo = repo;
        }

        public AnswerDto Create(AnswerCreateDto dto)
        {
            var answer = new Answer
            {
                QuestionId = dto.QuestionId,
                Text = dto.Text,
                IsCorrect = dto.IsCorrect
            };

            _repo.Add(answer);

            return new AnswerDto
            {
                Id = answer.Id,
                QuestionId = answer.QuestionId,
                Text = answer.Text,
                IsCorrect = answer.IsCorrect
            };
        }

        public AnswerDto Update(int id, AnswerUpdateDto dto)
        {
            var answer = _repo.GetById(id) ?? throw new System.Exception("Answer not found");

            if (!string.IsNullOrEmpty(dto.Text))
                answer.Text = dto.Text;

            if (dto.IsCorrect.HasValue)
                answer.IsCorrect = dto.IsCorrect.Value;

            _repo.Update(answer);

            return new AnswerDto
            {
                Id = answer.Id,
                QuestionId = answer.QuestionId,
                Text = answer.Text,
                IsCorrect = answer.IsCorrect
            };
        }

        public void Delete(int id)
        {
            var answer = _repo.GetById(id) ?? throw new System.Exception("Answer not found");
            _repo.Delete(answer);
        }

        public AnswerDto GetById(int id)
        {
            var answer = _repo.GetById(id) ?? throw new System.Exception("Answer not found");
            return new AnswerDto
            {
                Id = answer.Id,
                QuestionId = answer.QuestionId,
                Text = answer.Text,
                IsCorrect = answer.IsCorrect
            };
        }

        public IEnumerable<AnswerDto> GetByQuestionId(int questionId)
        {
            return _repo.GetByQuestionId(questionId)
                .Select(a => new AnswerDto
                {
                    Id = a.Id,
                    QuestionId = a.QuestionId,
                    Text = a.Text,
                    IsCorrect = a.IsCorrect
                });
        }
    }
}
