using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace wsredjurista.services
{
    public interface IFileProcessor
    {
         Task Start();
         void Stop();
         void Process(object source, FileSystemEventArgs e);
    }
}