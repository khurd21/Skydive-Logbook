using LogbookService.Records;

namespace Logbook.Responses.Authentication;

public class AuthenticateResponse
{
    public string? Email { get; init; }
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public int USPAMembershipNumber { get; init; }
    public string? USPALicenseNumber { get; init; }
    public string? Token { get; init; }

    public AuthenticateResponse()
    {
    }

    public AuthenticateResponse(in SkydiverInfo skydiverInfo, in string token)
    {
        this.Email = skydiverInfo.Email;
        this.FirstName = skydiverInfo.FirstName;
        this.LastName = skydiverInfo.LastName;
        this.USPAMembershipNumber = skydiverInfo.USPAMembershipNumber;
        this.USPALicenseNumber = skydiverInfo.USPALicenseNumber;
        this.Token = token;
    }
}