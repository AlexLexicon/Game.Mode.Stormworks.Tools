using Game.Mode.Stormworks.Tools.Swtpkg.Application.Options;
using Game.Mode.Stormworks.Tools.Swtpkg.Application.Validators;
using Lexicom.ConsoleApp.Amenities;
using Lexicom.ConsoleApp.Tui;
using Microsoft.Extensions.Options;

namespace Game.Mode.Stormworks.Tools.Swtpkg.ConsoleApp.Operations.Packaging;
[TuiPriority(PackagingPriority.CleanPackage)]
[TuiPage("Packaging")]
[TuiTitle("Clean Package")]
public class CleanPackage : ITuiOperation
{
    private readonly IOptions<FilePathOptions> _filePathOptions;

    public CleanPackage(IOptions<FilePathOptions> filePathOptions)
    {
        _filePathOptions = filePathOptions;
    }

    public Task ExecuteAsync()
    {
        FilePathOptions filePathOptions = _filePathOptions.Value;
        FilePathOptionsValidator.ThrowIfNull(filePathOptions.WorkingDirectoryPath);

        var workingDirectory = new DirectoryInfo(filePathOptions.WorkingDirectoryPath);

        Console.WriteLine($"Everything inside the working directory '{filePathOptions.WorkingDirectoryPath}' will be deleted.");
        Consolex
            .BinaryQuestion()
            .Ask($"Are you sure you want to continue?");

        return Task.CompletedTask;
    }

    public static long RecursiveSizeInBytes(DirectoryInfo directoryInfo)
    {
        ArgumentNullException.ThrowIfNull(directoryInfo);

        long size = 0;

        FileInfo[] files = directoryInfo.GetFiles();
        foreach (FileInfo file in files)
        {
            size += file.Length;
        }

        DirectoryInfo[] directories = directoryInfo.GetDirectories();
        foreach (DirectoryInfo directory in directories)
        {
            size += RecursiveSizeInBytes(directory);
        }

        return size;
    }
}
