namespace Game.Mode.Stormworks.Tools.Swtpkg.Application.Exceptions;
public class WorkingDirectoryDoesNotExistException : Exception
{
    public WorkingDirectoryDoesNotExistException(string? workingDirectoryPath) : base($"The working directory with the path '{workingDirectoryPath ?? "null"}' does not exist.")
    {
    }
}
