using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;

namespace WebSocketMessenger.Infrastructure.FileSystem
{
    public static class FileManager
    {
        private static string UPLOAD_PATH;
        private static  PhysicalFileProvider _physicalFileProvider ;


        static FileManager()
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            UPLOAD_PATH = configuration["UploadPath"]; // Assuming "UploadPath" key exists in appsettings.json
            _physicalFileProvider = new PhysicalFileProvider(UPLOAD_PATH);
        }       
        // Returns string path
        public static string AddNewFile(string ecodedBase64File, string extention)
        {
            Guid fileId = Guid.NewGuid();
            string fileName = fileId.ToString() + extention;
            // Split the string to get the Base64 encoded data part
            string[] parts = ecodedBase64File.Split(',');
            string base64Data = parts.Length > 1 ? parts[1] : "";

            File.WriteAllBytesAsync(Path.Combine(UPLOAD_PATH, fileName), Convert.FromBase64String(base64Data));
            return fileName;

        }

        public static FileStream CreateFileStream(string fileName)
            => new FileStream(Path.Combine(UPLOAD_PATH, fileName), FileMode.Create);

        public static void DeleteFile(string fileName)
        {
            string fullPath = Path.Combine(UPLOAD_PATH, fileName);
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);

            }
        }
        public static string GetBase64StringByFileName(string fileName)
        {
            string fullPath = Path.Combine(UPLOAD_PATH, fileName);
            if (File.Exists(fullPath))
            {
                return Convert.ToBase64String(File.ReadAllBytes(fullPath));
            }
            return "unknown file";
        }
        
        public static IFileInfo GetFileByFileName(string fileName)
        {
            string fullPath = Path.Combine(UPLOAD_PATH, fileName);
            if (File.Exists(fullPath))
            {
                return _physicalFileProvider.GetFileInfo(fileName);
            }
            return null;
        }

    }
}
