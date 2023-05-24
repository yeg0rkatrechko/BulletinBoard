using Microsoft.AspNetCore.Http;
using Services.DataContract;

namespace Dal.Repositories;

public class ImageRepository : IImageRepository
{
    public async Task<string> SaveImage(IFormFile file, string path)
    {
        if (file.Length == 0)
        {
            throw new Exception("File is empty");
        }

        await using var fileStream = new FileStream(path, FileMode.Create);
        await file.CopyToAsync(fileStream);

        return path;
    }

    public async Task DeleteImage(string? path)
    {
        if (File.Exists(path))
        {
            await Task.Run(() => File.Delete(path));
        }
    }
}