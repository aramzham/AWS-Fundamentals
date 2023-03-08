using System.Text.Json;
using Amazon.SQS;
using Amazon.SQS.Model;
using Microsoft.Extensions.Options;

namespace Customers.Sqs.Api.Messaging;

public class SqsMessenger : ISqsMessenger
{
    private readonly IAmazonSQS _sqs;
    private readonly IOptions<QueueSettings> _settings;
    private string? _queueUrl;

    public SqsMessenger(IAmazonSQS sqs, IOptions<QueueSettings> settings)
    {
        _sqs = sqs;
        _settings = settings;
    }

    public async Task<SendMessageResponse> SendMessageAsync<T>(T message)
    {
        var queueUrl = await GetQueueUrl();

        var sendMessageRequest = new SendMessageRequest()
        {
            QueueUrl = queueUrl,
            MessageAttributes = new Dictionary<string, MessageAttributeValue>()
            {
                {"MessageType", new MessageAttributeValue()
                {
                    DataType = "String",
                    StringValue = typeof(T).Name
                }}  
            },
            MessageBody = JsonSerializer.Serialize(message)
        };

        return await _sqs.SendMessageAsync(sendMessageRequest);
    }

    private async Task<string> GetQueueUrl()
    {
        if (_queueUrl is not null) // think about thread safety
            return _queueUrl;

        var response = await _sqs.GetQueueUrlAsync(_settings.Value.Name);
        _queueUrl = response.QueueUrl;
        return _queueUrl;
    }
}