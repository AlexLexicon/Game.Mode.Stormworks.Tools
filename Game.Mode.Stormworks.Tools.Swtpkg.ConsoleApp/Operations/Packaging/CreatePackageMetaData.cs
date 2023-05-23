using Game.Mode.Stormworks.Tools.Swtpkg.Application.Models;
using Game.Mode.Stormworks.Tools.Swtpkg.Application.Services;
using Lexicom.ConsoleApp.Amenities;
using Lexicom.ConsoleApp.Tui;

namespace Game.Mode.Stormworks.Tools.Swtpkg.ConsoleApp.Operations.Packaging;
[TuiPriority(PackagingPriority.CreatePackageMetaData)]
[TuiPage("Packaging")]
[TuiTitle("Create package meta data (swtpkg.json)")]
public class CreatePackageMetaData : ITuiOperation
{
    private readonly IPackageDataService _packageMetaDataService;

    public CreatePackageMetaData(IPackageDataService packageMetaDataService)
    {
        _packageMetaDataService = packageMetaDataService;
    }

    public async Task ExecuteAsync()
    {
        PackageMetaData metaData = await _packageMetaDataService.CreateMetaDataAsync();

        Consolex.WriteAsJsonWithType(metaData);
    }
}
