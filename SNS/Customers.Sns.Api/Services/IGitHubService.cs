namespace Customers.Sns.Api.Services;

public interface IGitHubService
{
    Task<bool> IsValidGitHubUser(string username);
}
