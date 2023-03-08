using Customers.Sns.Api.Contracts.Messages;
using Customers.Sns.Api.Domain;

namespace Customers.Sns.Api.Mapping;

public static class DomainToMessageMapper
{
    public static CustomerCreated ToCustomerCreated(this Customer customer)
    {
        return new CustomerCreated()
        {
            Id = customer.Id,
            Email = customer.Email,
            FullName = customer.FullName,
            DateOfBirth = customer.DateOfBirth,
            GitHubUsername = customer.GitHubUsername
        };
    }
    
    public static CustomerUpdated ToCustomerUpdated(this Customer customer)
    {
        return new CustomerUpdated()
        {
            Id = customer.Id,
            Email = customer.Email,
            FullName = customer.FullName,
            DateOfBirth = customer.DateOfBirth,
            GitHubUsername = customer.GitHubUsername
        };
    }
}