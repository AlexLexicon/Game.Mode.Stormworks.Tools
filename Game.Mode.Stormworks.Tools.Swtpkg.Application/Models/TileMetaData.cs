namespace Game.Mode.Stormworks.Tools.Swtpkg.Application.Models;
public class TileMetaData
{
    public required string PackageVersion { get; init; }
    public required string GameVersion { get; init; }
    public required DateTimeOffset CreatedDateTimeUtc { get; init; }
    public required string CreatedBy { get; init; }
    public required DateTimeOffset ModifiedDateTimeUtc { get; set; }
    public required string ModifiedBy { get; init; }
}
