using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace AuthenticationService
{

    public class AuthService
    {
        private readonly IConfiguration _config;
        private List<User> Users { get; set; }

        public AuthService(IConfiguration config)
        {
            _config = config;
            Users = LoadUsers();
        }


        public async Task<AuthenticatedResponse> Login(User user)
        {

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetValue<string>("Jwt:SecurityKey")));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var tokeOptions = new JwtSecurityToken(
                issuer: _config.GetValue<string>("Jwt:Issuer"),
                audience: _config.GetValue<string>("Jwt:Issuer"),



                claims: new List<Claim>(){
                        new Claim("sub", user.UserName),
                        new Claim("username", user.UserName),
                        new Claim("MeinPayload", "#irgendwas")
                },
                expires: DateTime.Now.AddMinutes(5),
                signingCredentials: signinCredentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
            var AuthenticatedResponse = new AuthenticatedResponse { Token = tokenString };

            return AuthenticatedResponse;

        }


        public Task Register(User user)
        {
            Users.Add(user);
            return Task.CompletedTask;
        }

        public async Task<List<User>> GetUsers()
        {
            return Users;
        }


        private List<User> LoadUsers()
        {
            var users = _config.GetSection("Users").Get<List<User>>();
            return users;
        }
    }
}