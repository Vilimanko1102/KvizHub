using KvizHubBack.DTOs.Answer;

namespace KvizHubBack.Services
{
    public interface IAnswerService
    {
        AnswerDto Create(AnswerCreateDto dto);
        AnswerDto Update(int id, AnswerUpdateDto dto);
        void Delete(int id);
        AnswerDto GetById(int id);
        IEnumerable<AnswerDto> GetByQuestionId(int questionId);
    }
}
