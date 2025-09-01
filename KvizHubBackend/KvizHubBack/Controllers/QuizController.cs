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
        public IActionResult Create([FromBody] QuizCreateDto dto) => Ok(_service.Create(dto));

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] QuizUpdateDto dto) => Ok(_service.Update(id, dto));

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _service.Delete(id);
            return NoContent();
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id) => Ok(_service.GetById(id));

        [HttpGet]
        public IActionResult GetAll() => Ok(_service.GetAll());

        [HttpGet("filter")]
        public IActionResult Filter([FromQuery] string? category, [FromQuery] string? difficulty, [FromQuery] string? search)
        {
            return Ok(_service.Filter(category, difficulty, search));
        }

        [HttpGet("{id}/results")]
        public IActionResult GetResults(int id) => Ok(_service.GetQuizResults(id));
    }
}
