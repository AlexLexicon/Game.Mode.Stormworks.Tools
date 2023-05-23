using Lexicom.ConsoleApp.Tui;

namespace Game.Mode.Stormworks.Tools.Swtpkg.ConsoleApp.Operations.Packaging;
[TuiPriority(PackagingPriority.GenerateTileMetaData)]
[TuiPage("Packaging")]
[TuiTitle("Generate tile meta data (.meta.data.json)")]
public class GenerateTileMetadata : ITuiOperation
{
    public Task ExecuteAsync()
    {
        throw new NotImplementedException();
    }
}
