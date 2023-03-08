using System.Text.Json;
using Amazon.Runtime;
using Amazon.SQS;
using Amazon.SQS.Model;
using SqsPublisher;

var sqsClient = new AmazonSQSClient();

var customer = new CustomerCreated()
{
    Email = "aram@mail.ru",
    Id = Guid.NewGuid(),
    FullName = "Aram Zhamkochyan",
    DateOfBirth = new DateTime(1990, 5, 26),
    GitHubUsername = "aramzham"
};

var queueUrlResponse = await sqsClient.GetQueueUrlAsync("customers");

var sendMessageRequest = new SendMessageRequest()
{
    QueueUrl = queueUrlResponse.QueueUrl,
    MessageBody = JsonSerializer.Serialize(customer),
    MessageAttributes = new Dictionary<string, MessageAttributeValue>()
    {
        {"MessageType", new MessageAttributeValue()
        {
            DataType = "String",
            StringValue = nameof(CustomerCreated)
        }}
    }
};

var response = await sqsClient.SendMessageAsync(sendMessageRequest);

Console.WriteLine();