using MangaApp.Data;
using MangaApp.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MangaApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly MangaAppDbcontext _dbContext;

        public UsersController(MangaAppDbcontext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUserById(Guid id)
        {
            var user = await _dbContext.Users
                .Include(u => u.UserMangas)
                .FirstOrDefaultAsync(u => u.UserId == id);

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
                UserMangas = user.UserMangas.Select(um => new UserMangaDto
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

        [HttpPut("{id}/face-authentication")]
        public async Task<IActionResult> UpdateFaceAuthenticationImage(Guid id, [FromBody] UpdateFaceDto updateDto)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserId == id);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            user.faceAuthenticationImage = updateDto.FaceAuthenticationImage;

            try
            {
                await _dbContext.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error updating the database.");
            }
        }

        [HttpPut("/Update/{id}")]
        public async Task<IActionResult> UpdateAvatar(Guid id, [FromBody] EditingUserDto updateDto)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserId == id);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            user.UserName = updateDto.UserName;
            user.Avatar = updateDto.Avatar;

            try
            {
                await _dbContext.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Status errors.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserId == id);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            _dbContext.Users.Remove(user);

            try
            {
                await _dbContext.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting the user.");
            }
        }
    }
}
