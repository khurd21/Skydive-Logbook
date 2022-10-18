using LogbookService.Records;

namespace LogbookService.Dependencies.AuthenticationService;

public interface IAuthenticationService
{
    void Delete(in string email);
    SkydiverInfo Login(in string email, in string password);
    SkydiverInfo Register(in SkydiverInfo skydiverInfo, in string password);
}