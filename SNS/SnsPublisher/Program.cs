using System.Text.Json;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using SnsPublisher;

var message = new CustomerCreated()
{
    Email = "aramz@yandex.ru",
    Id = Guid.NewGuid(),
    FullName = "Dustin Poirier",
    DateOfBirth = new DateTime(1990, 5, 26),
    GitHubUsername = "aramzham"
};

var snsClient = new AmazonSimpleNotificationServiceClient();

var topicArnResponse = await snsClient.FindTopicAsync("customers");

var request = new PublishRequest()
{
    TopicArn = topicArnResponse.TopicArn,
    Message = JsonSerializer.Serialize(message),
    MessageAttributes = new Dictionary<string, MessageAttributeValue>()
    {
        {
            "MessageType", new MessageAttributeValue()
            {
                DataType = "String",
                StringValue = nameof(CustomerCreated)
            }
        }
    }
};

var result = await snsClient.PublishAsync(request);
