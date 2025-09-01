using KvizHubBack.DTOs.UserAnswer;
using KvizHubBack.Models;
using KvizHubBack.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace KvizHubBack.Services
{
    public class UserAnswerService : IUserAnswerService
    {
        private readonly IUserAnswerRepository _repo;

        public UserAnswerService(IUserAnswerRepository repo)
        {
            _repo = repo;
        }

        public UserAnswerDto Create(UserAnswerCreateDto dto)
        {
            var answer = new UserAnswer
            {
                QuizAttemptId = dto.QuizAttemptId,
                QuestionId = dto.QuestionId,
                SelectedAnswerIds = dto.SelectedAnswerIds,
                TextAnswer = dto.TextAnswer,
                IsCorrect = false // initially false, can be updated after grading
            };

            _repo.Add(answer);

            return new UserAnswerDto
            {
                Id = answer.Id,
                QuizAttemptId = answer.QuizAttemptId,
                QuestionId = answer.QuestionId,
                SelectedAnswerIds = answer.SelectedAnswerIds,
                TextAnswer = answer.TextAnswer,
                IsCorrect = answer.IsCorrect
            };
        }

        public UserAnswerDto Update(int id, UserAnswerUpdateDto dto)
        {
            var answer = _repo.GetById(id) ?? throw new Exception("UserAnswer not found");

            if (dto.SelectedAnswerIds != null)
                answer.SelectedAnswerIds = dto.SelectedAnswerIds;

            if (!string.IsNullOrEmpty(dto.TextAnswer))
                answer.TextAnswer = dto.TextAnswer;

            answer.IsCorrect = dto.IsCorrect;

            _repo.Update(answer);

            return new UserAnswerDto
            {
                Id = answer.Id,
                QuizAttemptId = answer.QuizAttemptId,
                QuestionId = answer.QuestionId,
                SelectedAnswerIds = answer.SelectedAnswerIds,
                TextAnswer = answer.TextAnswer,
                IsCorrect = answer.IsCorrect
            };
        }

        public void Delete(int id)
        {
            var answer = _repo.GetById(id) ?? throw new Exception("UserAnswer not found");
            _repo.Delete(answer);
        }

        public UserAnswerDto GetById(int id)
        {
            var answer = _repo.GetById(id) ?? throw new Exception("UserAnswer not found");
            return new UserAnswerDto
            {
                Id = answer.Id,
                QuizAttemptId = answer.QuizAttemptId,
                QuestionId = answer.QuestionId,
                SelectedAnswerIds = answer.SelectedAnswerIds,
                TextAnswer = answer.TextAnswer,
                IsCorrect = answer.IsCorrect
            };
        }

        public IEnumerable<UserAnswerDto> GetByQuizAttempt(int quizAttemptId)
        {
            return _repo.GetByQuizAttempt(quizAttemptId)
                .Select(a => new UserAnswerDto
                {
                    Id = a.Id,
                    QuizAttemptId = a.QuizAttemptId,
                    QuestionId = a.QuestionId,
                    SelectedAnswerIds = a.SelectedAnswerIds,
                    TextAnswer = a.TextAnswer,
                    IsCorrect = a.IsCorrect
                });
        }
    }
}
