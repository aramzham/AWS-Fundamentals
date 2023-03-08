using System.Net;
using System.Text.Json;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Customers.S3.Api.Contracts.Data;

namespace Customers.S3.Api.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly IAmazonDynamoDB _dynamoDb;
    private const string TableName = "customers";

    public CustomerRepository(IAmazonDynamoDB dynamoDb)
    {
        _dynamoDb = dynamoDb;
    }

    public async Task<bool> CreateAsync(CustomerDto customer)
    {
        customer.UpdatedAt = DateTime.UtcNow;
        var customerJson = JsonSerializer.Serialize(customer);
        var customerAttributeDict = Document.FromJson(customerJson).ToAttributeMap();

        // put is used for both update and delete
        var createItemRequest = new PutItemRequest()
        {
            TableName = TableName,
            Item = customerAttributeDict
        };

        var result = await _dynamoDb.PutItemAsync(createItemRequest);
        return result.HttpStatusCode == HttpStatusCode.OK;
    }

    public async Task<CustomerDto?> GetAsync(Guid id)
    {
        var getItemRequest = new GetItemRequest()
        {
            TableName = TableName,
            Key = new Dictionary<string, AttributeValue>()
            {
                { "pk", new AttributeValue() { S = id.ToString() } },
                { "sk", new AttributeValue() { S = id.ToString() } }
            }
        };

        var response = await _dynamoDb.GetItemAsync(getItemRequest);
        if (response.Item.Count == 0)
            return null;

        var itemDocument = Document.FromAttributeMap(response.Item);
        return JsonSerializer.Deserialize<CustomerDto>(itemDocument.ToJson());
    }

    public async Task<CustomerDto?> GetByEmailAsync(string email)
    {
        var queryRequest = new QueryRequest()
        {
            TableName = TableName,
            IndexName = "email-id-index", // with global secondary index your table will be duplicated to store values in different way => be careful when using it because your inserts will be performed twice now!!
            KeyConditionExpression = "Email = :v_Email",
            ExpressionAttributeValues = new Dictionary<string, AttributeValue>()
            {
                { ":v_Email", new AttributeValue() { S = email } }
            }
        };
        
        var response = await _dynamoDb.QueryAsync(queryRequest);
        if (response.Items.Count == 0)
            return null;

        var itemDocument = Document.FromAttributeMap(response.Items[0]); // email is unique => we can return first item
        return JsonSerializer.Deserialize<CustomerDto>(itemDocument.ToJson());
    }

    public async Task<IEnumerable<CustomerDto>> GetAllAsync()
    {
        // don't do such scans because when you have a lot of items this operation may become insanely costly $$$
        var scanRequest = new ScanRequest()
        {
            TableName = TableName
        };

        return (await _dynamoDb.ScanAsync(scanRequest)).Items.Select(x =>
        {
            var json = Document.FromAttributeMap(x).ToJson();
            return JsonSerializer.Deserialize<CustomerDto>(json);
        })!;
    }

    public async Task<bool> UpdateAsync(CustomerDto customer, DateTime requestStarted)
    {
        customer.UpdatedAt = DateTime.UtcNow;
        var customerJson = JsonSerializer.Serialize(customer);
        var customerAttributeDict = Document.FromJson(customerJson).ToAttributeMap();

        // put is used for both update and delete
        var updateItemRequest = new PutItemRequest()
        {
            TableName = TableName,
            Item = customerAttributeDict,
            ConditionExpression = "UpdatedAt < :requestStarted",
            ExpressionAttributeValues = new Dictionary<string, AttributeValue>()
            {
                { ":requestStarted", new AttributeValue() { S = requestStarted.ToString("O") } }
            }
        };

        var result = await _dynamoDb.PutItemAsync(updateItemRequest);
        return result.HttpStatusCode == HttpStatusCode.OK;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var deleteItemRequest = new DeleteItemRequest()
        {
            TableName = TableName,
            Key = new Dictionary<string, AttributeValue>()
            {
                { "pk", new AttributeValue() { S = id.ToString() } },
                { "sk", new AttributeValue() { S = id.ToString() } }
            }
        };

        var response = await _dynamoDb.DeleteItemAsync(deleteItemRequest);
        return response.HttpStatusCode == HttpStatusCode.OK;
    }
}