namespace Game.Mode.Stormworks.Tools.Swtpkg.Application.Models;
public class PackageMetaDataState
{
    public required bool HasGameFiles { get; set; }
    public required bool HasTileMetaData { get; set; }
    public required bool HasTileCoreData { get; set; }
    public required bool HasTileWorkbenchData { get; set; }
    public required bool HasTilePositionData { get; set; }
    public required bool IsZipped { get; set; }
}
