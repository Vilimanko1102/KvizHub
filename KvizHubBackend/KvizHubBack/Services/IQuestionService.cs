using KvizHubBack.DTOs.Question;

namespace KvizHubBack.Services
{
    public interface IQuestionService
    {
        QuestionDto Create(QuestionCreateDto dto);
        QuestionDto Update(int id, QuestionUpdateDto dto);
        void Delete(int id);
        QuestionDto GetById(int id);
        IEnumerable<QuestionDto> GetAll();
        IEnumerable<QuestionDto> GetByQuizId(int quizId);
    }
}
