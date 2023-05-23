using Lexicom.ConsoleApp.Tui;

namespace Game.Mode.Stormworks.Tools.Swtpkg.ConsoleApp.Operations.Packaging;
[TuiPriority(PackagingPriority.ZipPackage)]
[TuiPage("Packaging")]
[TuiTitle("Zip package")]
public class ZipPackage : ITuiOperation
{
    public Task ExecuteAsync()
    {
        throw new NotImplementedException();
    }
}
