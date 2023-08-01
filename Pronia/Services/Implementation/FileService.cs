using Pronia.Areas.Admin.ViewModels.FeatureViewModels;
using Pronia.Exceptions;
using Pronia.Services.Interfaces;
using Pronia.Utils;

namespace Pronia.Services.Implementation
{
    public class FileService : IFileService
    {
        private object updateFeatureViewModel;

        public async Task<string> CreateFileAsync(IFormFile image, string path)
        {
            if (!image.CheckFileType("image/"))
            {
                throw new FileTypeException("sekil add edin");
            }
            string filename = $"{Guid.NewGuid()}-{image.FileName}";
            string resultPath = Path.Combine(path, filename);
            using (FileStream fileStream = new FileStream(resultPath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }
            return filename;
        }

        public void DeleteFile(string path)
        {

            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }

        }
    }
}
