namespace Game.Mode.Stormworks.Tools.Swtpkg.Application.Services;
public interface IPackageSettingsService
{
    Task<string> GetPackageVersionAsync();
    Task<string> GetGameVersionAsync();
    Task<string> GetAuthorAsync();
}
public class PackageSettingsService : IPackageSettingsService
{
    public Task<string> GetPackageVersionAsync()
    {
        throw new NotImplementedException();
    }

    public Task<string> GetGameVersionAsync()
    {
        throw new NotImplementedException();
    }

    public Task<string> GetAuthorAsync()
    {
        throw new NotImplementedException();
    }
}
