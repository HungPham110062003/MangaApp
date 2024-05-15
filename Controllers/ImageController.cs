using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace MangaApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly CloudinaryDotNet.Cloudinary _cloudinary;

        public ImageController(CloudinaryDotNet.Cloudinary cloudinary) // Corrected here
        {
            _cloudinary = cloudinary;
        }

        [HttpPost("upload")]
        public IActionResult UploadImage([FromForm] IFormFile file)
        {
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(file.FileName, file.OpenReadStream()),
                PublicId = Path.GetFileNameWithoutExtension(file.FileName), // Set the public_id to the file name without extension
                Overwrite = true // Overwrite the existing image if it exists
            };

            var uploadResult = _cloudinary.Upload(uploadParams);

            if (uploadResult.Error != null)
            {
                return BadRequest(uploadResult.Error.Message);
            }

            return Ok(new { imageUrl = uploadResult.Url.ToString() });
        }
    }
}