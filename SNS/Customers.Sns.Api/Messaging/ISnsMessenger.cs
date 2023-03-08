using Amazon.SimpleNotificationService.Model;

namespace Customers.Sns.Api.Messaging;

public interface ISnsMessenger
{
    Task<PublishResponse> PublishMessageAsync<T>(T message) /*you may want to use some kind of IMessage contract for all kind of messages => where T : IMessage like*/;
}