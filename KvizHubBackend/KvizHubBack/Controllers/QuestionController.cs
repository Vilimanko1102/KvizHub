using KvizHubBack.DTOs.Question;
using KvizHubBack.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace KvizHubBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private readonly IQuestionService _service;

        public QuestionController(IQuestionService service)
        {
            _service = service;
        }

        // ========================
        // Dohvatanje svih pitanja
        // ========================
        [HttpGet]
        public ActionResult<IEnumerable<QuestionDto>> GetAll()
        {
            var questions = _service.GetAll();
            return Ok(questions);
        }

        // ========================
        // Dohvatanje pitanja po ID
        // ========================
        [HttpGet("{id}")]
        public ActionResult<QuestionDto> GetById(int id)
        {
            try
            {
                var question = _service.GetById(id);
                return Ok(question);
            }
            catch (System.Exception e)
            {
                return NotFound(e.Message);
            }
        }

        // ========================
        // Dohvatanje pitanja po kvizu
        // ========================
        [HttpGet("quiz/{quizId}")]
        public ActionResult<IEnumerable<QuestionDto>> GetByQuizId(int quizId)
        {
            var questions = _service.GetByQuizId(quizId);
            return Ok(questions);
        }

        // ========================
        // Kreiranje novog pitanja (ADMIN)
        // ========================
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult<QuestionDto> Create([FromBody] QuestionCreateDto dto)
        {
            try
            {
                var question = _service.Create(dto);
                return CreatedAtAction(nameof(GetById), new { id = question.Id }, question);
            }
            catch (System.Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // ========================
        // Ažuriranje pitanja (ADMIN)
        // ========================
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public ActionResult<QuestionDto> Update(int id, [FromBody] QuestionUpdateDto dto)
        {
            try
            {
                var updatedQuestion = _service.Update(id, dto);
                return Ok(updatedQuestion);
            }
            catch (System.Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // ========================
        // Brisanje pitanja (ADMIN)
        // ========================
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
