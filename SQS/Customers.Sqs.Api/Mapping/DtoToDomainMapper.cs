using Customers.Sqs.Api.Contracts.Data;
using Customers.Sqs.Api.Domain;

namespace Customers.Sqs.Api.Mapping;

public static class DtoToDomainMapper
{
    public static Customer ToCustomer(this CustomerDto customerDto)
    {
        return new Customer
        {
            Id = customerDto.Id,
            Email = customerDto.Email,
            GitHubUsername = customerDto.GitHubUsername,
            FullName = customerDto.FullName,
            DateOfBirth = customerDto.DateOfBirth
        };
    }
}
