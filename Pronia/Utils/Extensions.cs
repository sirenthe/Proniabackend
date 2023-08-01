using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Pronia.Utils
{
    public  static  class Extensions
    {
        public static bool  CheckFileType(this IFormFile image , string fileType)
        {
            return image.ContentType.Contains(fileType);
        }
    }
}
