using Game.Mode.Stormworks.Tools.Swtpkg.Application.Models;

namespace Game.Mode.Stormworks.Tools.Swtpkg.Application.Services;
public interface IGameService
{
    Task<TileXml> GetTilesAsync(string tileFileName);
}
public class GameService : IGameService
{
    public Task<TileXml> GetTilesAsync(string tileFileName)
    {
        throw new NotImplementedException();
    }
}
