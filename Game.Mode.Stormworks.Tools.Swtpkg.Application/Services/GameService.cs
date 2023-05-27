using Game.Mode.Stormworks.Tools.Swtpkg.Application.Models;

namespace Game.Mode.Stormworks.Tools.Swtpkg.Application.Services;
public interface IGameService
{
    Task<TileXml> GetTilesAsync(string tileFileName);
    Task CopyTilesXmlToWorkingDirectoryAsync(IEnumerable<string> tileFileNames);
}
public class GameService : IGameService
{
    public Task<TileXml> GetTilesAsync(string tileFileName)
    {
        throw new NotImplementedException();
    }

    public Task CopyTilesXmlToWorkingDirectoryAsync(IEnumerable<string> tileFileNames)
    {
        return Task.CompletedTask;
    }
}
