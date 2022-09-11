using RabbitMQ.Client;

namespace SuperStore.Shared.Connections;

internal sealed class ChannelFactory : IChannelFactory
{
    private readonly IConnection connection;
    private readonly ChannelAccessor channelAccessor;

    public ChannelFactory(IConnection connection, ChannelAccessor channelAccessor)
    {
        this.connection = connection;
        this.channelAccessor = channelAccessor;
    }

    public IModel Create() => channelAccessor.Channel ?? (channelAccessor.Channel = connection.CreateModel());
}