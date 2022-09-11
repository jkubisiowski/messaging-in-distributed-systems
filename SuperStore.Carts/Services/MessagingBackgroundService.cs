using System.Runtime.CompilerServices;
using RabbitMQ.Client;
using SuperStore.Carts.Messages;
using SuperStore.Shared;

namespace SuperStore.Carts.Services;

internal sealed class MessagingBackgroundService : BackgroundService
{
    private readonly IMessageSubscriber messageSubscriber;
    private readonly ILogger<MessagingBackgroundService> logger;
    private readonly IModel channel;


    public MessagingBackgroundService(IMessageSubscriber messageSubscriber, ILogger<MessagingBackgroundService> logger, IChannelFactory channelFactory)
    {
        this.messageSubscriber = messageSubscriber;
        this.logger = logger;
        this.channel = channelFactory.Create();
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        channel.ExchangeDeclare("Funds", "topic", false, false);
        messageSubscriber.SubscribeMessage<FundsMessage>("carts-service-funds-message", "FundsMessage", "Funds",
            (message, args) =>
            {
                logger.LogInformation(
                    $"{DateTime.Now} Received message for customer: {message.CustomerId} | Funds: {message.CurrentFunds} | RoutingKey: {args.RoutingKey}");
                return Task.CompletedTask;
            });
        return Task.CompletedTask;
    }
}