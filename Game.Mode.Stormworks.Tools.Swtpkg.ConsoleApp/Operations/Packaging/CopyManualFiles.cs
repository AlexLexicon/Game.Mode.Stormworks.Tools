using Game.Mode.Stormworks.Tools.Swtpkg.Application.Services;
using Lexicom.ConsoleApp.Tui;

namespace Game.Mode.Stormworks.Tools.Swtpkg.ConsoleApp.Operations.Packaging;
[TuiPriority(PackagingPriority.CopyManualFiles)]
[TuiPage("Packaging")]
[TuiTitle("Copy manual files")]
public class CopyManualFiles : ITuiOperation
{
    private readonly IAddonService _addonService;

    public CopyManualFiles(IAddonService addonService)
    {
        _addonService = addonService;
    }

    public async Task ExecuteAsync()
    {
        await _addonService.CopyToWorkingDirectoryAsync();
    }
}
