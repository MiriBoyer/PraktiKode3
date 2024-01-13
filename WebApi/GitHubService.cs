using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Octokit;
using Swashbuckle.AspNetCore.Annotations;

namespace WebApi
{
    public class GitHubService : IGitHubService
    {
        private readonly GitHubClient _client;
        private readonly GitHubIntegrationOptions _options;

        //public GitHubService(string accessToken)
        //{
        //    _client = new GitHubClient(new ProductHeaderValue("MyApp"));
        //    _client.Credentials = new Credentials(accessToken);
        //}

        public GitHubService(IOptions<GitHubIntegrationOptions> options)
        {
            _client = new GitHubClient(new ProductHeaderValue("MyApp"));
            _options = options.Value;
            var token = _options.GitHubToken.ToString();
            _client.Credentials = new Credentials(token);
        }


        public async Task<IReadOnlyList<Repository>> GetRepositories()
        {
            return await _client.Repository.GetAllForCurrent();
        }
        public async Task<List<RepositoryInfo>> GetPortfolio()
        {
            var repositories = await GetRepositories();

            var repositoryInfos = new List<RepositoryInfo>();

            foreach (var repository in repositories)
            {
                var language = repository.Language;
                var commits = await _client.Repository.Commit.GetAll(repository.Owner.Login, repository.Name);
                var lastCommit = commits.First();
                Console.WriteLine(lastCommit.ToString());
                var stars = repository.StargazersCount;
                Console.WriteLine(stars.ToString());
                var pullRequests = await _client.PullRequest.GetAllForRepository(repository.Owner.Login, repository.Name);
                Console.WriteLine(pullRequests.ToString());
                var website = repository.GitUrl;

                var repositoryInfo = new RepositoryInfo
                {
                    Name = repository.Name,
                    Language = language,
                    LastCommit = lastCommit.Commit.Message,
                    Stars = stars,
                    PullRequests = pullRequests.Count,
                    Website = website
                };

                repositoryInfos.Add(repositoryInfo);
            }

            return repositoryInfos;
        }
        public async Task<List<string>> SearchRepositories([FromQuery][SwaggerParameter(Required = false)] string? name, [FromQuery][SwaggerParameter(Required = false)] string? language, [FromQuery][SwaggerParameter(Required = false)] string? userName)
        {
            List<string> repositoryList = new List<string>();
            var repositories = await GetRepositories();
            foreach (var repository in repositories)
            {
                if (name != null && repository.Name != name)
                    Console.WriteLine(repository.Name);
                else
                {
                    if (language != null && repository.Language != language)
                        Console.WriteLine(repository.Language);
                    else
                    {
                        //if (userName != "" && _client.User.ToString() != userName)
                        //    Console.WriteLine(_client.User);
                        //else
                        {
                            Console.WriteLine(repository.Name);
                            repositoryList.Add(repository.Name);
                        }
                    }
                }
            }
            return repositoryList;
        }
    }
    public class RepositoryInfo
    {
        public string Name { get; set; }
        public string Language { get; set; }
        public string LastCommit { get; set; }
        public int Stars { get; set; }
        public int PullRequests { get; set; }
        public string Website { get; set; }
    }
}