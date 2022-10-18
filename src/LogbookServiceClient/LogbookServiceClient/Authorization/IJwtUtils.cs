using LogbookService.Records;

namespace Logbook.Authorization;

public interface IJwtUtils
{
    string GenerateJwtToken(in SkydiverInfo skydiverInfo);
    string? ValidateJwtToken(in string token);
}