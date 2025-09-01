using KvizHubBack.DTOs.Answer;

namespace KvizHubBack.Services
{
    public interface IAnswerService
    {
        AnswerDto CreateAnswer(AnswerCreateDto dto);
        IEnumerable<AnswerDto> GetAllAnswers();
        AnswerDto GetAnswerById(int id);
        AnswerDto UpdateAnswer(int id, AnswerUpdateDto dto);
        void DeleteAnswer(int id);
    }
}
