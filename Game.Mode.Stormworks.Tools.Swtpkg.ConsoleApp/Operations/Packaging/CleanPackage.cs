using Game.Mode.Stormworks.Tools.Swtpkg.Application.Options;
using Game.Mode.Stormworks.Tools.Swtpkg.Application.Validators;
using Lexicom.ConsoleApp.Amenities;
using Lexicom.ConsoleApp.Tui;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Game.Mode.Stormworks.Tools.Swtpkg.ConsoleApp.Operations.Packaging;
[TuiPriority(PackagingPriority.CleanPackage)]
[TuiPage("Packaging")]
[TuiTitle("Clean Package")]
public class CleanPackage : ITuiOperation
{
    private readonly ILogger<CleanPackage> _logger;
    private readonly IOptions<FilePathOptions> _filePathOptions;
    private readonly IOptions<PackagingOptions> _packagingOptions;

    public CleanPackage(IOptions<FilePathOptions> filePathOptions, IOptions<PackagingOptions> packagingOptions, ILogger<CleanPackage> logger)
    {
        _filePathOptions = filePathOptions;
        _packagingOptions = packagingOptions;
        _logger = logger;
    }

    public Task ExecuteAsync()
    {
        FilePathOptions filePathOptions = _filePathOptions.Value;
        FilePathOptionsValidator.ThrowIfNull(filePathOptions.WorkingDirectoryPath);

        PackagingOptions packagingOptions = _packagingOptions.Value;
        PackagingOptionsValidator.ThrowIfNull(packagingOptions.CleanAllowedExtensions);

        string[] allowedExtensions = packagingOptions.CleanAllowedExtensions.Split(',');

        var directory = new DirectoryInfo(filePathOptions.WorkingDirectoryPath);

        foreach (var file in directory.GetFiles())
        {
            if (!allowedExtensions.Contains(file.Extension))
            {
                _logger.LogInformation("The file '{file}' is being deleted", file.FullName);
                file.Delete();
            }
        }

        foreach(var subDirectory in directory.GetDirectories())
        {
            _logger.LogInformation("The sub directory '{subDirectory}' is being deleted", subDirectory.FullName);

            long size = RecursiveSizeInBytes(subDirectory);

            long gigabyte = 1000000000;
            if (size > gigabyte)
            {
                throw new Exception($"The sub folder '{subDirectory.FullName}' size '{size}' was greater than '{gigabyte}'.");
            }

            DeepDeleteDirectory(subDirectory.FullName);
        }

        return Task.CompletedTask;
    }

    private void DeepDeleteDirectory(string directoryToDelete)
    {
        var directory = new DirectoryInfo(directoryToDelete);

        _logger.LogInformation("Starting delete of '{directoryName}'", directory.Name);

        FileInfo[] files = directory.GetFiles();
        foreach (FileInfo file in files)
        {
            _logger.LogInformation("Deleting file '{fileName}'", file.Name);
            file.Delete();
        }

        DirectoryInfo[] subDirectories = directory.GetDirectories();
        foreach (DirectoryInfo subDirectory in subDirectories)
        {
            DeepDeleteDirectory(subDirectory.FullName);
        }

        directory.Delete();
    }
    //todo move to lexicom (add maximum size limit to break early
    public static long RecursiveSizeInBytes(DirectoryInfo directoryInfo, long? maximumSize = null)
    {
        ArgumentNullException.ThrowIfNull(directoryInfo);

        long size = 0;

        FileInfo[] files = directoryInfo.GetFiles();
        foreach (FileInfo file in files)
        {
            size += file.Length;
            
            if (maximumSize is not null && size > maximumSize)
            {
                return maximumSize.Value;
            }
        }

        DirectoryInfo[] directories = directoryInfo.GetDirectories();
        foreach (DirectoryInfo directory in directories)
        {
            size += RecursiveSizeInBytes(directory, maximumSize);
            
            if (maximumSize is not null && size > maximumSize)
            {
                return maximumSize.Value;
            }
        }

        return size;
    }
}
