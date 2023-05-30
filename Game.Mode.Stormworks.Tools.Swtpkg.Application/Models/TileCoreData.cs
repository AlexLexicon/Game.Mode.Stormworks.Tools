namespace Game.Mode.Stormworks.Tools.Swtpkg.Application.Models;
public class TileCoreData
{
    public required string TileName { get; init; }
    public required bool IsIsland { get; init; }
    public required bool IsPurchasable { get; init; }
    public required int PurchaseCost { get; init; }
}
