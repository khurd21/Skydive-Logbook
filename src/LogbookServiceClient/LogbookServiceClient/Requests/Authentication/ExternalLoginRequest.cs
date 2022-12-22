namespace Logbook.Requests.Authentication;

public class ExternalLoginRequest
{
    public string? Provider { get; init; }

    public string? ReturnUrl { get; init; }
}