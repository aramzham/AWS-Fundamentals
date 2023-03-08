using Amazon.SQS.Model;

namespace Customers.Sqs.Api.Messaging;

public interface ISqsMessenger
{
    Task<SendMessageResponse> SendMessageAsync<T>(T message) /*you may want to use some kind of IMessage contract for all kind of messages => where T : IMessage like*/;
}