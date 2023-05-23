using FluentValidation;
using Lexicom.Validation;
using Lexicom.Validation.Amenities.Extensions;

namespace Game.Mode.Stormworks.Tools.Swtpkg.Application.RuleSets;
public class DirectoryPathRuleSet : AbstractRuleSet<string?>
{
    public override void Use<T>(IRuleBuilderOptions<T, string?> ruleBuilder)
    {
        ruleBuilder
            .NotNull()
            .NotSimplyEmpty()
            .NotAllWhitespaces()
            .Must(Directory.Exists).WithMessage("The directory path does not exist");
    }
}
