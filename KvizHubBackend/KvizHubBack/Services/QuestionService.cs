using KvizHubBack.DTOs.Question;
using KvizHubBack.Models;
using KvizHubBack.Repositories;

namespace KvizHubBack.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly IQuestionRepository _repo;

        public QuestionService(IQuestionRepository repo)
        {
            _repo = repo;
        }

        public QuestionDto CreateQuestion(QuestionCreateDto dto)
        {
            var question = new Question
            {
                Text = dto.Text,
                QuizId = dto.QuizId
            };

            _repo.Add(question);

            return new QuestionDto
            {
                Id = question.Id,
                Text = question.Text,
                QuizId = question.QuizId
            };
        }

        public IEnumerable<QuestionDto> GetAllQuestions()
        {
            return _repo.GetAll().Select(q => new QuestionDto
            {
                Id = q.Id,
                Text = q.Text,
                QuizId = q.QuizId
            });
        }

        public QuestionDto GetQuestionById(int id)
        {
            var q = _repo.GetById(id);
            if (q == null) throw new KeyNotFoundException();
            return new QuestionDto { Id = q.Id, Text = q.Text, QuizId = q.QuizId };
        }

        public QuestionDto UpdateQuestion(int id, QuestionUpdateDto dto)
        {
            var q = _repo.GetById(id);
            if (q == null) throw new KeyNotFoundException();

            q.Text = dto.Text;
            q.QuizId = dto.QuizId;

            _repo.Update(q);

            return new QuestionDto { Id = q.Id, Text = q.Text, QuizId = q.QuizId };
        }

        public void DeleteQuestion(int id)
        {
            var q = _repo.GetById(id);
            if (q == null) throw new KeyNotFoundException();
            _repo.Delete(q);
        }
    }
}
