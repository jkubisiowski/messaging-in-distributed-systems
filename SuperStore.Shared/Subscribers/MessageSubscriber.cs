using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace SuperStore.Shared.Subscribers;

internal sealed class MessageSubscriber : IMessageSubscriber
{
    private readonly IModel channel;

    public MessageSubscriber(IChannelFactory channelFactory) => channel = channelFactory.Create();


    public IMessageSubscriber SubscribeMessage<TMessage>(string queue, string routingKey, string exchange,
        Func<TMessage, BasicDeliverEventArgs, Task> handle) where TMessage : class, IMessage
    {
        channel.QueueDeclare(queue, durable: false, autoDelete: false, exclusive: false);
        channel.QueueBind(queue, exchange, routingKey);

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = JsonSerializer.Deserialize<TMessage>(Encoding.UTF8.GetString(body));
            await handle(message, ea);
        };

        channel.BasicConsume(queue, autoAck: true, consumer: consumer);
        return this;
    }
}