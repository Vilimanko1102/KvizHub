using KvizHubBack.DTOs.UserAnswer;
using KvizHubBack.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace KvizHubBack.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserAnswerController : ControllerBase
    {
        private readonly IUserAnswerService _service;

        public UserAnswerController(IUserAnswerService service)
        {
            _service = service;
        }

        [HttpPost]
        public ActionResult<UserAnswerDto> Create(UserAnswerCreateDto dto)
        {
            return Ok(_service.Create(dto));
        }

        [HttpPut("{id}")]
        public ActionResult<UserAnswerDto> Update(int id, UserAnswerUpdateDto dto)
        {
            return Ok(_service.Update(id, dto));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _service.Delete(id);
            return NoContent();
        }

        [HttpGet("{id}")]
        public ActionResult<UserAnswerDto> GetById(int id)
        {
            return Ok(_service.GetById(id));
        }

        [HttpGet("quizAttempt/{quizAttemptId}")]
        public ActionResult<IEnumerable<UserAnswerDto>> GetByQuizAttempt(int quizAttemptId)
        {
            return Ok(_service.GetByQuizAttempt(quizAttemptId));
        }
    }
}
