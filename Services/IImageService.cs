using Microsoft.AspNetCore.Http;

namespace Services
{
    public interface IImageService
    {
        Task<string> UploadImage(IFormFile image);
        Task DeleteImage(string imagePath);
    }
}
