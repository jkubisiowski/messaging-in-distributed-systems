using SuperStore.Carts.Messages;
using SuperStore.Carts.Services;
using SuperStore.Shared;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMessaging();
builder.Services.AddHostedService<MessagingBackgroundService>();

var app = builder.Build();

app.MapGet("/", () => "Funds Service!");
app.MapGet("/message/send/EU/{country}", async (IMessagePublisher messagePublisher, string country) =>
{
    var message = new FundsMessage(123, 10.00m);
    await messagePublisher.PublishAsync("Funds", $"EU.{country}", message);
});

app.Run("http://localhost:5002");
