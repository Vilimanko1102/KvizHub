using KvizHubBack.DTOs.Answer;
using KvizHubBack.Models;
using KvizHubBack.Repositories;

namespace KvizHubBack.Services
{
    public class AnswerService : IAnswerService
    {
        private readonly IAnswerRepository _repo;

        public AnswerService(IAnswerRepository repo)
        {
            _repo = repo;
        }

        public AnswerDto CreateAnswer(AnswerCreateDto dto)
        {
            var answer = new Answer
            {
                Text = dto.Text,
                IsCorrect = dto.IsCorrect,
                QuestionId = dto.QuestionId
            };

            _repo.Add(answer);

            return new AnswerDto
            {
                Id = answer.Id,
                Text = answer.Text,
                IsCorrect = answer.IsCorrect,
                QuestionId = answer.QuestionId
            };
        }

        public IEnumerable<AnswerDto> GetAllAnswers()
        {
            return _repo.GetAll().Select(a => new AnswerDto
            {
                Id = a.Id,
                Text = a.Text,
                IsCorrect = a.IsCorrect,
                QuestionId = a.QuestionId
            });
        }

        public AnswerDto GetAnswerById(int id)
        {
            var a = _repo.GetById(id);
            if (a == null) throw new KeyNotFoundException();
            return new AnswerDto { Id = a.Id, Text = a.Text, IsCorrect = a.IsCorrect, QuestionId = a.QuestionId };
        }

        public AnswerDto UpdateAnswer(int id, AnswerUpdateDto dto)
        {
            var a = _repo.GetById(id);
            if (a == null) throw new KeyNotFoundException();

            a.Text = dto.Text;
            a.IsCorrect = dto.IsCorrect;
            a.QuestionId = dto.QuestionId;

            _repo.Update(a);

            return new AnswerDto { Id = a.Id, Text = a.Text, IsCorrect = a.IsCorrect, QuestionId = a.QuestionId };
        }

        public void DeleteAnswer(int id)
        {
            var a = _repo.GetById(id);
            if (a == null) throw new KeyNotFoundException();
            _repo.Delete(a);
        }
    }
}
