using System;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MangaApp.Model.Domain;
using System.Security.Cryptography;
using Google.Apis.Auth;
using MangaApp.Data;
using MangaApp.DTO;
using MangaApp.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using MySqlConnector;

namespace MangaApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly MangaAppDbcontext _context;
        private readonly ITokenService _tokenService;

        public AuthController(MangaAppDbcontext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthDto>> Register( RegisterDto registerDto)
        {
            using var hmac = new HMACSHA512();
            if (await UserExists(registerDto.UserEmail)) return BadRequest("UserEmail  Is Already Taken");
            var user = new User
            {   
                UserId = Guid.NewGuid(),
                UserName = registerDto.UserName,
                UserEmail = registerDto.UserEmail.ToLower(),
                Avatar = "https://img.freepik.com/free-photo/person-preparing-get-driver-license_23-2150167558.jpg?t=st=1714792408~exp=1714796008~hmac=66a1407366466c5cf9538e70626c0d88501ab59383a7416a53667743666f4247&w=360",
                HashPassword = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                SaltPassword = hmac.Key,
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new AuthDto
            {
                Token = _tokenService.CreateToken(user),
            };
        }
        [HttpPost("Register/google")]
        public async Task<ActionResult<AuthDto>> GoogleRegister(string idToken)
        {
            var payload = await GoogleJsonWebSignature.ValidateAsync(idToken);

            // Check if a user with the given email already exists
            var user = await _context.Users.SingleOrDefaultAsync(x => x.UserEmail == payload.Email);
            if (user != null)
            {
                // If the user exists, return an error
                return BadRequest("A user with this email already exists.");
            }

            // If the user does not exist, create a new user
            user = new User
            {
                UserId = Guid.NewGuid(),
                UserName = payload.Name,
                UserEmail = payload.Email,
                Avatar = payload.Picture,
                CreatedAt = DateTime.UtcNow
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Generate a token for the new user
            return new AuthDto
            {
                Token = _tokenService.CreateToken(user),
            };
        }

        [HttpPost("Login/google")]
        public async Task<ActionResult<AuthDto>> GoogleLogin(string idToken)
        {
            var payload = await GoogleJsonWebSignature.ValidateAsync(idToken);

            // Check if a user with the given email exists
            var user = await _context.Users.SingleOrDefaultAsync(x => x.UserEmail == payload.Email);
            if (user == null)
            {
                // If the user does not exist, return an error
                return BadRequest("User not found.");
            }

            // Generate a token for the user
            return new AuthDto
            {
                Token = _tokenService.CreateToken(user),
            };
        }
        [HttpPost("login")]
        public async Task<ActionResult<AuthDto>> Login(LoginDto loginDto)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.UserEmail == loginDto.UserEmail);
            if (user == null) return Unauthorized("Invalid UserEmail");
            using var hmac = new HMACSHA512(user.SaltPassword);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.HashPassword[i]) return Unauthorized("Invalid Password");
            }
            return new AuthDto
            {
                Token = _tokenService.CreateToken(user),
            };
        }
        [Authorize]
        [HttpGet("profile")]
        public ActionResult<UserDto> GetUserProfile()
        {
            var userEmailClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userEmailClaim))
            {
                return BadRequest("User email claim not found.");
            }

            var user = _context.Users
                .Include(u => u.UserMangas) // Include UserMangas in the query
                .FirstOrDefault(u => u.UserEmail == userEmailClaim);
            if (user == null)
            {
                return NotFound("User not found.");
            }
            var userDto = new UserDto
            {
                UserID = user.UserId.ToString(),
                UserEmail = user.UserEmail,
                UserName = user.UserName,
                Avatar = user.Avatar,
                CreatedAt = user.CreatedAt,
                UserMangas = user.UserMangas?.Select(um => new UserMangaDto // Map UserManga to UserMangaDto
                {
                    MangaId = um.MangaId,
                    UserId = um.UserId,
                    Slug = um.Slug,
                    MangaName = um.MangaName,
                    MangaImage = um.MangaImage
                }).ToList()
            };

            return userDto;
        }
        private async Task <bool>UserExists(string userEmail)
        {
            return await _context.Users.AnyAsync(x => x.UserEmail == userEmail.ToLower());
        }
    }
}