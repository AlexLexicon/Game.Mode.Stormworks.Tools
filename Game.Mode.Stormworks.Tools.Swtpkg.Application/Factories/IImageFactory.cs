using Game.Mode.Stormworks.Tools.Swtpkg.Application.Models;

namespace Game.Mode.Stormworks.Tools.Swtpkg.Application.Factories;
public interface IImageFactory
{
    Task<IImage> CreateImageAsync(string imageFilePath);
}
