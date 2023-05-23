using Game.Mode.Stormworks.Tools.Swtpkg.Application.Models;
using Game.Mode.Stormworks.Tools.Swtpkg.ConsoleApp.Extensions;
using System.Drawing;

namespace Game.Mode.Stormworks.Tools.Swtpkg.ConsoleApp.Models;
public class BitmapImage : IImage
{
    private readonly Bitmap _bitmap;

    public BitmapImage(string filename)
    {
        _bitmap = new Bitmap(filename);
    }
    private BitmapImage(Bitmap bitmap)
    {
        _bitmap = bitmap;
    }

    public int Width => _bitmap.Width;
    public int Height => _bitmap.Height;
    public PixelFormat PixelFormat => _bitmap.PixelFormat.ToAbstractions();

    public Color GetPixel(int x, int y)
    {
        return _bitmap.GetPixel(x, y);
    }

    public IImage Clone(Rectangle rect, PixelFormat pixelFormat)
    {
        Bitmap clonedBitmap = _bitmap.Clone(rect, pixelFormat.ToDrawing());

        return new BitmapImage(clonedBitmap);
    }

    public void Save(string filename)
    {
        _bitmap.Save(filename); 
    }

    public void Dispose()
    {
        _bitmap.Dispose();
    }
}
