using Core.HttpDynamo;
using Core.JwtBuilder;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Recipendium.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RecipeController : ControllerBase
    {
        private readonly ILogger<RecipeController> _logger;
        private readonly IConfiguration _config;
        private readonly IHttpClientFactory _httpClientFactory;

        public RecipeController(ILogger<RecipeController> logger, IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _config = configuration;
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string q)
        {
            try
            {

                var token = JwtTokenGenerator.GenerateToken(_config, new[] { new Claim("source", "portfolio") });
                var resourceGroupList = await HttpDynamo.GetRequestAsync<List<string>>(_httpClientFactory, "https://recipeparser20221210093759.azurewebsites.net/Ingredients/WPRM", token, null);

                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}