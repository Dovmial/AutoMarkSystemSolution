using AmsAPI.Autorize.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AmsAPI.Autorize.Features
{
    public class JwtTokenGenerator(IOptions<AuthSettings> options)
    {
        public string Generate(Account account)
        {
            List<Claim> claims =
            [
                new Claim(ClaimTypes.NameIdentifier, account.Login),
                new Claim(JwtRegisteredClaimNames.Jti, account.Id.ToString()),
                .. account.Roles.Select(x => new Claim(ClaimTypes.Role, x.Name)),
            ];

            byte[] secretKey = Encoding.UTF8.GetBytes(options.Value.SecretKey);
            SymmetricSecurityKey symmetricKey = new SymmetricSecurityKey(secretKey);
            SigningCredentials credentials = new(symmetricKey, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken jwtToken = new(
                expires: DateTime.UtcNow.Add(options.Value.Expires),
                claims: claims,
                signingCredentials: credentials);

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(jwtToken);
        }
    }
}
