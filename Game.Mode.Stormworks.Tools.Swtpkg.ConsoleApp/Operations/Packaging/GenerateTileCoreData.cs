using Game.Mode.Stormworks.Tools.Swtpkg.Application.Services;
using Lexicom.ConsoleApp.Tui;

namespace Game.Mode.Stormworks.Tools.Swtpkg.ConsoleApp.Operations.Packaging;
[TuiPriority(PackagingPriority.GenerateTileCoreData)]
[TuiPage("Packaging")]
[TuiTitle("Generate tile core data (.core.data.json)")]
public class GenerateTileCoreData : ITuiOperation
{
    private readonly ITileDataService _tileDataService;

    public GenerateTileCoreData(ITileDataService tileDataService)
    {
        _tileDataService = tileDataService;
    }

    public async Task ExecuteAsync()
    {
        await _tileDataService.CreateCoreDataAsync();
    }
}
