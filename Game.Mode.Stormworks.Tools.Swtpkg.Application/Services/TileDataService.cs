using Game.Mode.Stormworks.Tools.Swtpkg.Application.Models;
using Game.Mode.Stormworks.Tools.Swtpkg.Application.Options;
using Game.Mode.Stormworks.Tools.Swtpkg.Application.Validators;
using Lexicom.DependencyInjection.Primitives;
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
    private readonly ITimeProvider _timeProvider;
    private readonly IOptions<FilePathOptions> _filePathOptions;
    private readonly IOptions<PackagingOptions> _packagingOptions;
    private readonly IOptions<PackageSettingsOptions> _packageSettingsOptions;

    public TileDataService(
        ILogger<TileDataService> logger,
        IAddonService addonService,
        IGameService gameService,
        IOptions<FilePathOptions> filePathOptions,
        IOptions<PackagingOptions> packagingOptions,
        ITimeProvider timeProvider,
        IOptions<PackageSettingsOptions> packageSettingsOptions)
    {
        _logger = logger;
        _addonService = addonService;
        _gameService = gameService;
        _filePathOptions = filePathOptions;
        _packagingOptions = packagingOptions;
        _timeProvider = timeProvider;
        _packageSettingsOptions = packageSettingsOptions;
    }

    public async Task CreateMetaDataAsync()
    {
        FilePathOptions filePathOptions = _filePathOptions.Value;
        FilePathOptionsValidator.ThrowIfNull(filePathOptions.WorkingDirectoryPath);

        IReadOnlyList<AddonXml> addons = await _addonService.GetAddonsAsync();
        foreach (AddonXml addon in addons)
        {
            await CreateMetaDataAsync(addon);
        }
    }
    private async Task CreateMetaDataAsync(AddonXml addon)
    {
        FilePathOptions filePathOptions = _filePathOptions.Value;
        FilePathOptionsValidator.ThrowIfNull(filePathOptions.WorkingDirectoryPath);

        PackagingOptions packagingOptions = _packagingOptions.Value;
        PackagingOptionsValidator.ThrowIfNull(packagingOptions.SourceByName);

        PackageSettingsOptions packageSettingsOptions = _packageSettingsOptions.Value;
        PackageSettingsOptionsValidator.ThrowIfNull(packageSettingsOptions.Version);

        _logger.LogInformation("Retrieving the current Utc time.");
        DateTimeOffset createdDateTimeUtc = _timeProvider.UtcNow;

        _logger.LogInformation("Creating the meta data.");
        var metaData = new TileMetaData
        {
            PackageVersion = packageSettingsOptions.Version,
            GameVersion = "?",
            CreatedDateTimeUtc = createdDateTimeUtc,
            CreatedBy = packagingOptions.SourceByName,
            ModifiedDateTimeUtc = createdDateTimeUtc,
            ModifiedBy = packagingOptions.SourceByName,
        };

        _logger.LogInformation("Serilazing the core data.");
        string fileText = JsonSerializer.Serialize(metaData, new JsonSerializerOptions
        {
            WriteIndented = true,
        });

        string normalizedFileName = $"{addon.TileName.ToLowerInvariant()}.meta.data.json";

        _logger.LogInformation("Getting the file path.");
        string filePath = Path.Combine(filePathOptions.WorkingDirectoryPath, normalizedFileName);

        _logger.LogInformation("Writing the core data to the file '{filePath}'.", filePath);
        await File.WriteAllTextAsync(filePath, fileText);
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
    private async Task CreateCoreDataAsync(TileXml tile, AddonXml addon)
    {
        FilePathOptions filePathOptions = _filePathOptions.Value;
        FilePathOptionsValidator.ThrowIfNull(filePathOptions.WorkingDirectoryPath);

        _logger.LogInformation("Creating the core data.");
        var coreData = new TileCoreData
        {
            TileName = addon.TileName,
            IsIsland = tile.IsIsland,
            IsPurchasable = tile.IsPurchasable,
            PurchaseCost = tile.PurchaseCost,
        };

        _logger.LogInformation("Serilazing the core data.");
        string fileText = JsonSerializer.Serialize(coreData, new JsonSerializerOptions
        {
            WriteIndented = true,
        });

        string normalizedFileName = $"{addon.TileName.ToLowerInvariant()}.core.data.json";

        _logger.LogInformation("Getting the file path.");
        string filePath = Path.Combine(filePathOptions.WorkingDirectoryPath, normalizedFileName);

        _logger.LogInformation("Writing the core data to the file '{filePath}'.", filePath);
        await File.WriteAllTextAsync(filePath, fileText);

        //todo update modified metadata for tile
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
