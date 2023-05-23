using Lexicom.ConsoleApp.Tui;

namespace Game.Mode.Stormworks.Tools.Swtpkg.ConsoleApp.Operations.Packaging;
[TuiPriority(PackagingPriority.CopyGameFiles)]
[TuiPage("Packaging")]
[TuiTitle("Copy game files")]
public class CopyGameFiles : ITuiOperation
{
    public Task ExecuteAsync()
    {
        throw new NotImplementedException();
    }
}
