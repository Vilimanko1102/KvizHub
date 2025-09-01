using KvizHubBack.DTOs.UserAnswer;
using System.Collections.Generic;

namespace KvizHubBack.Services
{
    public interface IUserAnswerService
    {
        UserAnswerDto Create(UserAnswerCreateDto dto);
        UserAnswerDto Update(int id, UserAnswerUpdateDto dto);
        void Delete(int id);
        UserAnswerDto GetById(int id);
        IEnumerable<UserAnswerDto> GetByQuizAttempt(int quizAttemptId);
    }
}
