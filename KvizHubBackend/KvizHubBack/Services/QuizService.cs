using KvizHubBack.DTOs.Quiz;
using KvizHubBack.Models;
using KvizHubBack.Repositories;

namespace KvizHubBack.Services
{
    public class QuizService : IQuizService
    {
        private readonly IQuizRepository _repo;

        public QuizService(IQuizRepository repo)
        {
            _repo = repo;
        }

        public QuizDto CreateQuiz(QuizCreateDto dto)
        {
            var quiz = new Quiz
            {
                Title = dto.Title
            };

            _repo.Add(quiz);

            return new QuizDto
            {
                Id = quiz.Id,
                Title = quiz.Title
            };
        }

        public IEnumerable<QuizDto> GetAllQuizzes()
        {
            return _repo.GetAll()
                        .Select(q => new QuizDto
                        {
                            Id = q.Id,
                            Title = q.Title
                        });
        }

        public QuizDto GetQuizById(int id)
        {
            var quiz = _repo.GetById(id);
            if (quiz == null)
                throw new KeyNotFoundException("Quiz not found");

            return new QuizDto
            {
                Id = quiz.Id,
                Title = quiz.Title
            };
        }

        public QuizDto UpdateQuiz(int id, QuizUpdateDto dto)
        {
            var quiz = _repo.GetById(id);
            if (quiz == null)
                throw new KeyNotFoundException("Quiz not found");

            quiz.Title = dto.Title;

            _repo.Update(quiz);

            return new QuizDto
            {
                Id = quiz.Id,
                Title = quiz.Title
            };
        }

        public void DeleteQuiz(int id)
        {
            var quiz = _repo.GetById(id);
            if (quiz == null)
                throw new KeyNotFoundException("Quiz not found");

            _repo.Delete(quiz);
        }
    }
}
