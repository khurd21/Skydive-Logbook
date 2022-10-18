namespace LogbookService.Settings;

public static class ProjectSettings
{
    public static string AwsEncryptionArn => File.ReadAllText(Path.Combine(_solutionDirectory, "aws-encryption-arn.txt"));

    public static string JwtToken => File.ReadAllText(Path.Combine(_solutionDirectory, "jwt-token.txt"));

    private static string _solutionDirectory = ProjectSettings.GetSolutionDirectory().FullName;

    private static string _awsEncryptionArn = "aws-encryption-arn.key";

    private static string _jwtToken = "jwt-token.key";

    private static DirectoryInfo GetSolutionDirectory(in string? currentPath = null, in string solutionName = "*.sln")
    {
        DirectoryInfo? directoryInfo = new DirectoryInfo(currentPath ?? Directory.GetCurrentDirectory());

        while (directoryInfo != null && !directoryInfo.GetFiles(solutionName).Any())
        {
            directoryInfo = directoryInfo.Parent;
        }

        return directoryInfo ?? throw new DirectoryNotFoundException($"Could not find {solutionName} in {currentPath ?? Directory.GetCurrentDirectory()}");

    }
}