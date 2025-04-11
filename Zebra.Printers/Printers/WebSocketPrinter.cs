using System.Net.WebSockets;
using System.Text;

namespace POS.Printers;

public class WebSocketPrinter : IPrinter
{
    private readonly string uri;

    public WebSocketPrinter(string uri)
    {
        this.uri = uri;
    }

    public async Task Print(string command)
    {
        using var client = new ClientWebSocket();
        await client.ConnectAsync(new Uri(uri), CancellationToken.None);

        var data = Encoding.UTF8.GetBytes(command);
        await client.SendAsync(new ArraySegment<byte>(data), WebSocketMessageType.Text, true, CancellationToken.None);
        await client.CloseAsync(WebSocketCloseStatus.NormalClosure, "Print complete", CancellationToken.None);
    }
}
