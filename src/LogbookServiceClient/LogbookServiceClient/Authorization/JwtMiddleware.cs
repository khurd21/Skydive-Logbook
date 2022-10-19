using Amazon.DynamoDBv2.DataModel;
using LogbookService.Records;

namespace Logbook.Authorization;

/// <summary>
/// https://learn.microsoft.com/en-us/aspnet/core/fundamentals/middleware/write?view=aspnetcore-6.0
/// </summary>
public class JwtMiddleware
{
    private RequestDelegate Next { get; init; }

    public JwtMiddleware(RequestDelegate next)
    {
        this.Next = next;
    }

    public async Task InvokeAsync(HttpContext context, IDynamoDBContext dynamoDBContext, IJwtUtils jwtUtils)
    {
        string? token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        if (token is not null)
        {
            string? email = jwtUtils.ValidateJwtToken(token);

            if (email is not null)
            {
                SkydiverInfo? skydiverInfo = dynamoDBContext.LoadAsync<SkydiverInfo>(email).Result;

                if (skydiverInfo is not null)
                {
                    context.Items["SkydiverInfo"] = skydiverInfo;
                }
            }
        }

        await this.Next(context);
    }
}