using Amazon.DynamoDBv2.DataModel;
using LogbookService.Exceptions;
using LogbookService.Records;

using static BCrypt.Net.BCrypt;

namespace LogbookService.Dependencies.AuthenticationService;

public class AuthenticationServiceProvider : IAuthenticationService
{
    private IDynamoDBContext DynamoDBContext { get; init; }

    public AuthenticationServiceProvider(IDynamoDBContext dynamoDBContext)
    {
        this.DynamoDBContext = dynamoDBContext;
    }

    public SkydiverInfo Login(in string email, in string password)
    {
        SkydiverInfo? skydiver = this.DynamoDBContext.LoadAsync<SkydiverInfo>(email).Result;

        if (skydiver is null || (Verify(password, skydiver.PasswordHash) is false))
        {
            throw new SkydiverNotFoundException("Skydiver email or password is incorrect");
        }

        return skydiver;
    }

    public void Delete(in string email)
    {
        SkydiverInfo? skydiver = this.DynamoDBContext.LoadAsync<SkydiverInfo>(email).Result;

        if (skydiver is null)
        {
            throw new SkydiverNotFoundException(email);
        }

        this.DynamoDBContext.DeleteAsync<SkydiverInfo>(email).Wait();
        this.DynamoDBContext.DeleteAsync<SkydiverInfo>($"uspaNumber#{skydiver.USPAMembershipNumber}").Wait();
    }

    public SkydiverInfo Register(in SkydiverInfo skydiverInfo, in string password)
    {
        SkydiverInfo? skydiver = this.DynamoDBContext.LoadAsync<SkydiverInfo>(skydiverInfo.Email).Result;

        // https://aws.amazon.com/blogs/database/simulating-amazon-dynamodb-unique-constraints-using-transactions/
        SkydiverInfo? skydiverByUSPANumber = this.DynamoDBContext
            .LoadAsync<SkydiverInfo>(
                $"uspaNumber#{skydiverInfo.USPAMembershipNumber}")
            .Result;

        if (skydiver is not null)
        {
            throw new SkydiverAlreadyExistsException(skydiverInfo.Email!);
        }
        if (skydiverByUSPANumber is not null)
        {
            throw new USPANumberTakenException(skydiverInfo.USPAMembershipNumber);
        }

        skydiver = new()
        {
            Email = skydiverInfo.Email,
            FirstName = skydiverInfo.FirstName,
            LastName = skydiverInfo.LastName,
            PasswordHash = HashPassword(password),
            USPAMembershipNumber = skydiverInfo.USPAMembershipNumber,
            USPALicenseNumber = skydiverInfo.USPALicenseNumber,
        };
        this.DynamoDBContext.SaveAsync<SkydiverInfo>(skydiver).Wait();
        this.DynamoDBContext.SaveAsync<SkydiverInfo>(new SkydiverInfo() { Email = $"uspaNumber#{skydiverInfo.USPAMembershipNumber}" }).Wait();
        return skydiver;
    }
}