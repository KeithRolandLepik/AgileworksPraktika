using System;
using System.Collections;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Domain.Users;
using Facade.Users;
using Infra.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Soft.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUsersRepository _usersRepository;
        private readonly AppSettings _appSettings;

        public UsersController(
            IUsersRepository usersRepository,
            IOptions<AppSettings> appSettings)
        {
            _usersRepository = usersRepository;
            _appSettings = appSettings.Value;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] UserRequest userRequest)
        {
            var user = await _usersRepository.Authenticate(userRequest.Username, userRequest.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Data.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            
            return Ok(new
            {
                user.Data.Id,
                user.Data.Username,
                user.Data.FirstName,
                user.Data.LastName,
                Token = tokenString
            });
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRequest userRequest)
        { 
            await _usersRepository.Create(UserMapper.MapRequestToDomain(userRequest), userRequest.Password); 
            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult<List<User>>> GetAll()
        {
            var users = await _usersRepository.GetAll();
            var usersList = users.ToList();
            return usersList;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetById(int id)
        {
            var user = await _usersRepository.GetById(id);
            return Ok(user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UserRequest userRequest)
        {
            userRequest.Id = id; 
            await _usersRepository.Update(UserMapper.MapRequestToDomain(userRequest), userRequest.Password); 
            return Ok();
            
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _usersRepository.GetById(id);
            if (user == null) return BadRequest();
         
            await _usersRepository.Delete(id);
            return Ok();
        }
    }
}