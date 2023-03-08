namespace Customers.Sqs.Api.Services;

public interface IGitHubService
{
    Task<bool> IsValidGitHubUser(string username);
}
