using Game.Mode.Stormworks.Tools.Swtpkg.Application.Factories;
using Game.Mode.Stormworks.Tools.Swtpkg.Application.Models;
using Game.Mode.Stormworks.Tools.Swtpkg.Application.Options;
using Game.Mode.Stormworks.Tools.Swtpkg.Application.Validators;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Drawing;

namespace Game.Mode.Stormworks.Tools.Swtpkg.Application.Services;
public interface IAddonImageService
{
    /// <exception cref="FileNotFoundException"/>
    Task CropManualSatelliteImagesAsync();
}
public class AddonImageService : IAddonImageService
{
    private readonly ILogger<AddonImageService> _logger;
    private readonly IImageFactory _imageFactory;
    private readonly IOptions<FilePathOptions> _filePathOptions;

    public AddonImageService(
        ILogger<AddonImageService> logger,  
        IImageFactory imageFactory, 
        IOptions<FilePathOptions> filePathOptions)
    {
        _logger = logger;
        _imageFactory = imageFactory;
        _filePathOptions = filePathOptions;
    }

    public async Task CropManualSatelliteImagesAsync()
    {
        FilePathOptions filePathOptions = _filePathOptions.Value;
        FilePathOptionsValidator.ThrowIfNull(filePathOptions.WorkingDirectoryPath);

        if (!Directory.Exists(filePathOptions.WorkingDirectoryPath))
        {
            throw new Exception($"The working directory '{filePathOptions.WorkingDirectoryPath}' does not exist.");
        }

        string[] filePaths = Directory.GetFiles(filePathOptions.WorkingDirectoryPath);
        foreach (string filePath in filePaths)
        {
            if (filePath.EndsWith(".png"))
            {
                await CropManualSatelliteImageAsync(filePath);
            }
        }
    }

    private async Task CropManualSatelliteImageAsync(string imageFilePath)
    {
        var fi = new FileInfo(imageFilePath);

        if (!fi.Exists || fi.DirectoryName is null)
        {
            throw new FileNotFoundException(null, imageFilePath);
        }

        _logger.LogInformation("Creating temp image file to perform the cropping to.");

        string tempImageFilePath = Path.Combine(fi.DirectoryName, $"{fi.Name}.temp.{DateTime.Now.Ticks}{fi.Extension}");

        _logger.LogInformation("Copying the contents of the image file to the temp file '{tempFilePath}'", tempImageFilePath);
        //you cannot save a bitmap file path to itself
        //so we will create and modify a temp copy of the bitmap
        //and then save that temp to the original file path
        File.Copy(imageFilePath, tempImageFilePath);

        _logger.LogInformation("Creating image from file");
        IImage tempImage = await _imageFactory.CreateImageAsync(tempImageFilePath);

        Point? satelliteCropTopLeft = null;
        Point? satelliteCropBottomRight = null;

        _logger.LogInformation("Performing crop on image");
        for (int y = 0; y < tempImage.Height; y++)
        {
            double progress = Math.Round((tempImage.Height - y) / (double)tempImage.Height, 2);
            _logger.LogInformation("Progress: {progress}%", progress);
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
            _logger.LogInformation("Cloning cropped image");
            var replacementImage = tempImage.Clone(new Rectangle
            {
                X = satelliteCropTopLeft.Value.X,
                Y = satelliteCropTopLeft.Value.Y,
                Width = satelliteCropBottomRight.Value.X - satelliteCropTopLeft.Value.X,
                Height = satelliteCropBottomRight.Value.Y - satelliteCropTopLeft.Value.Y,
            }, tempImage.PixelFormat);

            _logger.LogInformation("Saving cropped image to file '{filePath}'", imageFilePath);
            replacementImage.Save(imageFilePath);

            _logger.LogInformation("Disposing of cropped image.");
            replacementImage.Dispose();
        }

        _logger.LogInformation("Disposing of original image.");
        tempImage.Dispose();

        _logger.LogInformation("Deleting the temp file '{tempFilePath}'", tempImageFilePath);
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
