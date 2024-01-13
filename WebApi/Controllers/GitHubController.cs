using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Octokit;
using Swashbuckle.AspNetCore.Annotations;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GitHubController : ControllerBase
    {
        private readonly IGitHubService _gitHubService;

        public GitHubController(IGitHubService gitHubService)
        {
            _gitHubService = gitHubService;
        }

        [HttpGet("repositories")]
        public async Task<ActionResult<IEnumerable<Repository>>> GetRepositories()
        {
            var repositories = await _gitHubService.GetRepositories();
            return Ok(repositories);
        }
        [HttpGet("portfolio")]
        public async Task<ActionResult<IEnumerable<Repository>>> GetPortfolio()
        {
            var repositories = await _gitHubService.GetPortfolio();
            return Ok(repositories);
        }
        [HttpGet("searchRepositories")]
        public async Task<ActionResult<IEnumerable<string>>> SearchRepositories([FromQuery][SwaggerParameter(Required = false)] string? name, [FromQuery][SwaggerParameter(Required = false)] string? language, [FromQuery][SwaggerParameter(Required = false)] string? userName)
        {
            var repositories = await _gitHubService.SearchRepositories(name,language,userName);
            return Ok(repositories);
        }
    }
}
