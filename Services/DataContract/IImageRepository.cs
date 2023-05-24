using Microsoft.AspNetCore.Http;

namespace Services.DataContract;

public interface IImageRepository
{
    Task<string> SaveImage(IFormFile file, string path);
    Task DeleteImage(string? path);
}