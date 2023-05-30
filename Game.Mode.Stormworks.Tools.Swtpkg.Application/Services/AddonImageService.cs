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
    Task RenameSatelliteImagesAsync();
    /// <exception cref="FileNotFoundException"/>
    Task CropManualSatelliteImagesAsync();
}
public class AddonImageService : IAddonImageService
{
    private readonly ILogger<AddonImageService> _logger;
    private readonly IImageFactory _imageFactory;
    private readonly IAddonService _addonService;
    private readonly IOptions<FilePathOptions> _filePathOptions;
    private readonly IOptions<PackagingOptions> _packagingOptions;

    public AddonImageService(
        ILogger<AddonImageService> logger,
        IImageFactory imageFactory,
        IAddonService addonService,
        IOptions<FilePathOptions> filePathOptions,
        IOptions<PackagingOptions> packagingOptions)
    {
        _logger = logger;
        _imageFactory = imageFactory;
        _addonService = addonService;
        _filePathOptions = filePathOptions;
        _packagingOptions = packagingOptions;
    }

    public async Task RenameSatelliteImagesAsync()
    {
        FilePathOptions filePathOptions = _filePathOptions.Value;
        FilePathOptionsValidator.ThrowIfNull(filePathOptions.WorkingDirectoryPath);

        PackagingOptions packagingOptions = _packagingOptions.Value;
        PackagingOptionsValidator.ThrowIfNull(packagingOptions.SatelliteImageExtension);

        var directory = new DirectoryInfo(filePathOptions.WorkingDirectoryPath);
        foreach (FileInfo file in directory.GetFiles())
        {
            if (file.Extension == packagingOptions.SatelliteImageExtension)
            {
                string addonDirectoryName = Path.GetFileNameWithoutExtension(file.Name);

                AddonXml addon = await _addonService.GetAddonAsync(addonDirectoryName);

                if (file.DirectoryName is null)
                {
                    throw new Exception($"The directory name for the addon file '{file.FullName}' was null");
                }

                string normalizedTileName = addon.TileName.ToLowerInvariant();

                string newImageFilePath = Path.Combine(file.DirectoryName, $"{normalizedTileName}{packagingOptions.SatelliteImageExtension}");

                _logger.LogInformation($"Renaming the file '{file.Name}' to '{normalizedTileName}'");
                File.Move(file.FullName, newImageFilePath);
            }
        }
    }

    public async Task CropManualSatelliteImagesAsync()
    {
        FilePathOptions filePathOptions = _filePathOptions.Value;
        FilePathOptionsValidator.ThrowIfNull(filePathOptions.WorkingDirectoryPath);

        PackagingOptions packagingOptions = _packagingOptions.Value;
        PackagingOptionsValidator.ThrowIfNull(packagingOptions.SatelliteImageExtension);

        var directory = new DirectoryInfo(filePathOptions.WorkingDirectoryPath);
        foreach (FileInfo file in directory.GetFiles())
        {
            if (file.Extension == packagingOptions.SatelliteImageExtension)
            {
                await CropManualSatelliteImageAsync(file.FullName);
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
