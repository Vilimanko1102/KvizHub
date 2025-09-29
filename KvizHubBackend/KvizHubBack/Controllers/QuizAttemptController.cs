using KvizHubBack.DTOs.QuizAttempt;
using KvizHubBack.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace KvizHubBack.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuizAttemptController : ControllerBase
    {
        private readonly IQuizAttemptService _service;

        public QuizAttemptController(IQuizAttemptService service)
        {
            _service = service;
        }

        [HttpPost]
        public ActionResult<QuizAttemptDto> Create(QuizAttemptSubmitDto dto)
        {
            var result = _service.SubmitAttempt(dto);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public ActionResult<QuizAttemptDto> Update(int id, QuizAttemptUpdateDto dto)
        {
            var result = _service.Update(id, dto);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _service.Delete(id);
            return NoContent();
        }

        [HttpGet("{id}")]
        public ActionResult<QuizAttemptDto> GetById(int id)
        {
            var result = _service.GetById(id);
            return Ok(result);
        }

        [HttpGet("user/{userId}")]
        public ActionResult<IEnumerable<QuizAttemptDto>> GetByUserId(int userId)
        {
            var result = _service.GetByUserId(userId);
            return Ok(result);
        }

        [HttpGet("quiz/{quizId}")]
        public ActionResult<IEnumerable<QuizAttemptDto>> GetByQuizId(int quizId)
        {
            var result = _service.GetByQuizId(quizId);
            return Ok(result);
        }
    }
}
