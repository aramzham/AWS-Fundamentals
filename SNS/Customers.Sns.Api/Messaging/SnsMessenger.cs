using System.Text.Json;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Microsoft.Extensions.Options;

namespace Customers.Sns.Api.Messaging;

public class SnsMessenger : ISnsMessenger
{
    private readonly IAmazonSimpleNotificationService _sns;
    private readonly IOptions<TopicSettings> _settings;
    private string? _topicArn;

    public SnsMessenger(IAmazonSimpleNotificationService sns, IOptions<TopicSettings> settings)
    {
        _sns = sns;
        _settings = settings;
    }

    private async ValueTask<string> GetTopicArn()
    {
        if (_topicArn is not null) // think about thread safety
            return _topicArn;

        var response = await _sns.FindTopicAsync(_settings.Value.Name);
        _topicArn = response.TopicArn;
        return _topicArn;
    }

    public async Task<PublishResponse> PublishMessageAsync<T>(T message)
    {
        var topicArn = await GetTopicArn();

        var publishRequest = new PublishRequest()
        {
            TopicArn = topicArn,
            MessageAttributes = new Dictionary<string, MessageAttributeValue>()
            {
                {
                    "MessageType", new MessageAttributeValue()
                    {
                        DataType = "String",
                        StringValue = typeof(T).Name
                    }
                }
            },
            Message = JsonSerializer.Serialize(message)
        };

        return await _sns.PublishAsync(publishRequest);
    }
}