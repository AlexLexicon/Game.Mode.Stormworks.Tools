using Game.Mode.Stormworks.Tools.Swtpkg.Application.Models;
using Game.Mode.Stormworks.Tools.Swtpkg.Application.Options;
using Game.Mode.Stormworks.Tools.Swtpkg.Application.Validators;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Text.Json;

namespace Game.Mode.Stormworks.Tools.Swtpkg.Application.Services;
public interface IPackageDataService
{
    Task<PackageMetaData> CreateMetaDataAsync();
    Task SetMetaDataStateAsync(PackageMetaDataState state);
}
public class PackageDataService : IPackageDataService
{
    private readonly ILogger<PackageDataService> _logger;
    private readonly IOptions<FilePathOptions> _filePathOptions;
    private readonly IOptions<PackageSettingsOptions> _packageSettingsOptions;
    private readonly IOptions<PackagingOptions> _packagingOptions;

    public PackageDataService(
        ILogger<PackageDataService> logger,
        IOptions<FilePathOptions> filePathOptions,
        IOptions<PackageSettingsOptions> packageSettingsOptions,
        IOptions<PackagingOptions> packagingOptions)
    {
        _logger = logger;
        _filePathOptions = filePathOptions;
        _packageSettingsOptions = packageSettingsOptions;
        _packagingOptions = packagingOptions;
    }

    public async Task<PackageMetaData> CreateMetaDataAsync()
    {
        PackageSettingsOptions packageSettingsOptions = _packageSettingsOptions.Value;
        PackageSettingsOptionsValidator.ThrowIfNull(packageSettingsOptions.Version);

        PackagingOptions packagingOptions = _packagingOptions.Value;
        PackagingOptionsValidator.ThrowIfNull(packagingOptions.SourceByName);

        _logger.LogInformation("Retrieving the current Utc time.");
        //todo use ITimeProvider
        DateTimeOffset createdDateTimeUtc = DateTimeOffset.UtcNow;

        _logger.LogInformation("Creating the metadata.");
        var metaData = new PackageMetaData
        {
            GameVersion = "?",
            PackageVersion = packageSettingsOptions.Version,
            CreateDateTimeUtc = createdDateTimeUtc,
            CreatedBy = packagingOptions.SourceByName,
            ModifiedDateTimeUtc = createdDateTimeUtc,
            ModifiedBy = packagingOptions.SourceByName,
            State = new PackageMetaDataState
            {
                HasGameFiles = false,
                HasTileCoreData = false,
                HasTileMetaData = false,
                HasTilePositionData = false,
                HasTileWorkbenchData = false,
                IsZipped = false,
            },
        };

        _logger.LogInformation("Serilazing the metadata.");
        string metaDataFileText = JsonSerializer.Serialize(metaData, new JsonSerializerOptions
        {
            WriteIndented = true,
        });

        _logger.LogInformation("Getting the file path.");
        string filePath = GetFilePath();

        _logger.LogInformation("Writing the meta data to the file '{filePath}'.", filePath);
        await File.WriteAllTextAsync(filePath, metaDataFileText);

        return metaData;
    }

    public async Task SetMetaDataStateAsync(PackageMetaDataState state)
    {
        _logger.LogInformation("Getting the file path.");
        string filePath = GetFilePath();

        _logger.LogInformation("Reading the meta data from the file '{filePath}'.", filePath);
        string metaDataFileText = await File.ReadAllTextAsync(filePath);

        _logger.LogInformation("Deserializing the metadata.");
        PackageMetaData? metaData = JsonSerializer.Deserialize<PackageMetaData>(metaDataFileText);

        if (metaData is null)
        {
            throw new UnreachableException($"The deserialized meta data was null.");
        }

        _logger.LogInformation("Updating the meta data state.");
        metaData.State = state;

        _logger.LogInformation("Serilazing the meta data.");
        metaDataFileText =  JsonSerializer.Serialize(metaData, new JsonSerializerOptions
        {
            WriteIndented = true,
        });

        _logger.LogInformation("Writing the meta data to the file '{filePath}'.", filePath);
        await File.WriteAllTextAsync(filePath, metaDataFileText);
    }

    private string GetFilePath()
    {
        FilePathOptions filePathOptions = _filePathOptions.Value;
        FilePathOptionsValidator.ThrowIfNull(filePathOptions.WorkingDirectoryPath);

        if (!Directory.Exists(filePathOptions.WorkingDirectoryPath))
        {
            throw new Exception($"The working directory '{filePathOptions.WorkingDirectoryPath}' does not exist");
        }

        PackagingOptions packagingOptions = _packagingOptions.Value;
        PackagingOptionsValidator.ThrowIfNull(packagingOptions.PackageMetaDataFileName);

        string fileName = $"{packagingOptions.PackageMetaDataFileName}.json";
        string filePath = Path.Combine(filePathOptions.WorkingDirectoryPath, fileName);

        return filePath;
    }
}
