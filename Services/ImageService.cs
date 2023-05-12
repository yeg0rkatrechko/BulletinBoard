using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Services
{
    public class ImageService : IImageService
    {
        private readonly string _imageDirectory;

        public ImageService(IConfiguration configuration)
        {
            _imageDirectory = configuration.GetValue<string>("ImageDirectory");
        }

        public async Task<string> UploadImage(IFormFile file)
        {
            if (file.Length == 0)
            {
                throw new Exception("Файл пуст");
            }

            var fileName = Path.GetFileName(file.FileName);
            var filePath = Path.Combine(_imageDirectory, Guid.NewGuid() + "_" + fileName);

            await using var fileStream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(fileStream);

            return filePath;
        }
        public async Task DeleteImage(string? imagePath)
        {
            if (File.Exists(imagePath))
            {
                await Task.Run(() => File.Delete(imagePath));
            }
        }
    }
}
