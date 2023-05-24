using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Services.DataContract;

namespace Services
{
    public class ImageService : IImageService
    {
        private readonly IImageRepository _imageRepository;
        private readonly string _imageDirectory;

        public ImageService(IConfiguration configuration, IImageRepository imageRepository)
        {
            _imageRepository = imageRepository;
            _imageDirectory = configuration.GetValue<string>("ImageDirectory");
        }

        public async Task<string> UploadImage(IFormFile file)
        {
            var fileName = Path.GetFileName(file.FileName);
            var filePath = Path.Combine(_imageDirectory, Guid.NewGuid() + "_" + fileName);
            return await _imageRepository.SaveImage(file, filePath);
        }

        public async Task DeleteImage(string? imagePath)
        {
            await _imageRepository.DeleteImage(imagePath);
        }
    }
}
