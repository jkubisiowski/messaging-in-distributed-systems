using System.Text;
using System.Text.Json;
using RabbitMQ.Client;

namespace SuperStore.Shared.Publishers;

internal sealed class MessagePublisher : IMessagePublisher
{
    private readonly IModel channel;


    public MessagePublisher(IChannelFactory channelFactory) => channel = channelFactory.Create();

    public Task PublishAsync<TMessage>(string exchange, string routingKey, TMessage message) where TMessage : class, IMessage
    {
        var json = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(json);

        var properties = channel.CreateBasicProperties();
        
        channel.ExchangeDeclare(exchange, "topic", false, false);
        channel.BasicPublish(exchange, routingKey, properties, body);
        return Task.CompletedTask;
    }
}