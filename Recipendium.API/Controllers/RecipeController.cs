using Core.HttpDynamo;
using Core.JwtBuilder;
using Microsoft.AspNetCore.Mvc;
using Recipendium.API.Models;
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
        public async Task<IActionResult> Get([FromQuery] string q, [FromQuery] int? limit)
        {
            try
            {
                var token = JwtTokenGenerator.GenerateToken(_config, new List<Claim>() );
                var wprmSites = new List<WPRMResult>();

                if (limit == null)
                    limit = 100;

                var searchResults = await HttpDynamo.GetRequestAsync<SearchResponse>(_httpClientFactory, "https://searchcustomgoogle20221208153914.azurewebsites.net/CustomGoogle?q=" + q + "&limit=" + limit, token, null);

                if (searchResults != null)
                {
                    var searchArr = searchResults.Items.ToArray();

                    wprmSites = await HttpDynamo.PostRequestAsync<List<WPRMResult>>(_httpClientFactory, "https://recipeparser20221210093759.azurewebsites.net/Ingredients/WPRM", token, searchArr, null);
                }

                return Ok(wprmSites);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}