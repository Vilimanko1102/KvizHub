using KvizHubBack.DTOs.Quiz;
using KvizHubBack.Services;
using Microsoft.AspNetCore.Mvc;

namespace KvizHubBack.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuizController : ControllerBase
    {
        private readonly IQuizService _service;

        public QuizController(IQuizService service)
        {
            _service = service;
        }

        [HttpPost]
        public IActionResult CreateQuiz([FromBody] QuizCreateDto dto)
        {
            var quiz = _service.CreateQuiz(dto);
            return Ok(quiz);
        }

        [HttpGet]
        public IActionResult GetAllQuizzes()
        {
            IEnumerable<QuizDto> quizzes = _service.GetAllQuizzes();
            return Ok(quizzes);
        }

        [HttpGet("{id}")]
        public IActionResult GetQuizById(int id)
        {
            try
            {
                var quiz = _service.GetQuizById(id);
                return Ok(quiz);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateQuiz(int id, [FromBody] QuizUpdateDto dto)
        {
            try
            {
                var quiz = _service.UpdateQuiz(id, dto);
                return Ok(quiz);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteQuiz(int id)
        {
            try
            {
                _service.DeleteQuiz(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
