using Game.Mode.Stormworks.Tools.Swtpkg.Application.Models;
using Game.Mode.Stormworks.Tools.Swtpkg.Application.Services;
using Lexicom.ConsoleApp.Tui;

namespace Game.Mode.Stormworks.Tools.Swtpkg.ConsoleApp.Operations.Packaging;
[TuiPriority(PackagingPriority.CopyGameFiles)]
[TuiPage("Packaging")]
[TuiTitle("Copy game files")]
public class CopyGameFiles : ITuiOperation
{
    private readonly IAddonService _addonService;
    private readonly IGameService _gameService;

    public CopyGameFiles(
        IAddonService addonService, 
        IGameService gameService)
    {
        _addonService = addonService;
        _gameService = gameService;
    }

    public async Task ExecuteAsync()
    {
        IReadOnlyList<AddonXml> addons = await _addonService.GetAddonsAsync();

        IEnumerable<string> tileFileNames = addons
            .Select(a => a.TileFileName)
            .Where(tfn => !string.IsNullOrWhiteSpace(tfn));

        await _gameService.CopyTilesXmlToWorkingDirectoryAsync(tileFileNames);
    }
}
