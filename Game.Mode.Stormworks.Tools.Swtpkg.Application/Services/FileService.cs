using Game.Mode.Stormworks.Tools.Swtpkg.Application.Exceptions;
using Game.Mode.Stormworks.Tools.Swtpkg.Application.Options;
using Game.Mode.Stormworks.Tools.Swtpkg.Application.Validators;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Game.Mode.Stormworks.Tools.Swtpkg.Application.Services;
public interface IFileService
{
    Task<IReadOnlyList<FileInfo>> GetMapImageFilesAsync();
    Task CreateJsonFileAsync<T>(T jsonBody, string fileNameWithoutExtension) where T : class;
}
public class FileService : IFileService
{
    private readonly IOptions<FilePathOptions> _filePathOptions;
    private readonly IOptions<PackagingOptions> _packagingOptions;

    public FileService(
        IOptions<FilePathOptions> filePathOptions,
        IOptions<PackagingOptions> packagingOptions)
    {
        _filePathOptions = filePathOptions;
        _packagingOptions = packagingOptions;
    }

    public Task<IReadOnlyList<FileInfo>> GetMapImageFilesAsync()
    {
        PackagingOptions packagingOptions = _packagingOptions.Value;
        PackagingOptionsValidator.ThrowIfNull(packagingOptions.MapImageExtension);

        DirectoryInfo directory = GetWorkingDirectory();

        List<FileInfo> files = directory
            .GetFiles()
            .Where(fi => string.Equals(fi.Extension, packagingOptions.MapImageExtension, StringComparison.OrdinalIgnoreCase))
            .ToList();

        return Task.FromResult<IReadOnlyList<FileInfo>>(files);
    }

    public async Task CreateJsonFileAsync<T>(T jsonBody, string fileNameWithoutExtension) where T : class
    {
        string metaDataFileText = JsonSerializer.Serialize(jsonBody, new JsonSerializerOptions
        {
            WriteIndented = true,
        });

        DirectoryInfo directory = GetWorkingDirectory();

        string filePath = Path.Combine(directory.FullName, $"{fileNameWithoutExtension}.json");

        await File.WriteAllTextAsync(filePath, metaDataFileText);
    }

    private DirectoryInfo GetWorkingDirectory()
    {
        FilePathOptions filePathOptions = _filePathOptions.Value;
        FilePathOptionsValidator.ThrowIfNull(filePathOptions.WorkingDirectoryPath);

        var directory = new DirectoryInfo(filePathOptions.WorkingDirectoryPath);

        if (!directory.Exists)
        {
            throw new WorkingDirectoryDoesNotExistException(directory.FullName);
        }

        return directory;
    }
}
