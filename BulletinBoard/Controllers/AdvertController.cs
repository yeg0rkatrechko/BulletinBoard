using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Models;
using Services;
using static Services.AdvertService;

namespace BulletinBoard.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdvertController : ControllerBase
    {
        private readonly AdvertService _advertService;
        public AdvertController(AdvertService advertService)
        {
            _advertService = advertService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAdvert(Guid userId, string text, [FromForm] List<IFormFile> images, bool isDraft)
        {
            await _advertService.CreateAdvert(userId, text, images, isDraft);
            return NoContent();
        }

        [HttpGet("{advertId}")]
        public async Task<IActionResult> GetAdvertById(Guid advertId, Guid userId)
        {
            var response = await _advertService.GetAdvertById(advertId, userId);
            return Ok(response);
        }

        [HttpGet("adverts")]
        public async Task<IActionResult> GetAllPublishedAdverts(AdvertSortOrder sortOrder)
        {
            var response = await _advertService.GetAllPublishedAdverts(sortOrder);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> SearchAdverts(string searchText, AdvertSortOrder sortOrder)
        {
            var response = await _advertService.SearchAdverts(searchText, sortOrder);
            return Ok(response);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAdvert(Guid advertId, Guid userId)
        {
            await _advertService.DeleteAdvert(advertId, userId);
            return NoContent();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAdvert(Guid advertId, Guid userId, string? text, bool isDraft, [FromForm] List<IFormFile> newImages, [FromForm] List<Guid> imagesToDelete)
        {
            await _advertService.UpdateAdvert(advertId, userId, text, isDraft, newImages, imagesToDelete);
            return NoContent();
        }

        [HttpPut("react")]
        public async Task<IActionResult> ReactToAdvert(Guid userId, Guid advertId, Reaction reaction)
        {
            await _advertService.ReactToAdvert(userId, advertId, reaction);
            return NoContent();
        }
    }
}
