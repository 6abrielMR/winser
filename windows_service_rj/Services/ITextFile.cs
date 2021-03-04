namespace wsredjurista.services
{
    public interface ITextFile
    {
        string[] Lines
        {
            get;
            set;
        }
         void Read(string path);
    }
}