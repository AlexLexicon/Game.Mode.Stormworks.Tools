using FluentValidation;
using Game.Mode.Stormworks.Tools.Swtpkg.Application.Options;
using Lexicom.Validation.Amenities.Extensions;
using Lexicom.Validation.Options;

namespace Game.Mode.Stormworks.Tools.Swtpkg.Application.Validators;
public class PackagingOptionsValidator : AbstractOptionsValidator<PackagingOptions>
{
    public PackagingOptionsValidator()
    {
        RuleFor(o => o.PackageMetaDataFileName)
            .NotNull()
            .NotSimplyEmpty()
            .NotAllWhitespaces();
        //todo add NotAllDigits validator
    }
}
