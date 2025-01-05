using UserService.DTOs;
using UserService.Models;
using UserService.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using UserService.Services;
using UserService.Kafka;

namespace UserService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;
        private readonly IKafkaProducer _kafkaProducer;

        public UserController(IUserRepository userRepository, IJwtService jwtService, IKafkaProducer kafkaProducer)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _kafkaProducer = kafkaProducer;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            if (await _userRepository.GetByEmailAsync(dto.Email) != null)
            {
                return BadRequest("Email is already registered.");
            }

            var user = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                PasswordHash = HashPassword(dto.Password)
            };

            await _userRepository.AddUserAsync(user);
            return Ok("User registered successfully.");
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgetPasswordDto forgetPasswordDto)
        {
            var email = forgetPasswordDto.Email;
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
                return BadRequest("Email not registered.");

            var otp = GenerateOtp();
            var expiry = DateTime.UtcNow.AddMinutes(3);
            await _userRepository.UpdateOPTAsync(email, otp, expiry);

            var notificationPayload = new
            {
                Type = "resetPassword",
                Email = email,
                Otp = otp
            };

            _kafkaProducer.Produce("job-topic", Guid.NewGuid().ToString(), notificationPayload);

            return Ok("OTP has been sent to your email.");
        }



        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto dto)
        {
            var user = await _userRepository.GetByOPTAsync(dto.Otp);
            if (user == null)
                return BadRequest("Invalid or expired OTP.");

            // Update password
            user.PasswordHash = HashPassword(dto.NewPassword);
            user.OPT = string.Empty;
            user.OPTExpiry = null;

            await _userRepository.UpdateUserAsync(user);

            return Ok("Password reset successfully.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await _userRepository.GetByEmailAsync(dto.Email);
            if (user == null || !VerifyPassword(dto.Password, user.PasswordHash))
            {
                return Unauthorized("Invalid email or password.");
            }

            var token = _jwtService.GenerateToken(user.Id, user.Email, user.Username);
            return Ok(new { Token = token });
        }

        private static string GenerateOtp()
        {
            var random = new Random();
            return random.Next(100000, 999999).ToString();
        }

        private static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            return Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(password)));
        }

        private static bool VerifyPassword(string password, string passwordHash)
        {
            using var sha256 = SHA256.Create();
            var hash = Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(password)));
            return hash == passwordHash;
        }
    }
}
