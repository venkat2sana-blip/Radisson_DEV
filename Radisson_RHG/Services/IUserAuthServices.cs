using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Radisson_RHG.Services
{
    public interface IUserAuthServices
    {
        string? Authenticate(string userName, string password);
    }

    public class UserAuthServices:IUserAuthServices
    {
        private readonly IRepositoryUserInterface _iuser;
        private readonly IConfiguration _config;

        public UserAuthServices(IRepositoryUserInterface useri,IConfiguration configu)
        {
            _iuser = useri;
            _config = configu;
        }

        public string? Authenticate(string userName,string password)
        {
            var user = _iuser.GetByUserName(userName);
            if (user == null)
                return null;
            if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                return null;

            var jwtSection = _config.GetSection("JWT");
            var key=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSection["Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
              new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
              new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
              new Claim("id",user.Id.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: jwtSection["Issuer"],
                audience: jwtSection["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(double.Parse(jwtSection["ExpireMinutes"])),
                signingCredentials:creds
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
