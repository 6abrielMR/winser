using System.IO;

namespace wsredjurista.services
{
    public interface IBlobStorage
    {
         void Start();
         void Stop();
         void UploadAsync();
         void DeleteAsync(string[] filesToDelete);
    }
}