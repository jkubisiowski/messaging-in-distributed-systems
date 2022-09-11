using SuperStore.Funds.Messages;
using SuperStore.Shared;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMessaging();
var app = builder.Build();

app.MapGet("/", () => "Funds service");
app.MapGet("/message/send", async (IMessagePublisher messagePublisher) =>
{
    var message = new FundsMessage(123, 20.00m);
    await messagePublisher.PublishAsync("Funds", "FundsMessage", message);
});

app.Run();