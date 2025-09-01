using KvizHubBack.DTOs.User;
using KvizHubBack.Services;
using Microsoft.AspNetCore.Mvc;
using System;

namespace KvizHubBack.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;
        public UserController(IUserService service) => _service = service;

        [HttpPost("register")]
        public IActionResult Register(UserRegisterDto dto)
        {
            var result = _service.Register(dto);
            return Ok(result);
        }

        [HttpPost("login")]
        public IActionResult Login(UserLoginDto dto)
        {
            var result = _service.Login(dto);
            return Ok(result);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _service.GetAll();
            return Ok(users);
        }
    }

}
