namespace Game.Mode.Stormworks.Tools.Swtpkg.ConsoleApp;
public static class PackagingPriority
{
    public const int ResetWorkingDirectory = 0;
    public const int CreatePackageMetaData = 1;
    public const int CopyManualFiles = 2;
    public const int CropSatelliteImages = 3;
    public const int RenameSatelliteImages = 4;
    public const int CopyGameFiles = 4;
    public const int GenerateTileMetaData = 5;
    public const int GenerateTileCoreData = 6;
    public const int PopulateTileCoreData = 7;
    public const int GenerateTileWorkbenchData = 8;
    public const int GenerateTilePositionData = 9;
    public const int CleanPackage = 10;
    public const int ZipPackage = 11;
    public const int ExportPackage = 12;
}
