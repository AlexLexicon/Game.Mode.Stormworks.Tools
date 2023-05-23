using Game.Mode.Stormworks.Tools.Swtpkg.Application.Models;
using Game.Mode.Stormworks.Tools.Swtpkg.Application.Options;
using Game.Mode.Stormworks.Tools.Swtpkg.Application.Validators;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Game.Mode.Stormworks.Tools.Swtpkg.Application.Services;
public interface IAddonService
{
    Task CopyToWorkingDirectoryAsync();
    Task<IReadOnlyList<AddonXml>> GetAddonsAsync();
}
public class AddonService : IAddonService
{
    private readonly ILogger<AddonService> _logger;
    private readonly IOptions<FilePathOptions> _filePathOptions;

    public AddonService(
        ILogger<AddonService> logger,
        IOptions<FilePathOptions> filePathOptions)
    {
        _logger = logger;   
        _filePathOptions = filePathOptions;
    }

    public Task CopyToWorkingDirectoryAsync()
    {
        FilePathOptions filePathOptions = _filePathOptions.Value;
        FilePathOptionsValidator.ThrowIfNull(filePathOptions.WorkingDirectoryPath);
        FilePathOptionsValidator.ThrowIfNull(filePathOptions.CopySourceDirectoryPath);
        
        if (!Directory.Exists(filePathOptions.WorkingDirectoryPath))
        {
            throw new Exception($"The working directory '{filePathOptions.WorkingDirectoryPath}' does not exist.");
        }

        if (!Directory.Exists(filePathOptions.CopySourceDirectoryPath))
        {
            throw new Exception($"The copy source directory '{filePathOptions.CopySourceDirectoryPath}' does not exist.");
        }

        _logger.LogInformation("Starting deep clone of the copy source directory '{copySourceDirectory}'.", filePathOptions.CopySourceDirectoryPath);

        DeepCloneDirectory(filePathOptions.CopySourceDirectoryPath, filePathOptions.WorkingDirectoryPath);

        _logger.LogInformation("Finished deep clone of the copy source directory '{copySourceDirectory}'.", filePathOptions.CopySourceDirectoryPath);

        return Task.CompletedTask;
    }

    //todo move to lexicom
    private void DeepCloneDirectory(string sourceDirectory, string targetDirectory)
    {
        var directory = new DirectoryInfo(sourceDirectory);
        if (!directory.Exists || !Directory.Exists(targetDirectory))
        {
            return;
        }

        _logger.LogInformation("Starting clone of '{directoryName}'", directory.Name);

        FileInfo[] files = directory.GetFiles();
        foreach (FileInfo file in files)
        {
            string targetFilePath = Path.Combine(targetDirectory, file.Name);

            _logger.LogInformation("Copying file '{fileName}'", file.Name);
            file.CopyTo(targetFilePath);
        }

        DirectoryInfo[] subDirectories = directory.GetDirectories();
        foreach (DirectoryInfo subDirectory in subDirectories)
        {
            string targetSubDirectoryPath = Path.Combine(targetDirectory, subDirectory.Name);

            Directory.CreateDirectory(targetSubDirectoryPath);

            DeepCloneDirectory(subDirectory.FullName, targetSubDirectoryPath);
        }
    }
}
