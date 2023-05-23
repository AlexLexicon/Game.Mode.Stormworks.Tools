using Game.Mode.Stormworks.Tools.Swtpkg.Application.Options;
using Game.Mode.Stormworks.Tools.Swtpkg.Application.Services;
using Lexicom.DependencyInjection.Amenities.Extensions;
using Lexicom.Validation.Amenities.Extensions;
using Lexicom.Validation.Extensions;
using Lexicom.Validation.Options.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Game.Mode.Stormworks.Tools.Swtpkg.Application.Extensions;
public static class ServiceCollectionExtensions
{
    public static void AddSwtpkgApplication(this IServiceCollection services)
    {
        services.AddLexicomValidation(options =>
        {
            options.AddAmenities();
            options.AddRuleSets<AssemblyScanMarker>();
            options.AddValidators<AssemblyScanMarker>();
        });

        services
            .AddOptions<FilePathOptions>()
            .BindConfiguration()
            .Validate();

        services
            .AddOptions<PackageSettingsOptions>()
            .BindConfiguration()
            .Validate();

        services
            .AddOptions<PackagingOptions>()
            .BindConfiguration()
            .Validate();

        services.AddScoped<IAddonImageService, AddonImageService>();
        services.AddScoped<IPackageDataService, PackageDataService>();
        services.AddScoped<IAddonService, AddonService>();
        services.AddScoped<IAddonService, AddonService>();
        services.AddScoped<IGameService, GameService>();
    }
}
