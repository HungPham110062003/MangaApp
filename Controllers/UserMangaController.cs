using System.Security.Claims;
using MangaApp.Data;
using MangaApp.DTO;
using MangaApp.Model.Domain;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace MangaApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserMangaController : ControllerBase
    {
        private readonly MangaAppDbcontext _dbContext;

        public UserMangaController(MangaAppDbcontext dbContext)
        {
            _dbContext = dbContext;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("savemanga")]
        public async Task<ActionResult<mangaDto>> SaveManga(Guid userId, [FromBody] mangaDto mangaDto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("Invalid token.");
            }

            if (mangaDto == null)
            {
                return BadRequest("Manga data is required.");
            }

            // Check if the manga is already saved by the user
            var existingManga = await _dbContext.UserMangas
                .FirstOrDefaultAsync(um => um.UserId == userId && um.Slug == mangaDto.Slug);

            if (existingManga != null)
            {
                return Conflict("This manga is already saved by the user.");
            }

            var manga = new UserManga
            {
                UserId = userId,
                Slug = mangaDto.Slug,
                MangaName = mangaDto.MangaName,
                MangaImage = mangaDto.MangaImage
            };

            _dbContext.UserMangas.Add(manga);
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                return StatusCode(StatusCodes.Status500InternalServerError, "Error saving manga data.");
            }

            return CreatedAtAction(nameof(SaveManga), new { id = manga.Slug }, mangaDto);
        }
    }
}