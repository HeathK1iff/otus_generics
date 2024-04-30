using otus_generics.Utils.EventArgs;

namespace otus_generics.Utils;

public class FileScanner
{
    public string[] ScanFolder(string folder)
    {
        if (!Directory.Exists(folder))
        {
            throw new DirectoryNotFoundException();
        }

        var result = new List<string>();
        try
        {
            foreach (string fileName in Directory.GetFiles(folder))
            {
                var eventArgs = new FileFoundEventArgs()
                {
                    FileName = Path.GetFileName(fileName),
                    FilePath = Path.GetDirectoryName(fileName),
                    ForceStop = false
                };

                result.Add(fileName);

                OnFileFound(eventArgs);

                if (eventArgs.ForceStop)
                {
                    break;
                }
            }
        }
        finally
        {
            OnScanCompleted(new ScanCompletedEventArgs()
            {
                Handled = result.Count
            });
        }

        return result.ToArray();
    }

    protected virtual void OnScanCompleted(ScanCompletedEventArgs args)
    {
        ScanCompleted?.Invoke(this, args);
    }

    protected virtual void OnFileFound(FileFoundEventArgs args)
    {
        FileFound?.Invoke(this, args);
    }

    public event EventHandler<ScanCompletedEventArgs> ScanCompleted;

    public event EventHandler<FileFoundEventArgs> FileFound;
}