using System.IO;
using System.Runtime.InteropServices;

namespace wsredjurista.services
{
    public interface IWatcher
    {
        NotifyFilters NotifyFilters {
            get;
            set;
        }

        string Path
        {
            get;
            set;
        }

        string Filter
        {
            get;
            set;
        }

        RenamedEventHandler Renamed
        {
            get;
            set;
        }
        void Start();
        void Stop();
    }
}