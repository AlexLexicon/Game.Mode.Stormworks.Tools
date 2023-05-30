using Game.Mode.Stormworks.Tools.Swtpkg.Application.Services;
using Lexicom.ConsoleApp.Tui;

namespace Game.Mode.Stormworks.Tools.Swtpkg.ConsoleApp.Operations.Packaging;
[TuiPriority(PackagingPriority.GenerateTileMetaData)]
[TuiPage("Packaging")]
[TuiTitle("Generate tile meta data (.meta.data.json)")]
public class GenerateTileMetadata : ITuiOperation
{
    private readonly ITileDataService _tileDataService;

    public GenerateTileMetadata(ITileDataService tileDataService)
    {
        _tileDataService = tileDataService;
    }

    public async Task ExecuteAsync()
    {
        await _tileDataService.CreateMetaDataAsync();
    }
}
