using Game.Mode.Stormworks.Tools.Swtpkg.Application.Factories;
using Game.Mode.Stormworks.Tools.Swtpkg.Application.Models;

namespace Game.Mode.Stormworks.Tools.Swtpkg.ConsoleApp.Factories;
public class DrawingImageFactory : IImageFactory
{
    public Task<IImage> CreateImageAsync(string imageFilePath)
    {
        throw new NotImplementedException();
    }
}
