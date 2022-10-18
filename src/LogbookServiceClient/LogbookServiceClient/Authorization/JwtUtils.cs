using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LogbookService.Records;
using LogbookService.Settings;
using Microsoft.IdentityModel.Tokens;

namespace Logbook.Authorization;

public class JwtUtils : IJwtUtils
{
    public string GenerateJwtToken(in SkydiverInfo skydiverInfo)
    {
        JwtSecurityTokenHandler tokenHandler = new();
        byte[] key = Encoding.ASCII.GetBytes(ProjectSettings.JwtToken);

        SecurityTokenDescriptor tokenDescriptor = new()
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, skydiverInfo.Email!),
            }),
            Expires = DateTime.UtcNow.AddDays(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public string? ValidateJwtToken(in string token)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            return null;
        }

        JwtSecurityTokenHandler tokenHandler = new();
        byte[] key = Encoding.ASCII.GetBytes(ProjectSettings.JwtToken);
        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            JwtSecurityToken jwtToken = (JwtSecurityToken)validatedToken;
            string email = jwtToken.Claims.First(x => x.Type == ClaimTypes.Name).Value;

            return email;
        }
        catch
        {
            return null;
        }
    }
}