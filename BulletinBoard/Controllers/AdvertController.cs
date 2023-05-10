using BulletinBoard.ServiceModel;
using Microsoft.AspNetCore.Mvc;
using Models.Dto;
using Services;

namespace BulletinBoard.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdvertController : ControllerBase
    {
        private readonly IAdvertService _advertService;
        public AdvertController(IAdvertService advertService)
        {
            _advertService = advertService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAdvert(Guid userId, [FromForm] CreateAdvertRequest createAdvertRequest, [FromForm] List<IFormFile> images, bool isDraft)
        {
            await _advertService.CreateAdvert(userId, createAdvertRequest.Text, createAdvertRequest.Heading, images, isDraft);
            return NoContent();
        }

        [HttpGet("{advertId}")]
        public async Task<IActionResult> GetAdvertById(Guid advertId, Guid userId)
        {
            var response = await _advertService.GetAdvertById(advertId, userId);
            return Ok(response);
        }

        [HttpGet("adverts")]
        public async Task<IActionResult> GetAllPublishedAdverts()
        {
            var response = await _advertService.GetAllPublishedAdverts();
            return Ok(response);
        }
        
        [HttpGet("adverts/search")]
        public async Task<IActionResult> SearchAdverts(string searchText, AdvertSortOrder sortOrder, DateTime? startDate, DateTime? endDate)
        {
            var response = await _advertService.SearchAdverts(searchText, sortOrder, startDate, endDate);
            return Ok(response);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAdvert(Guid advertId, Guid userId)
        {
            await _advertService.DeleteAdvert(advertId, userId);
            return NoContent();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAdvert(Guid advertId, Guid userId, [FromForm] CreateAdvertRequest? createAdvertRequest, bool isDraft, [FromForm] List<IFormFile> newImages, [FromForm] List<Guid> imagesToDelete)
        {
            if (createAdvertRequest == null)
            {
                throw new ArgumentNullException(nameof(createAdvertRequest));
            }

            await _advertService.UpdateAdvert(advertId, userId, createAdvertRequest.Text, isDraft, newImages, imagesToDelete);
            return NoContent();
        }

        [HttpPut("react")]
        public async Task<IActionResult> ReactToAdvert(Guid userId, Guid advertId, bool isLike)
        {
            await _advertService.ReactToAdvert(userId, advertId, isLike);
            return NoContent();
        }
    }
}
