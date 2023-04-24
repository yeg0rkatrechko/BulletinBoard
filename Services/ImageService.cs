using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            if (file == null || file.Length == 0)
            {
                return null;
            }

            var fileName = Path.GetFileName(file.FileName);
            var filePath = Path.Combine(_imageDirectory, Guid.NewGuid().ToString() + "_" + fileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }

            return filePath;
        }
        public async Task DeleteImage(string imagePath)
        {
            if (File.Exists(imagePath))
            {
                File.Delete(imagePath);
            }
        }
    }
}
