using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Octokit;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi
{
    public interface IGitHubService
    {
        Task<IReadOnlyList<Repository>> GetRepositories();
        Task<List<RepositoryInfo>> GetPortfolio();
        Task<List<string>> SearchRepositories([FromQuery][SwaggerParameter(Required = false)] string? name, [FromQuery][SwaggerParameter(Required = false)] string? language, [FromQuery][SwaggerParameter(Required = false)] string? userName);
    }
}
