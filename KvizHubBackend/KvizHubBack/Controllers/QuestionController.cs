using KvizHubBack.DTOs.Question;
using KvizHubBack.Services;
using Microsoft.AspNetCore.Mvc;

namespace KvizHubBack.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuestionController : ControllerBase
    {
        private readonly IQuestionService _service;

        public QuestionController(IQuestionService service)
        {
            _service = service;
        }

        [HttpPost]
        public IActionResult CreateQuestion([FromBody] QuestionCreateDto dto)
        {
            var question = _service.CreateQuestion(dto);
            return Ok(question);
        }

        [HttpGet]
        public IActionResult GetAllQuestions()
        {
            var questions = _service.GetAllQuestions();
            return Ok(questions);
        }

        [HttpGet("{id}")]
        public IActionResult GetQuestionById(int id)
        {
            try
            {
                var question = _service.GetQuestionById(id);
                return Ok(question);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateQuestion(int id, [FromBody] QuestionUpdateDto dto)
        {
            try
            {
                var question = _service.UpdateQuestion(id, dto);
                return Ok(question);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteQuestion(int id)
        {
            try
            {
                _service.DeleteQuestion(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
