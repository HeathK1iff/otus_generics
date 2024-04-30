using otus_generics.Utils.EventArgs;

namespace otus_generics.Utils;


public delegate void EventHandlerInterrupted<TEventArgs>(object? sender, TEventArgs e, out bool forceStop);

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
                    FilePath = Path.GetDirectoryName(fileName)
                };

                result.Add(fileName);

                OnFileFound(eventArgs, out bool forceStop);

                if (forceStop)
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
    
    protected virtual void OnFileFound(FileFoundEventArgs args, out bool forceStop)
    {
        forceStop = false;
        FileFound?.Invoke(this, args, out forceStop);
    }

    public event EventHandler<ScanCompletedEventArgs> ScanCompleted;

    public event EventHandlerInterrupted<FileFoundEventArgs> FileFound;
}