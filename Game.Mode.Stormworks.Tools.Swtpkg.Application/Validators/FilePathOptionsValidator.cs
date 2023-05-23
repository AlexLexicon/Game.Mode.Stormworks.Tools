using Game.Mode.Stormworks.Tools.Swtpkg.Application.Options;
using Game.Mode.Stormworks.Tools.Swtpkg.Application.RuleSets;
using Lexicom.Validation.Extensions;
using Lexicom.Validation.Options;

namespace Game.Mode.Stormworks.Tools.Swtpkg.Application.Validators;
public class FilePathOptionsValidator : AbstractOptionsValidator<FilePathOptions>
{
    public FilePathOptionsValidator(DirectoryPathRuleSet directoryPathRuleSet)
    {
        RuleFor(o => o.CopySourceDirectoryPath)
            .UseRuleSet(directoryPathRuleSet);

        RuleFor(o => o.WorkingDirectoryPath)
            .UseRuleSet(directoryPathRuleSet);

        RuleFor(o => o.OutputDirectoryPath)
            .UseRuleSet(directoryPathRuleSet);
    }
}
