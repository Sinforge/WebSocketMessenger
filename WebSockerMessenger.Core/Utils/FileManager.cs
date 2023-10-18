using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text;

namespace WebSockerMessenger.Core.Utils
{
    public static class FileManager
    {
        private const string UPLOAD_PATH = "C:\\Users\\vladv\\source\\repos\\WebSocketMessenger\\Upload\\";
        // Returns string path
        public static string AddNewFile(string ecodedBase64File, string extention)
        {
            Guid fileId = Guid.NewGuid();
            string fileName = fileId.ToString() + extention;
            File.WriteAllBytesAsync(Path.Combine(UPLOAD_PATH, fileName) , Convert.FromBase64String(ecodedBase64File));
            return fileName;

        }

        public static void DeleteFile(string fileName)
        {
            string fullPath = Path.Combine(UPLOAD_PATH, fileName);
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);

            }
        }
        
    }
}
