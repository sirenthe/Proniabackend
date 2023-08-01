namespace Pronia.Services.Interfaces
{
    public interface IFileService
    {
        Task<string> CreateFileAsync(IFormFile image, string path);
            void DeleteFile(string path);
    }

}
