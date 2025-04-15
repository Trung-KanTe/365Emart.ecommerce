using Commerce.Command.Contract.Abstractions;
using System.Text.RegularExpressions;

namespace Commerce.Command.Contract.Services
{
    /// <summary>
    /// Implementation for handling file upload
    /// </summary>
    public class FileService : IFileService
    {
        private readonly string _rootPath = @"E:\KhoaLuan\365Emart.Commerce\src\Commerce\Query\Commerce.Query.API\UploadFile\Image";

        public FileService()
        {
            if (!Directory.Exists(_rootPath))
            {
                Directory.CreateDirectory(_rootPath);
            }
        }

        /// <inheritdoc />
        public async Task<string> UploadFile(string fileName, string base64String, string relativePath)
        {
            if (string.IsNullOrWhiteSpace(fileName) || string.IsNullOrWhiteSpace(base64String))
            {
                throw new ArgumentException("Invalid file name or content.");
            }

            var base64Data = Regex.Replace(base64String, @"^data:image\/[a-z]+;base64,", string.Empty);

            byte[] fileBytes;
            try
            {
                fileBytes = Convert.FromBase64String(base64Data);
            }
            catch (FormatException)
            {
                throw new InvalidDataException("Invalid Base64 string.");
            }

            string timeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            string newFileName = $"{fileName}_{timeStamp}.png";

            string directoryPath = Path.Combine(_rootPath, relativePath);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            string filePath = Path.Combine(directoryPath, newFileName);

            await File.WriteAllBytesAsync(filePath, fileBytes);

            string relativeFilePath = Path.Combine(relativePath, newFileName).Replace("\\", "/");
            return relativeFilePath;
        }
    }
}