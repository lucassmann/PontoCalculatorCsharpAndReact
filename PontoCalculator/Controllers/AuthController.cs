using BCrypt.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.IdentityModel.Tokens;
using PontoCalculator.Data;
using PontoCalculator.Dtos;
using PontoCalculator.Helpers;
using PontoCalculator.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace PontoCalculator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IUserRepository _repository;
        private readonly JwtService _jwtService;
        public AuthController(IUserRepository repository, JwtService jwtService)
        {
            _repository = repository;
            _jwtService = jwtService;
        }

        [HttpPost("register")]
        public IActionResult Register(RegisterDto dto)
        {
            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            };

            return Created("Success", _repository.Create(user));
        }

        [HttpPost("login")]
        public IActionResult Login(LoginDto dto)
        {
            var user = _repository.GetByEmail(dto.Email);
            if (user == null) return BadRequest(new { message = "Invalid Credentials" });
            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.Password)) return BadRequest(new { message = "Invalid Credentials" });
            var jwt = _jwtService.Generate(user.Id);

            Response.Cookies.Append("jwt", jwt, new CookieOptions
            {
                HttpOnly = true
            });

            return Ok(new { message = "success" });

        }

        [HttpGet("user")]
        public IActionResult User()
        {
            try
            {
                var jwt = Request.Cookies["jwt"];
                var token = _jwtService.Verify(jwt);
                int userId = int.Parse(token.Issuer);
                var user = _repository.GetById(userId);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return Unauthorized();
            }
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwt");
            return Ok(new
            {
                message = "success"
            });
        }

        private string CreateRandomToken()
        {
            return Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
        }

        [HttpPost("forgot-password")]
        public IActionResult ForgotPassword(string email)
        {
            var user = _repository.GetByEmail(email);
            if (user == null) return BadRequest("User not found");
            user.PasswordResetToken = CreateRandomToken();
            user.PasswordResetTokenExpires = DateTime.UtcNow.AddDays(1);
            _repository.Update(user);
            return Ok("You may now reset your password");
        }
    }
}

        //public static User user = new User();

//private readonly IConfiguration _configuration;

//public AuthController(IConfiguration configuration)
//{
//    _configuration = configuration;
//}

//[HttpPost("register")]
//public ActionResult<User> Register(UserDto request)
//{
//    string passwordHash
//        = BCrypt.Net.BCrypt.HashPassword(request.Password);

//    user.Email = request.Email;
//    user.PasswordHash = passwordHash;

//    return Ok(user);

//}

//[HttpPost("login")]
//public ActionResult<User> Login(UserDto request)
//{
//    if ((user.Email != request.Email) || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
//    {
//        return BadRequest("Please check your e-mail and password then try again");
//    }

//    string token = CreateToken(user);

//    return Ok(token);

//}

//private String CreateToken(User user)
//{
//    List<Claim> claims = new List<Claim> {
//        new Claim(ClaimTypes.Email, user.Email)
//    };

//    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value!));

//    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

//    var token = new JwtSecurityToken(
//            claims: claims,
//            expires: DateTime.Now.AddMinutes(5),
//            signingCredentials: creds
//        ) ;

//    var jwt = new JwtSecurityTokenHandler().WriteToken(token);

//    return jwt;
//}
//    }
//}
