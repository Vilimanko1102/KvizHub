using KvizHubBack.DTOs.QuizAttempt;
using System.Collections.Generic;

namespace KvizHubBack.Services
{
    public interface IQuizAttemptService
    {
        QuizAttemptDto Create(QuizAttemptCreateDto dto);
        QuizAttemptDto Update(int id, QuizAttemptUpdateDto dto);
        void Delete(int id);
        QuizAttemptDto GetById(int id);
        IEnumerable<QuizAttemptDto> GetByUserId(int userId);
        IEnumerable<QuizAttemptDto> GetByQuizId(int quizId);
    }
}
