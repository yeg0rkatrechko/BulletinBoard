using BulletinBoard.ServiceModel;
using Domain;
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

        [HttpGet("{advertId:guid}")]
        public async Task<IActionResult> GetAdvertById(Guid advertId, Guid userId)
        {
            var response = await _advertService.GetAdvertById(advertId, userId);
            return Ok(response);
        }

        [HttpGet("adverts")]
        public async Task<IActionResult> GetPublishedAdverts(int pageNumber, int pageSize)
        {
            var response = await _advertService.GetPublishedAdverts(pageNumber, pageSize);
            return Ok(response);
        }
        
        [HttpGet("adverts/search")]
        public async Task<IActionResult> SearchAdverts(int pageNumber, int pageSize, string? searchText, AdvertSortOrder sortOrder, DateTime? startDate, DateTime? endDate)
        {
            var response = await _advertService.SearchAdverts(pageNumber, pageSize, searchText, sortOrder, startDate, endDate);
            return Ok(response);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAdvert(Guid advertId, Guid userId)
        {
            await _advertService.DeleteAdvert(advertId, userId);
            return NoContent();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAdvert(Guid advertId, Guid userId, [FromForm] CreateAdvertRequest? updateAdvertRequest, bool isDraft, [FromForm] List<IFormFile> newImages, [FromForm] List<Guid> imagesToDelete)
        {
            await _advertService.UpdateAdvert(advertId, userId, updateAdvertRequest.Text, updateAdvertRequest.Heading, isDraft, newImages, imagesToDelete);
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
