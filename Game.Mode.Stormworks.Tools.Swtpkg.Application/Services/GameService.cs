using Game.Mode.Stormworks.Tools.Swtpkg.Application.Models;
using Game.Mode.Stormworks.Tools.Swtpkg.Application.Options;
using Game.Mode.Stormworks.Tools.Swtpkg.Application.Validators;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Xml;

namespace Game.Mode.Stormworks.Tools.Swtpkg.Application.Services;
public interface IGameService
{
    Task<TileXml> GetTilesAsync(string tileFileName);
    Task CopyTilesXmlToWorkingDirectoryAsync(IEnumerable<string> tileFileNames);
}
public class GameService : IGameService
{
    private readonly ILogger<GameService> _logger;
    private readonly IOptions<FilePathOptions> _filePathOptions;

    public GameService(
        ILogger<GameService> logger,
        IOptions<FilePathOptions> filePathOptions)
    {
        _logger = logger;
        _filePathOptions = filePathOptions;
    }

    public async Task<TileXml> GetTilesAsync(string tileFileName)
    {
        FilePathOptions filePathOptions = _filePathOptions.Value;
        FilePathOptionsValidator.ThrowIfNull(filePathOptions.WorkingDirectoryPath);

        var directory = new DirectoryInfo(filePathOptions.WorkingDirectoryPath);
        foreach (var file in directory.GetFiles())
        {
            if (file.Extension is ".xml")
            {
                string[] rawXml = await File.ReadAllLinesAsync(file.FullName);

                int type = -1;
                bool isIsland = false;
                bool isPurchasable = false;
                int purchaseCost = 0;

                string? definitionLine = rawXml.FirstOrDefault(r => r.StartsWith("<definition"));
                if (definitionLine is not null)
                {
                    string? typeString = GetSlice(definitionLine, "tile_type=\"", "\"");
                    if (typeString is not null && int.TryParse(typeString, out int typeResult))
                    {
                        type = typeResult;
                    }
                    string? isIslandString = GetSlice(definitionLine, "is_island=\"", "\"");
                    if (isIslandString is not null && bool.TryParse(isIslandString, out bool isIslandResult))
                    {
                        isIsland = isIslandResult;
                    }
                    string? isPurchasableString = GetSlice(definitionLine, "is_purchasable=\"", "\"");
                    if (isPurchasableString is not null && bool.TryParse(isPurchasableString, out bool isPurchasableResult))
                    {
                        isPurchasable = isPurchasableResult;
                    }
                    string? purchaseCostString = GetSlice(definitionLine, "purchase_cost=\"", "\"");
                    if (purchaseCostString is not null && int.TryParse(purchaseCostString, out int purchaseCostResult))
                    {
                        purchaseCost = purchaseCostResult;
                    }
                    
                    return new TileXml
                    {
                        IsIsland = isIsland,
                        IsPurchasable = isPurchasable,
                        PurchaseCost = purchaseCost,
                    };
                }
                //try
                //{
                //    var playlist = new XmlDocument();
                //    playlist.Load(file.FullName);

                //    XmlNodeList? nodeList = playlist.SelectNodes("definition");
                //    if (nodeList is not null)
                //    {
                //        foreach (XmlNode node in nodeList)
                //        {
                //            string? isIslandString = node.Attributes?["is_island"]?.Value;
                //            string? isPurchasableString = node.Attributes?["is_purchasable"]?.Value;
                //            string? purchaseCostString = node.Attributes?["purchase_cost"]?.Value;

                //            bool isIsland = bool.Parse(isIslandString ?? "false");
                //            bool isPurchasable = bool.Parse(isPurchasableString ?? "false");
                //            int purchaseCost = int.Parse(purchaseCostString ?? "0");

                //            _logger.LogInformation("Getting the tile xml data for the tile '{tileFileName}'", tileFileName);

                //            var tileXml = new TileXml
                //            {
                //                IsIsland = isIsland,
                //                IsPurchasable = isPurchasable,
                //                PurchaseCost = purchaseCost,
                //            };

                //            return Task.FromResult(tileXml);
                //        }
                //    }
                //}
                //catch (Exception e)
                //{
                //    throw;
                //}
            }
        }

        throw new Exception($"Could not find the tile xml for the tile file name '{tileFileName}'");
    }

    private string? GetSlice(string input, string startString, string endString)
    {
        int start = input.IndexOf(startString);

        if (start < 0)
        {
            return null;
        }

        int begin = start + startString.Length;
        int end = input.IndexOf(endString, begin);

        if (end > 0 && end > begin)
        {
            return input[begin..end];
        }

        return null;
    }

    public Task CopyTilesXmlToWorkingDirectoryAsync(IEnumerable<string> tileFileNames)
    {
        FilePathOptions filePathOptions = _filePathOptions.Value;
        FilePathOptionsValidator.ThrowIfNull(filePathOptions.GameDataTilesDirectoryPath);
        FilePathOptionsValidator.ThrowIfNull(filePathOptions.WorkingDirectoryPath);

        var directory = new DirectoryInfo(filePathOptions.GameDataTilesDirectoryPath);

        List<string> tileFileNamesList = tileFileNames.ToList();

        foreach (FileInfo file in directory.GetFiles())
        {
            string tileFileName = Path.GetFileNameWithoutExtension(file.Name);

            if (tileFileNamesList.Contains(tileFileName))
            {
                string path = Path.Combine(filePathOptions.WorkingDirectoryPath, file.Name);

                _logger.LogInformation("Copying the tile file to '{path}'", path);
                file.CopyTo(path);
            }
        }

        return Task.CompletedTask;
    }
}
