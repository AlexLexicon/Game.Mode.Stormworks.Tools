using Game.Mode.Stormworks.Tools.Swtpkg.Application.Factories;
using Game.Mode.Stormworks.Tools.Swtpkg.Application.Models;
using System.Drawing;

namespace Game.Mode.Stormworks.Tools.Swtpkg.Application.Services;
public interface IAddonLocationImageService
{
    /// <exception cref="FileNotFoundException"/>
    Task CropManualSatelliteImageAsync(string imageFilePath);
}
public class AddonLocationImageService : IAddonLocationImageService
{
    private readonly IImageFactory _imageFactory;

    public AddonLocationImageService(IImageFactory imageFactory)
    {
        _imageFactory = imageFactory;
    }

    public async Task CropManualSatelliteImageAsync(string imageFilePath)
    {
        var fi = new FileInfo(imageFilePath);

        if (!fi.Exists || fi.DirectoryName is null)
        {
            throw new FileNotFoundException(null, imageFilePath);
        }

        string tempImageFilePath = Path.Combine(fi.DirectoryName, $"{fi.Name}.temp.{DateTime.Now.Ticks}{fi.Extension}");

        //you cannot save a bitmap file path to itself
        //so we will create and modify a temp copy of the bitmap
        //and then save that temp to the original file path
        File.Copy(imageFilePath, tempImageFilePath);

        IImage tempImage = await _imageFactory.CreateImageAsync(tempImageFilePath);

        Point? satelliteCropTopLeft = null;
        Point? satelliteCropBottomRight = null;

        for (int y = 0; y < tempImage.Height; y++)
        {
            Color previousPixel = default;

            int? satelliteCropStartedX = null;
            bool firstBottomRightOfTheCurrentRow = false;
            for (int x = 0; x < tempImage.Width; x++)
            {
                var currentPixel = tempImage.GetPixel(x, y);

                bool isPreviousPixelMouseHover = IsManualSatelliteMouseHoverColor(previousPixel);
                bool isCurrentPixelMouseHover = IsManualSatelliteMouseHoverColor(currentPixel);

                if (isPreviousPixelMouseHover && !isCurrentPixelMouseHover)
                {
                    satelliteCropStartedX = x;
                }

                if (satelliteCropStartedX is not null && isCurrentPixelMouseHover)
                {
                    int staelliteCropRowLength = x - satelliteCropStartedX.Value;
                    if (staelliteCropRowLength > 200)
                    {
                        satelliteCropTopLeft ??= new Point(satelliteCropStartedX.Value, y);
                        if (!firstBottomRightOfTheCurrentRow && y < 450)
                        {
                            satelliteCropBottomRight = new Point(x, y + 1);
                            firstBottomRightOfTheCurrentRow = true;
                        }
                    }
                    else
                    {
                        satelliteCropStartedX = null;
                    }
                }

                previousPixel = currentPixel;
            }
        }

        if (satelliteCropTopLeft is not null && satelliteCropBottomRight is not null)
        {
            var replacementImage = tempImage.Clone(new Rectangle
            {
                X = satelliteCropTopLeft.Value.X,
                Y = satelliteCropTopLeft.Value.Y,
                Width = satelliteCropBottomRight.Value.X - satelliteCropTopLeft.Value.X,
                Height = satelliteCropBottomRight.Value.Y - satelliteCropTopLeft.Value.Y,
            }, tempImage.PixelFormat);

            replacementImage.Save(imageFilePath);

            replacementImage.Dispose();
        }

        tempImage.Dispose();

        File.Delete(tempImageFilePath);
    }

    private bool IsManualSatelliteMouseHoverColor(Color color)
    {
        if (color.R is < 63 or > 64)
        {
            return false;
        }

        if (color.G is < 143 or > 145)
        {
            return false;
        }

        if (color.B is < 177 or > 179)
        {
            return false;
        }

        return true;
    }
}
