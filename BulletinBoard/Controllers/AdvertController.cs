using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Models;
using Services;

namespace BulletinBoard.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdvertController : ControllerBase
    {
        private readonly AdvertService _advertService;
        private readonly IImageService _imageService;
        private readonly IConfiguration _configuration;
        public AdvertController(AdvertService bulletinBoardService, IImageService imageService, IConfiguration configuration)
        {
            _advertService = bulletinBoardService;
            _imageService = imageService;
            _configuration = configuration; 
        }

        [HttpPost]
        public async Task<IActionResult> CreateAdvert(Guid userId, string text, [FromForm]List<IFormFile> images)
        {
            AdvertDto advertDto = new AdvertDto();
            advertDto.UserId = userId;
            advertDto.Text = text;
            advertDto.Rating = _configuration.GetValue<int>("AppSettings:DefaultAdvertRating");
            advertDto.ExpirationDate = DateTime.Now.AddDays(_configuration.GetValue<int>("AppSettings:ExpirationDays"));
            await _advertService.CreateAdvert(advertDto, images);
            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAdvertById(Guid id)
        {
            await _advertService.GetAdvertById(id);
            return Ok();
        }

        [HttpDelete]

        public async Task<IActionResult> DeleteAdvert(Guid advertId, Guid userId)
        {
            await _advertService.DeleteAdvert(advertId, userId);
            return NoContent();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAdvert(Guid advertId, string text, [FromForm] List<IFormFile> newImages, [FromForm] List<Guid> imagesToDelete)
        {
            var advert = await _advertService.GetAdvertById(advertId);

            if (advert == null)
            {
                return NotFound();
            }
            await _advertService.UpdateAdvert(advertId, text, newImages, imagesToDelete);
            return NoContent();
        }
    }
}
