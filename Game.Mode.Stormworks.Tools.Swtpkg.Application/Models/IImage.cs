using System.Drawing;

namespace Game.Mode.Stormworks.Tools.Swtpkg.Application.Models;
public interface IImage : IDisposable
{
    int Width { get; }
    int Height { get; }
    PixelFormat PixelFormat { get; }

    Color GetPixel(int x, int y);
    IImage Clone(Rectangle rect, PixelFormat pixelFormat);
    void Save(string filename);
}
