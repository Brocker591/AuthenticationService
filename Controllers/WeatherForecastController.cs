using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationService.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }


        [HttpGet("GetWeatherForecast")]
        public ActionResult<string[]> Get()
        {
            return Summaries;
        }


        [HttpGet("/GetMyUserName")]
        public ActionResult<string> GetDummeAnwort()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var claims = identity.Claims;

            string result = claims.FirstOrDefault(x=> x.Type == "username" ).Value;
            return result;
        }


    }
}