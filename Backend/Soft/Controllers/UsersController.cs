using System;
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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Soft.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : BaseController
    {
        private readonly IUsersRepository _usersRepository;
        private readonly AppSettings _appSettings;
        private readonly UserRequestValidator _validator;
        public UsersController(
            IUsersRepository usersRepository,
            IOptions<AppSettings> appSettings)
        {
            _usersRepository = usersRepository;
            _appSettings = appSettings.Value;
            _validator = new UserRequestValidator();
        }

        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string[]))]
        [HttpPost("authenticate")]
        public async Task<ActionResult<UserModel>> Authenticate([FromBody] UserRequest userRequest)
        {
            var user = await _usersRepository.Authenticate(userRequest.Username, userRequest.Password);

            if (user == null)
                return BadRequest();

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new(ClaimTypes.Name, user.Data.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            
            return Ok(UserMapper.MapDomainToModel(user,tokenString));
        }

        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string[]))]
        [HttpPost("register")]
        public async Task<ActionResult<UserModel>> Register(UserRequest userRequest)
        { 
            var result = await _usersRepository.Create
                (UserMapper.MapRequestToDomain(userRequest), userRequest.Password);
            return Ok(UserMapper.MapDomainToModel(result, TokenGenerator(result)));
        }

        [HttpGet]
        public async Task<ActionResult<List<UserModel>>> GetAll()
        {
            var users = await _usersRepository.GetAll();
            var usersList = users.Select(x => UserMapper.MapDomainToModel(x,string.Empty)).ToList();
            return usersList;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserModel>> GetById(int id)
        {
            var user = await _usersRepository.GetById(id);
            if (user == null) return NotFound();

            return Ok(UserMapper.MapDomainToModel(user,string.Empty));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UserRequest userRequest)
        {
            var validationResult = await _validator.ValidateAsync(userRequest);
            if (!validationResult.IsValid) return BadRequest();
            
            if (userRequest.Id != id) return BadRequest(); 
            await _usersRepository.Update(UserMapper.MapRequestToDomain(userRequest), userRequest.Password); 
            return Ok();
            
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _usersRepository.GetById(id);

            if (user == null) return NotFound();
         
            await _usersRepository.Delete(id);
            return Ok();
        }
    }
}