﻿using Lexicom.ConsoleApp.Tui;

namespace Game.Mode.Stormworks.Tools.Swtpkg.ConsoleApp.Operations.Packaging;
[TuiPriority(PackagingPriority.GenerateTileCoreData)]
[TuiPage("Packaging")]
[TuiTitle("Populate tile core data (.core.data.json)")]
public class PopulateTileCoreData : ITuiOperation
{
    public Task ExecuteAsync()
    {
        throw new NotImplementedException();
    }
}
