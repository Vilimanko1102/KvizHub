using KvizHubBack.DTOs.Answer;
using KvizHubBack.Services;
using Microsoft.AspNetCore.Mvc;

namespace KvizHubBack.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnswerController : ControllerBase
    {
        private readonly IAnswerService _service;

        public AnswerController(IAnswerService service)
        {
            _service = service;
        }

        [HttpPost]
        public IActionResult CreateAnswer([FromBody] AnswerCreateDto dto)
        {
            var answer = _service.CreateAnswer(dto);
            return Ok(answer);
        }

        [HttpGet]
        public IActionResult GetAllAnswers()
        {
            var answers = _service.GetAllAnswers();
            return Ok(answers);
        }

        [HttpGet("{id}")]
        public IActionResult GetAnswerById(int id)
        {
            try
            {
                var answer = _service.GetAnswerById(id);
                return Ok(answer);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateAnswer(int id, [FromBody] AnswerUpdateDto dto)
        {
            try
            {
                var answer = _service.UpdateAnswer(id, dto);
                return Ok(answer);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteAnswer(int id)
        {
            try
            {
                _service.DeleteAnswer(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
