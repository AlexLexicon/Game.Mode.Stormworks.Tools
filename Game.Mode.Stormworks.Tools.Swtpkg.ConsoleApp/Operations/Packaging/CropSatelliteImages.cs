using Game.Mode.Stormworks.Tools.Swtpkg.Application.Services;
using Lexicom.ConsoleApp.Tui;

namespace Game.Mode.Stormworks.Tools.Swtpkg.ConsoleApp.Operations.Packaging;
[TuiPriority(PackagingPriority.CropSatelliteImages)]
[TuiPage("Packaging")]
[TuiTitle("Crop Satellite images")]
public class CropSatelliteImages : ITuiOperation
{
    private readonly IAddonImageService _addonImageService;

    public CropSatelliteImages(IAddonImageService addonImageService)
    {
        _addonImageService = addonImageService;
    }

    public async Task ExecuteAsync()
    {
        await _addonImageService.CropManualSatelliteImagesAsync();
    }
}
