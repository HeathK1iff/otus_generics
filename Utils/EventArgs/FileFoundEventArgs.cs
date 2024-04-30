namespace otus_generics.Utils.EventArgs;

public class FileFoundEventArgs : System.EventArgs
{
    public string FileName { get; set; }
    public string FilePath { get; set; }
}
