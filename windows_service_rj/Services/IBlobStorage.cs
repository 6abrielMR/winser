using System.IO;

namespace wsredjurista.services
{
    public interface IBlobStorage
    {
         void Start();
         void Stop();
         void UploadAsync(FileStream uploadFileStream, string filename);
         void DeleteAsync(string[] filesToDelete);
    }
}