namespace Commerce.Command.Contract.Abstractions
{
    /// <summary>
    /// Interface provide method handle upload file
    /// </summary>
    public interface IFileService
    {
        /// <summary>
        /// Upload file to asset server
        /// </summary>
        /// <param name="fileName">Name of file</param>
        /// <param name="base64String">File content in format of base64 string</param>
        /// <param name="relativePath">File relative path stored in asset server</param>
        /// <returns>Relative path where file locate in asset server</returns>
        Task<string> UploadFile(string fileName, string base64String, string relativePath);
    }
}