using Microsoft.AspNetCore.Mvc;

namespace AuthenticationService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }


        [HttpPost("login")]
        public async Task<ActionResult<AuthenticatedResponse>> Login(User user)
        {
            if (user is null)
                return BadRequest("Invalid client request");

            var users = await _authService.GetUsers();

            var userFound = users.FirstOrDefault(x => x.UserName == user.UserName && x.Password == user.Password);

            if (userFound is not null)
            {
                var response = await _authService.Login(user);
                return response;
            }

            return Unauthorized();
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register(User user)
        {
            var users = await _authService.GetUsers();
            var userFound = users.FirstOrDefault(x => x.UserName == user.UserName);

            if(userFound is not null)
                return BadRequest("Benutzer existiert bereits");

            await _authService.Register(user);

            return Ok();
        }
    }
}