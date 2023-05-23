using Lexicom.ConsoleApp.Tui;

namespace Game.Mode.Stormworks.Tools.Swtpkg.ConsoleApp.Operations;
[TuiPriority(0)]
[TuiTitle("Read Me")]
public class ReadMe : ITuiOperation
{
    public Task ExecuteAsync()
    {
        Console.WriteLine("""
        This app will allow you to create a '.swtpkg' file

        A '.swtpkg' file or Stormworks tile package is a single file containing an abundance of data about the Stormworks game tiles.
        
        In order to create a '.swtpkg' file several 'passes' must take place. Think of this like drawing a picture where first you draw an outline, then add some details before finally shading the peice. These steps could be called passes.
        These passes must be done one at a time in order however some passes will be optional. Additionally there may be some manually steps to perform during some passes.
        """);

        return Task.CompletedTask;
    }
}
