using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace ProductService.Application.Services
{
    /// <summary>
    /// Сервис по работе с изображениями.
    /// </summary>
    public class ImageService
    {
        public async Task<string> HandleImageAsync(IFormFile file, string saveDirectory, CancellationToken cancellation)
        {
            if (file == null)
            {
                throw new ArgumentNullException("File is empty");
            }

            var fileExtension = Path.GetExtension(file.FileName);
            var fileName = $"{Guid.NewGuid()}{fileExtension}";
            var savePath = Path.Combine(saveDirectory, fileName);

            Directory.CreateDirectory(saveDirectory);

            using var stream = file.OpenReadStream();

            using var image = await Image.LoadAsync(stream, cancellation);

            if(image.Width > 1000)
            {
                image.Mutate(x => x.Resize(1000, 0));
            }

            await image.SaveAsync(savePath, cancellation);

            return savePath;
        }

        public void DeleteImage(string imagePath)
        {
            if (File.Exists(imagePath))
            {
                File.Delete(imagePath);
            }
        }
    }
}
