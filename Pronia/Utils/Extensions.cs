using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Pronia.Utils
{
    public  static class Extensions
    {
        public static  bool CheckFileType(this IFormFile file, string fileType)
        {
            return file.ContentType.Contains(fileType);


        }
        public static bool CheckFileSize (this IFormFile file, double size) {
            return file.Length/1024<size;
        }
    }
}
