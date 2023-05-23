using FluentValidation;
using Game.Mode.Stormworks.Tools.Swtpkg.Application.Options;
using Lexicom.Validation.Amenities.Extensions;
using Lexicom.Validation.Options;

namespace Game.Mode.Stormworks.Tools.Swtpkg.Application.Validators;
public class PackageSettingsOptionsValidator : AbstractOptionsValidator<PackageSettingsOptions>
{
    public PackageSettingsOptionsValidator()
    {
        RuleFor(o => o.Version)
            .NotNull()
            .NotSimplyEmpty()
            .NotAllWhitespaces()
            .NotAllWhitespaces();
    }
}
