using Game.Mode.Stormworks.Tools.Swtpkg.Application.Models;
using Game.Mode.Stormworks.Tools.Swtpkg.Application.Options;
using Game.Mode.Stormworks.Tools.Swtpkg.Application.Validators;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Xml;

namespace Game.Mode.Stormworks.Tools.Swtpkg.Application.Services;
public interface IAddonService
{
    Task CopyToWorkingDirectoryAsync();
    Task<AddonXml> GetAddonAsync(string addonSubFolderName);
    Task<IReadOnlyList<AddonXml>> GetAddonsAsync();
}
public class AddonService : IAddonService
{
    private readonly ILogger<AddonService> _logger;
    private readonly IOptions<FilePathOptions> _filePathOptions;
    private readonly IOptions<PackagingOptions> _packagingOptions;

    public AddonService(
        ILogger<AddonService> logger,
        IOptions<FilePathOptions> filePathOptions,
        IOptions<PackagingOptions> packagingOptions)
    {
        _logger = logger;
        _filePathOptions = filePathOptions;
        _packagingOptions = packagingOptions;
    }

    public Task<AddonXml> GetAddonAsync(string addonSubFolderName)
    {
        FilePathOptions filePathOptions = _filePathOptions.Value;
        FilePathOptionsValidator.ThrowIfNull(filePathOptions.WorkingDirectoryPath);

        PackagingOptions packagingOptions = _packagingOptions.Value;
        PackagingOptionsValidator.ThrowIfNull(packagingOptions.AddonXmlFileName);

        var directory = new DirectoryInfo(filePathOptions.WorkingDirectoryPath);
        DirectoryInfo? subDirectory = directory
            .GetDirectories()
            .FirstOrDefault(d => d.Name == addonSubFolderName);

        if (subDirectory is null)
        {
            throw new Exception($"There is no addon directory with the provided addon directory name '{addonSubFolderName}'");
        }

        FileInfo? file = subDirectory
            .GetFiles()
            .First(f => f.Name == packagingOptions.AddonXmlFileName);

        AddonXml addonXml = ParseAddonXmlFile(file);

        return Task.FromResult(addonXml);
    }

    public Task CopyToWorkingDirectoryAsync()
    {
        FilePathOptions filePathOptions = _filePathOptions.Value;
        FilePathOptionsValidator.ThrowIfNull(filePathOptions.WorkingDirectoryPath);
        FilePathOptionsValidator.ThrowIfNull(filePathOptions.CopySourceDirectoryPath);

        if (!Directory.Exists(filePathOptions.WorkingDirectoryPath))
        {
            throw new Exception($"The working directory '{filePathOptions.WorkingDirectoryPath}' does not exist.");
        }

        if (!Directory.Exists(filePathOptions.CopySourceDirectoryPath))
        {
            throw new Exception($"The copy source directory '{filePathOptions.CopySourceDirectoryPath}' does not exist.");
        }

        _logger.LogInformation("Starting deep clone of the copy source directory '{copySourceDirectory}'.", filePathOptions.CopySourceDirectoryPath);

        DeepCloneDirectory(filePathOptions.CopySourceDirectoryPath, filePathOptions.WorkingDirectoryPath);

        _logger.LogInformation("Finished deep clone of the copy source directory '{copySourceDirectory}'.", filePathOptions.CopySourceDirectoryPath);

        return Task.CompletedTask;
    }

    //todo move to lexicom
    private void DeepCloneDirectory(string sourceDirectory, string targetDirectory)
    {
        var directory = new DirectoryInfo(sourceDirectory);
        if (!directory.Exists || !Directory.Exists(targetDirectory))
        {
            return;
        }

        _logger.LogInformation("Starting clone of '{directoryName}'", directory.Name);

        FileInfo[] files = directory.GetFiles();
        foreach (FileInfo file in files)
        {
            string targetFilePath = Path.Combine(targetDirectory, file.Name);

            _logger.LogInformation("Copying file '{fileName}'", file.Name);
            file.CopyTo(targetFilePath);
        }

        DirectoryInfo[] subDirectories = directory.GetDirectories();
        foreach (DirectoryInfo subDirectory in subDirectories)
        {
            string targetSubDirectoryPath = Path.Combine(targetDirectory, subDirectory.Name);

            Directory.CreateDirectory(targetSubDirectoryPath);

            DeepCloneDirectory(subDirectory.FullName, targetSubDirectoryPath);
        }
    }

    public Task<IReadOnlyList<AddonXml>> GetAddonsAsync()
    {
        FilePathOptions filePathOptions = _filePathOptions.Value;
        FilePathOptionsValidator.ThrowIfNull(filePathOptions.WorkingDirectoryPath);

        _logger.LogInformation("Gathering addon files from '{workingDirectoryPath}'", filePathOptions.WorkingDirectoryPath);
        IReadOnlyList<FileInfo> addonFiles = GetAddonFiles(filePathOptions.WorkingDirectoryPath);

        var addons = new List<AddonXml>();
        foreach (FileInfo addonFile in addonFiles)
        {
            var addonXml = ParseAddonXmlFile(addonFile);

            addons.Add(addonXml);
        }

        return Task.FromResult<IReadOnlyList<AddonXml>>(addons);
    }

    private AddonXml ParseAddonXmlFile(FileInfo addonFile)
    {
        string addonFilePath = addonFile.FullName;
        _logger.LogInformation("Loading the xml file '{addonFilePath}'", addonFilePath);

        var playlist = new XmlDocument();
        playlist.Load(addonFilePath);

        XmlNodeList? nodeList = playlist.SelectNodes("playlist/locations/locations/l");
        if (nodeList is not null)
        {
            foreach (XmlNode node in nodeList)
            {
                string? name = node.Attributes?["name"]?.Value;
                string? path = node.Attributes?["tile"]?.Value;

                string? tileFileName = Path.GetFileNameWithoutExtension(path);

                _logger.LogInformation("Getting the addon xml data for the tile '{tileFileName}'", tileFileName);

                return new AddonXml
                {
                    TileFileName = tileFileName ?? string.Empty,
                    TileName = name ?? string.Empty,
                };
            }
        }

        throw new Exception($"Could not parse the addon file '{addonFile.FullName}'");
    }

    private IReadOnlyList<FileInfo> GetAddonFiles(string directory, List<FileInfo>? addonXmlFiles = null)
    {
        PackagingOptions packagingOptions = _packagingOptions.Value;
        PackagingOptionsValidator.ThrowIfNull(packagingOptions.AddonXmlFileName);

        addonXmlFiles ??= new List<FileInfo>();

        var directoryInfo = new DirectoryInfo(directory);
        FileInfo[] files = directoryInfo.GetFiles();
        foreach (FileInfo file in files)
        {
            if (file.Name == packagingOptions.AddonXmlFileName)
            {
                addonXmlFiles.Add(file);
            }
        }

        foreach (DirectoryInfo subDirectory in directoryInfo.GetDirectories())
        {
            GetAddonFiles(subDirectory.FullName, addonXmlFiles);
        }

        return addonXmlFiles;
    }
}
