using Lexicom.ConsoleApp.Tui;

namespace Game.Mode.Stormworks.Tools.Swtpkg.ConsoleApp.Operations.Packaging;
[TuiPriority(PackagingPriority.GenerateTilePositionData)]
[TuiPage("Packaging")]
[TuiTitle("Generate tile position data (.position.data.json)")]
public class GenerateTilePositionData : ITuiOperation
{
    public Task ExecuteAsync()
    {
        throw new NotImplementedException();
    }
}
