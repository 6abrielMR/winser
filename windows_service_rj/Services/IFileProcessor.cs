using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace wsredjurista.services
{
    public interface IFileProcessor
    {
         Task Start();
         void Stop();
         void Process(object sender, ElapsedEventArgs e);
    }
}