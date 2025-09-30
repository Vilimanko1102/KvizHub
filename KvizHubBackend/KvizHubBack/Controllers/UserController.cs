using KvizHubBack.DTOs.User;
using KvizHubBack.Models;
using KvizHubBack.Services;
using Microsoft.AspNetCore.Mvc;

namespace KvizHubBack.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _service;
        private UserDto loggedUser;

        public UsersController(IUserService service)
        {
            _service = service;
        }

        [HttpPost("register")]
        public ActionResult<UserAuthResponseDto> Register([FromBody] UserRegisterDto dto)
        {
            try
            {
                var authResponse = _service.Register(dto);
                loggedUser = authResponse.User;
                return Created(string.Empty, authResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // ========================
        // Login korisnika
        // POST: api/users/login
        // ========================
        [HttpPost("login")]
        public ActionResult<UserAuthResponseDto> Login([FromBody] UserLoginDto dto)
        {
            try
            {
                var authResponse = _service.Login(dto);
                loggedUser = authResponse.User;
                return Ok(authResponse);
            }
            catch (Exception ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

        // ========================
        // Dohvatanje korisnika po ID
        // GET: api/users/{id}
        // ========================
        [HttpGet("me")]
        public ActionResult<UserDto> GetLoggedUser()
        {
            try
            {
                return Ok(loggedUser);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        // ========================
        // Dohvatanje korisnika po username
        // GET: api/users/by-username/{username}
        // ========================
        [HttpGet("{id}")]
        public ActionResult<UserDto> GetById(int id)
        {
            try
            {
                var user = _service.GetById(id);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        // ========================
        // Dohvatanje svih korisnika
        // GET: api/users
        // ========================
        [HttpGet]
        public ActionResult<IEnumerable<UserDto>> GetAll()
        {
            var users = _service.GetAll();
            return Ok(users);
        }

        // ========================
        // Update korisnika
        // PUT: api/users/{id}
        // ========================
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] UserUpdateDto dto)
        {
            try
            {
                _service.Update(id, dto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // ========================
        // Brisanje korisnika
        // DELETE: api/users/{id}
        // ========================
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                _service.Delete(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        // ========================
        // Dohvatanje istorije kvizova korisnika
        // GET: api/users/{id}/quiz-history
        // ========================
        [HttpGet("{id}/quiz-history")]
        public ActionResult<IEnumerable<UserQuizResultDto>> GetUserQuizHistory(int id)
        {
            try
            {
                var history = _service.GetUserQuizHistory(id);
                return Ok(history);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
