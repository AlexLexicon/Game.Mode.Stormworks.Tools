using Game.Mode.Stormworks.Tools.Swtpkg.Application.Options;
using Lexicom.ConsoleApp.Tui;

namespace Game.Mode.Stormworks.Tools.swtpkg.ConsoleApp.Operations;
[TuiTitle("Pass 0: Manually create missions for tiles")]
public class ManuallyCreateMissionsForTiles : ITuiOperation
{
    public Task ExecuteAsync()
    {
        Console.Write($"""
        By the end of this pass you will have created mission addons for each tile you will eventually package into the final '.swtpkg' file. Although this pass is labeled 'Manual', you may want to use an external tool to automate it's steps if you plan to package many tiles. The following steps will walk you through the exact process for creating these tile addon missions.
        
         1. Open stormworks game
         2. Click 'New Game'
         3. Choose 'Custom' from the game mode drop down
         4. Click 'Confirm' to create the new game
         5. Click 'Complete' on the character creator
         6. Open the pause menu (commonly with the 'esc' key)
         7. Click 'Addon Editor'
         8. Click 'NEW ADDON'
         9. Click 'Confirm' to create the new addon
        10. Click 'Select a location to mod'

        [Optional steps to include satellite tile image]
          10.1. Take a screen shot of the satellite image of the tile you are about to pick
                - The screen shot should only contain a single satellite image of one tile however a future pass will handle cropping for this image so an exact capture is not nessasary
          10.2. Save this screen shot as a '.png' to the missions directory where this addon will eventually be saved. (Commonly 'C:\Users\YOUR_USER\AppData\Roaming\Stormworks\data\missions')
          10.3. Give the screen shot file a name that will match what you will eventually name the addon mission. (For example if you will name the addon mission 'StarterBaseAddon1' then the satellite image file should be 'StarterBaseAddon1.png')
                      
        11. Click on the tile from the list you want to package into the '.swtpkg' file
        12. Click 'Save' to save the addon mission
        13. Click on the 'plus' button on the 'Saved' tab of the save menu
        14. Enter the name for the addon you want to package into the '.swtpkg' file 
            - This name will not matter for the rest of the passes and will not be used within the final '.swtpkg' file
            - If you took a screen shot of the satellite image of the tile for this mission make sure this name matches the name of that screen shot
            
        [Repeat steps 8-14 for each tile you wish to add into the '.swtpkg' file]

        15. Close stormworks game
        16. Move all of the addon mission folders (and satellite images if any) you just created from the missions directory (Commonly 'C:\Users\YOUR_USER\AppData\Roaming\Stormworks\data\missions') and put them into a new folder or directory that only contains these items.
        17. Modify this app's 'appsettings.json' file '{nameof(FilePathOptions)}.{nameof(FilePathOptions.CopySourceDirectoryPath)}' value with this new folder/directory path you just moved the items into. 
            - The 'appsettings.json' file should be located within the same directory as this app's '.exe' file
        18. Customize any other 'appsettings.json' file values for this new '.swtpkg' file
            - The 'appsettings.json' should contain comments to explain what values are
        19. Restart this app
            - The app 'appsettings.json' changes only take effect after a restart
        """);

        Console.WriteLine();

        return Task.CompletedTask;
    }
}
