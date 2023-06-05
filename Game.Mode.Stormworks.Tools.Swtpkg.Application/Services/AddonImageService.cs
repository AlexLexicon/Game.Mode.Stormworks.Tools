using Game.Mode.Stormworks.Tools.Swtpkg.Application.Factories;
using Game.Mode.Stormworks.Tools.Swtpkg.Application.Models;
using Game.Mode.Stormworks.Tools.Swtpkg.Application.Options;
using Game.Mode.Stormworks.Tools.Swtpkg.Application.Validators;
using Lexicom.DependencyInjection.Primitives;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Drawing;

namespace Game.Mode.Stormworks.Tools.Swtpkg.Application.Services;
public interface IAddonImageService
{
    Task RenameMapImagesAsync();
    /// <exception cref="FileNotFoundException"/>
    Task CropManualMapImagesAsync();
}
public class AddonImageService : IAddonImageService
{
    private readonly ILogger<AddonImageService> _logger;
    private readonly IImageFactory _imageFactory;
    private readonly IAddonService _addonService;
    private readonly IOptions<FilePathOptions> _filePathOptions;
    private readonly IOptions<PackagingOptions> _packagingOptions;
    private readonly IFileService _fileService;
    private readonly ITimeProvider _timeProvider;

    public AddonImageService(
        ILogger<AddonImageService> logger,
        IImageFactory imageFactory,
        IAddonService addonService,
        IOptions<FilePathOptions> filePathOptions,
        IOptions<PackagingOptions> packagingOptions,
        IFileService fileService,
        ITimeProvider timeProvider)
    {
        _logger = logger;
        _imageFactory = imageFactory;
        _addonService = addonService;
        _filePathOptions = filePathOptions;
        _packagingOptions = packagingOptions;
        _fileService = fileService;
        _timeProvider = timeProvider;
    }

    public async Task RenameMapImagesAsync()
    {
        IReadOnlyList<FileInfo> mapImageFiles = await _fileService.GetMapImageFilesAsync();
        foreach (FileInfo file in mapImageFiles)
        {
            if (file.DirectoryName is null)
            {
                throw new UnreachableException($"The file '{file.Name}' has a null directory name but this should never be possible since it must be located within the working directory.");
            }

            string addonSubFolderName = Path.GetFileNameWithoutExtension(file.Name);

            AddonXml addon = await _addonService.GetAddonAsync(addonSubFolderName);

            string normalizedTileName = addon.TileName.ToLowerInvariant();

            string newImageFilePath = Path.Combine(file.DirectoryName, $"{normalizedTileName}{packagingOptions.MapImageExtension}");

            _logger.LogInformation($"Renaming the file '{file.Name}' to '{normalizedTileName}'");
            File.Move(file.FullName, newImageFilePath);
        }
    }

    public async Task CropManualMapImagesAsync()
    {
        IReadOnlyList<FileInfo> mapImageFiles = await _fileService.GetMapImageFilesAsync();
        foreach (FileInfo file in mapImageFiles)
        {
            if (file.DirectoryName is null)
            {
                throw new UnreachableException($"The file '{file.Name}' has a null directory name but this should never be possible since it must be located within the working directory.");
            }

            string tempImageFilePath = Path.Combine(file.DirectoryName, $"{file.Name}.temp.{_timeProvider.UtcNow.Ticks}{file.Extension}");

            File.Copy(file.FullName, tempImageFilePath);

            IImage tempImage = await _imageFactory.CreateImageAsync(tempImageFilePath);

            Point? satelliteCropTopLeft = null;
            Point? satelliteCropBottomRight = null;
            for (int y = 0; y < tempImage.Height; y++)
            {
                double progress = Math.Round((tempImage.Height - y) / (double)tempImage.Height, 2);
                _logger.LogInformation("Progress: {progress}%", progress);
                Color previousPixel = default;

                int? satelliteCropStartedX = null;
                bool firstBottomRightOfTheCurrentRow = false;
                for (int x = 0; x < tempImage.Width; x++)
                {
                    Color currentPixel = tempImage.GetPixel(x, y);

                    bool isPreviousPixelMouseHover = IsManualMapMouseHoverColor(previousPixel);
                    bool isCurrentPixelMouseHover = IsManualMapMouseHoverColor(currentPixel);

                    if (isPreviousPixelMouseHover && !isCurrentPixelMouseHover)
                    {
                        satelliteCropStartedX = x;
                    }

                    if (satelliteCropStartedX is not null && isCurrentPixelMouseHover)
                    {
                        int staelliteCropRowLength = x - satelliteCropStartedX.Value;
                        if (staelliteCropRowLength is > 200)
                        {
                            satelliteCropTopLeft ??= new Point(satelliteCropStartedX.Value, y);
                            
                            if (!firstBottomRightOfTheCurrentRow && y is < 450)
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
                var croppedImageRect = new Rectangle
                {
                    X = satelliteCropTopLeft.Value.X,
                    Y = satelliteCropTopLeft.Value.Y,
                    Width = satelliteCropBottomRight.Value.X - satelliteCropTopLeft.Value.X,
                    Height = satelliteCropBottomRight.Value.Y - satelliteCropTopLeft.Value.Y,
                };

                IImage croppedImage = tempImage.Clone(croppedImageRect, tempImage.PixelFormat);

                croppedImage.Save(file.FullName);
                croppedImage.Dispose();
            }
            
            tempImage.Dispose();
            File.Delete(tempImageFilePath);
        }
    }

    private bool IsManualMapMouseHoverColor(Color color)
    {
        return
            color.R is >= 63 and <= 64 &&
            color.G is >= 143 and <= 145 &&
            color.B is >= 177 and <= 179;
    }
}
