using BCrypt.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.IdentityModel.Tokens;
using PontoCalculator.Data;
using PontoCalculator.Dtos;
using PontoCalculator.Helpers;
using PontoCalculator.Helpers.EmailService;
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
        private readonly IEmailService _emailService;
        private readonly JwtService _jwtService;
        public AuthController(IUserRepository repository, JwtService jwtService, IEmailService emailService)
        {
            _repository = repository;
            _emailService = emailService;
            _jwtService = jwtService;
        }

        [HttpPost("register")]
        public IActionResult Register(RegisterDto dto)
        {
            if (_repository.GetByEmail(dto.Email) != null)
            {
                return BadRequest("You already have an account!");
            }
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

            return Ok(new { message = "success", jwt = jwt });

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
            EmailDto emailDto = new EmailDto();
            emailDto.To = user.Email;
            emailDto.Subject = "Password Recovery";
            emailDto.Token = user.PasswordResetToken;
            _emailService.SendEmail(emailDto);
            return Ok(new
            {
                message = "You may now reset your password, please check your email inbox",
            });
        }

        [HttpPost("reset-password")]
        public IActionResult ResetPassword(ResetPasswordDto dto)
        {
            var user = _repository.GetByPasswordResetToken(dto.PasswordResetToken);
            if (user == null || user.PasswordResetTokenExpires < DateTime.UtcNow)
            {
                return BadRequest("Invalid password reset token, please request a new reset");
            }

            user.Password = BCrypt.Net.BCrypt.HashPassword(dto.newPassword);
            user.PasswordResetToken = null;
            user.PasswordResetTokenExpires = null;
            _repository.Update(user);
            return Ok("Password successfully updated!");
        }

        [HttpGet("validate-token")]
        public IActionResult ValidateToken(string passwordResetToken)
        {
            User user = _repository.GetByPasswordResetToken(passwordResetToken);
            if (user == null) return BadRequest("Invalid password reset token, please request a new reset");
            return Ok("success");
        }
    }
}