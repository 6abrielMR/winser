namespace wsredjurista.services
{
    public interface IServiceConfig
    {
        string defaultConnection
        {
            get;
            set;
        }
        string containerName
        {
            get;
            set;
        }
        string inputFolder
        {
            get;
            set;
        }
        string filesProcessed
        {
            get;
            set;
        }
        string filesToDelete
        {
            get;
            set;
        }
        string filter
        {
            get;
            set;
        }
    }
}