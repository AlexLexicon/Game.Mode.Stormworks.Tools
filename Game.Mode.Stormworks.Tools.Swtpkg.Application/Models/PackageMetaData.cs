namespace Game.Mode.Stormworks.Tools.Swtpkg.Application.Models;
public class PackageMetaData
{
    public required string PackageVersion { get; init; }
    public required string GameVersion { get; init; }
    public required DateTimeOffset CreateDateTimeUtc { get; init; }
    public required DateTimeOffset ModifiedDateTimeUtc { get; set; }
    public required string CreatedBy { get; init; }
    public required string ModifiedBy { get; set; }
    public required PackageMetaDataState State { get; set; }
}
