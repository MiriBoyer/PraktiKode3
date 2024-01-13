using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Caching.Memory;
using Octokit;
using Swashbuckle.AspNetCore.Annotations;

namespace WebApi.CachedServices
{
    public class CachedGitHubService : IGitHubService
    {
        private readonly IGitHubService _gitHubService;
        private readonly IMemoryCache _memoryCache;

        private const string ListProtfolioKey = "ListProtfolioKey";
        public CachedGitHubService(IGitHubService gitHubService, IMemoryCache memoryCache)
        {
            _gitHubService = gitHubService;
            _memoryCache = memoryCache;
        }

        public async Task<List<RepositoryInfo>> GetPortfolio()
        {
            if (_memoryCache.TryGetValue(ListProtfolioKey, out List<RepositoryInfo> repositoryInfo))
                return repositoryInfo;

            var cacheOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(30)).SetSlidingExpiration(TimeSpan.FromSeconds(10));
            repositoryInfo = await _gitHubService.GetPortfolio();
            _memoryCache.Set(ListProtfolioKey, repositoryInfo, cacheOptions);
            return repositoryInfo;
        }

        public async Task<IReadOnlyList<Repository>> GetRepositories()
        {
            return await _gitHubService.GetRepositories();
        }

        public async Task<List<string>> SearchRepositories([FromQuery][SwaggerParameter(Required = false)] string? name, [FromQuery][SwaggerParameter(Required = false)] string? language, [FromQuery][SwaggerParameter(Required = false)] string? userName)
        {
            return await _gitHubService.SearchRepositories(name, language, userName);
        }
    }
}
