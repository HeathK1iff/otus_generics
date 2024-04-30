using otus_generics.Entities;
using otus_generics.Utils;
using otus_generics.Extensions;
using Microsoft.Extensions.Configuration;
using otus_generics.Utils.EventArgs;

namespace otus_generics;

internal class Program
{
    public const string QuitKey = "Q";
    public const string ConfigurationFileName = "app.ini";

    private readonly IConfiguration _configuration;
    
    private static void Main(string[] args)
    {
        var config = new ConfigurationBuilder()
            .AddIniFile(ConfigurationFileName).Build();

        var program = new Program(config);
        program.Run();
    }


    public Program(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Run()
    {
        PrintWelcomeMessage();
        PrinSelectionMessage();

        string answer = Ask();
        do
        {
            if (IsQuit(answer))
            {
                break;
            }

            if (int.TryParse(answer, out int answerIndex))
            {
                switch (answerIndex)
                {
                    case 1:
                        GenericExtentionAction();
                        break;
                    case 2:
                        FileScannerAction();
                        break;
                }
            }

            answer = Ask();
        } while (true);

    }

    public void PrintWelcomeMessage()
    {
        Console.WriteLine("Otus Generic program");
    }

    public void PrinSelectionMessage()
    {
        Console.WriteLine("Please select:");
        Console.WriteLine("1. Extension GetMax<T> for Enumeration");
        Console.WriteLine("2. Scan folder with possibility to interrupt");
    }

    public bool IsQuit(string ask)
    {
        return ask.Equals(QuitKey, StringComparison.InvariantCultureIgnoreCase);
    }

    public string Ask()
    {
        return Ask("Input: ");
    }

    public string Ask(string promt)
    {
        Console.Write(promt);
        return Console.ReadLine();
    }

    public void GenericExtentionAction()
    {
        var persons = new Person[] {
            new Person { Name = "Peter", Age = 24.3f },
            new Person { Name = "Anna", Age = 23.5f },
            new Person { Name = "Katrin", Age = 53.4f },
            new Person { Name = "Jacob", Age = 3.2f },
            new Person { Name = "Miranda", Age = 12.5f },
            new Person { Name = "Philip", Age = 0.5f }
        };

        var maxAgePerson = persons.GetMax<Person>(item =>
        {
            Console.WriteLine($"Check age for {item.Name}: {item.Age}");
            return item.Age;
        });

        Console.WriteLine($"Oldest person is {maxAgePerson.Name} ({maxAgePerson.Age})");
    }


    public void FileScannerAction()
    {
         string folderPath = _configuration["Folder"] ?? string.Empty;

        if (string.IsNullOrWhiteSpace(folderPath))
        {
            throw new ArgumentException("Folder key is not found in App.ini");
        }

        var scanner = new FileScanner();
        
        scanner.ScanCompleted += (sender, args) =>
        {
            Console.WriteLine($"Scan completed. Handled: {args.Handled}");      
        };

        scanner.FileFound += FileFoundHandler;

        Console.WriteLine($"Scanning: {folderPath}");
        scanner.ScanFolder(folderPath);
    }

    public void FileFoundHandler(object? sender, FileFoundEventArgs args, out bool forceStop)
    {
        Console.WriteLine($"Found file {args.FileName}");
        string answer = Ask(" Do you want to interrupt scan (Yes(y), Default(n)):");
        forceStop = answer.Equals("Y", StringComparison.InvariantCultureIgnoreCase);
    }
}
