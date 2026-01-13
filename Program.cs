using System.Net.WebSockets;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Enable WebSockets
var webSocketOptions = new WebSocketOptions
{
    KeepAliveInterval = TimeSpan.FromSeconds(30)
};
app.UseWebSockets(webSocketOptions);

// Health check
app.MapGet("/health", () => Results.Ok(new
{
    status = "ok",
    service = "SimpleWebSocket"
}));

// WebSocket endpoint
app.Map("/ws", async context =>
{
    if (!context.WebSockets.IsWebSocketRequest)
    {
        context.Response.StatusCode = 400;
        return;
    }

    using var webSocket = await context.WebSockets.AcceptWebSocketAsync();
    Console.WriteLine("WebSocket client connected");

    var buffer = new byte[1024 * 4];

    while (webSocket.State == WebSocketState.Open)
    {
        var result = await webSocket.ReceiveAsync(
            new ArraySegment<byte>(buffer),
            CancellationToken.None
        );

        if (result.MessageType == WebSocketMessageType.Close)
        {
            Console.WriteLine("WebSocket client disconnected");
            await webSocket.CloseAsync(
                WebSocketCloseStatus.NormalClosure,
                "Closed by server",
                CancellationToken.None
            );
        }
        else
        {
            var clientMessage = Encoding.UTF8.GetString(buffer, 0, result.Count);
            Console.WriteLine($"Received: {clientMessage}");

            var serverMessage = Encoding.UTF8.GetBytes($"Echo from server: {clientMessage}");
            await webSocket.SendAsync(
                new ArraySegment<byte>(serverMessage),
                WebSocketMessageType.Text,
                true,
                CancellationToken.None
            );
        }
    }
});

app.Run();
