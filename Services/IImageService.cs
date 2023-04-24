using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Services
{
    public interface IImageService
    {
        Task<string> UploadImage(IFormFile image);
        Task DeleteImage(string imagePath);
    }
}
