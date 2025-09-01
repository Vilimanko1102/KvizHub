using KvizHubBack.DTOs.KvizHubBack.DTOs.Question;
using KvizHubBack.DTOs.Question;

namespace KvizHubBack.Services
{
    public interface IQuestionService
    {
        QuestionDto CreateQuestion(QuestionCreateDto dto);
        IEnumerable<QuestionDto> GetAllQuestions();
        QuestionDto GetQuestionById(int id);
        QuestionDto UpdateQuestion(int id, QuestionUpdateDto dto);
        void DeleteQuestion(int id);
    }
}
