using Game.Mode.Stormworks.Tools.Swtpkg.Application.Options;
using Game.Mode.Stormworks.Tools.Swtpkg.Application.Validators;
using Lexicom.ConsoleApp.Amenities;
using Lexicom.ConsoleApp.Tui;
using Microsoft.Extensions.Options;

namespace Game.Mode.Stormworks.Tools.Swtpkg.ConsoleApp.Operations.Packaging;
[TuiPriority(PackagingPriority.ResetWorkingDirectory)]
[TuiPage("Packaging")]
[TuiTitle("Reset working directory")]
public class ResetWorkingDirectory : ITuiOperation
{
    private readonly IOptions<FilePathOptions> _filePathOptions;

    public ResetWorkingDirectory(IOptions<FilePathOptions> filePathOptions)
    {
        _filePathOptions = filePathOptions;
    }

    public Task ExecuteAsync()
    {
        FilePathOptions filePathOptions = _filePathOptions.Value;
        FilePathOptionsValidator.ThrowIfNull(filePathOptions.WorkingDirectoryPath);

        string workingDirectory = filePathOptions.WorkingDirectoryPath;

        Console.WriteLine($"Are you sure you want to reset the working directory?");
        Console.WriteLine($"{workingDirectory}");

        bool cancel = !Consolex
            .BinaryQuestion()
            .Ask($"All non-exported data will be lost");

        if (cancel)
        {
            return Task.CompletedTask;
        }

        if (!Directory.Exists(workingDirectory))
        {
            Console.WriteLine("The working directory does not exist.");
        }

        Directory.Delete(workingDirectory, true);
        Directory.CreateDirectory(workingDirectory);

        return Task.CompletedTask;
    }
}
