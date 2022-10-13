namespace LogbookService.Settings;

public static class ProjectSettings
{
    public static string KeyFile = "encryptor.key";

    public static DirectoryInfo GetSolutionDirectoryInfo(
        in string? currentPath = null, in string solutionName = "*.sln")
    {
        DirectoryInfo? directory = new(currentPath ?? Directory.GetCurrentDirectory());

        while (directory != null && !directory.GetFiles(solutionName).Any())
        {
            directory = directory.Parent;
        }

        return directory ?? throw new FileNotFoundException("Solution file could not be found");
    }
}