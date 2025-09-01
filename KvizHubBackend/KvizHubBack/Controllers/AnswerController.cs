using KvizHubBack.DTOs.Answer;
using KvizHubBack.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace KvizHubBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnswerController : ControllerBase
    {
        private readonly IAnswerService _service;

        public AnswerController(IAnswerService service)
        {
            _service = service;
        }

        [HttpGet("{id}")]
        public ActionResult<AnswerDto> GetById(int id)
        {
            try
            {
                return Ok(_service.GetById(id));
            }
            catch (System.Exception e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpGet("question/{questionId}")]
        public ActionResult<IEnumerable<AnswerDto>> GetByQuestionId(int questionId)
        {
            return Ok(_service.GetByQuestionId(questionId));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult<AnswerDto> Create([FromBody] AnswerCreateDto dto)
        {
            var answer = _service.Create(dto);
            return CreatedAtAction(nameof(GetById), new { id = answer.Id }, answer);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public ActionResult<AnswerDto> Update(int id, [FromBody] AnswerUpdateDto dto)
        {
            try
            {
                return Ok(_service.Update(id, dto));
            }
            catch (System.Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                _service.Delete(id);
                return NoContent();
            }
            catch (System.Exception e)
            {
                return NotFound(e.Message);
            }
        }
    }
}
