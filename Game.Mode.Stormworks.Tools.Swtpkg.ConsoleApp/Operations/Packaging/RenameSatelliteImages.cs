using Game.Mode.Stormworks.Tools.Swtpkg.Application.Services;
using Lexicom.ConsoleApp.Tui;

namespace Game.Mode.Stormworks.Tools.Swtpkg.ConsoleApp.Operations.Packaging;
[TuiPriority(PackagingPriority.RenameSatelliteImages)]
[TuiPage("Packaging")]
[TuiTitle("Rename Satellite images")]
public class RenameSatelliteImages : ITuiOperation
{
    private readonly IAddonImageService _addonImageService;

    public RenameSatelliteImages(IAddonImageService addonImageService)
    {
        _addonImageService = addonImageService;
    }

    public async Task ExecuteAsync()
    {
        await _addonImageService.RenameMapImagesAsync();
    }
}
