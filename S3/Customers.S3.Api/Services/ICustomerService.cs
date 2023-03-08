using Customers.S3.Api.Domain;

namespace Customers.S3.Api.Services;

public interface ICustomerService
{
    Task<bool> CreateAsync(Customer customer);

    Task<Customer?> GetAsync(Guid id);

    Task<Customer?> GetByEmailAsync(string email);
    
    Task<IEnumerable<Customer>> GetAllAsync();

    Task<bool> UpdateAsync(Customer customer, DateTime requestStarted);

    Task<bool> DeleteAsync(Guid id);
}
