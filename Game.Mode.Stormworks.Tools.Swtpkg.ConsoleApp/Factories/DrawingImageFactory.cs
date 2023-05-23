using Game.Mode.Stormworks.Tools.Swtpkg.Application.Factories;
using Game.Mode.Stormworks.Tools.Swtpkg.Application.Models;
using Game.Mode.Stormworks.Tools.Swtpkg.ConsoleApp.Models;

namespace Game.Mode.Stormworks.Tools.Swtpkg.ConsoleApp.Factories;
public class DrawingImageFactory : IImageFactory
{
    public Task<IImage> CreateImageAsync(string imageFilePath)
    {
        var image = new BitmapImage(imageFilePath);

        return Task.FromResult<IImage>(image);
    }
}
