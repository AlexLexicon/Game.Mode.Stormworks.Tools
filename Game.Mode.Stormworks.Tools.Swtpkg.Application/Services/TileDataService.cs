using Game.Mode.Stormworks.Tools.Swtpkg.Application.Models;
using Game.Mode.Stormworks.Tools.Swtpkg.Application.Options;
using Game.Mode.Stormworks.Tools.Swtpkg.Application.Validators;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Game.Mode.Stormworks.Tools.Swtpkg.Application.Services;
public interface ITileDataService
{
    Task CreateMetaDataAsync();
    Task CreateCoreDataAsync();
    Task CreateWorkbenchDataAsync();
    Task CreatePositionDataAsync();
}
public class TileDataService : ITileDataService
{
    private readonly ILogger<TileDataService> _logger;
    private readonly IAddonService _addonService;
    private readonly IGameService _gameService;
    private readonly IOptions<FilePathOptions> _filePathOptions;

    public TileDataService(
        ILogger<TileDataService> logger,
        IAddonService addonService,
        IGameService gameService,
        IOptions<FilePathOptions> filePathOptions)
    {
        _logger = logger;
        _addonService = addonService;
        _gameService = gameService;
        _filePathOptions = filePathOptions;
    }

    public Task CreateMetaDataAsync()
    {
        throw new NotImplementedException();
    }

    public async Task CreateCoreDataAsync()
    {
        FilePathOptions filePathOptions = _filePathOptions.Value;
        FilePathOptionsValidator.ThrowIfNull(filePathOptions.WorkingDirectoryPath);

        IReadOnlyList<AddonXml> addons = await _addonService.GetAddonsAsync();
        foreach (AddonXml addon in addons)
        {
            TileXml tile = await _gameService.GetTilesAsync(addon.TileFileName);

            await CreateCoreDataAsync(tile, addon);
        }
    }
    private Task CreateCoreDataAsync(TileXml tile, AddonXml addon)
    {
        PackageSettingsOptions packageSettingsOptions = _packageSettingsOptions.Value;
        PackageSettingsOptionsValidator.ThrowIfNull(packageSettingsOptions.Version);

        PackagingOptions packagingOptions = _packagingOptions.Value;
        PackagingOptionsValidator.ThrowIfNull(packagingOptions.SourceByName);

        _logger.LogInformation("Retrieving the current Utc time.");
        //todo use ITimeProvider
        DateTimeOffset createdDateTimeUtc = DateTimeOffset.UtcNow;

        _logger.LogInformation("Creating the core data.");
        var coreData = new TileCoreData
        {

        };

        _logger.LogInformation("Serilazing the core data.");
        string fileText = JsonSerializer.Serialize(coreData, new JsonSerializerOptions
        {
            WriteIndented = true,
        });

        _logger.LogInformation("Getting the file path.");
        string filePath = GetFilePath();

        _logger.LogInformation("Writing the core data to the file '{filePath}'.", filePath);
        await File.WriteAllTextAsync(filePath, fileText);

        return coreData;
    }

    public Task CreateWorkbenchDataAsync()
    {
        throw new NotImplementedException();
    }

    public Task CreatePositionDataAsync()
    {
        throw new NotImplementedException();
    }
}
