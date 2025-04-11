using System.Net.Sockets;
using System.Text;

namespace POS.Printers;

public class TcpPrinter : IPrinter
{
    private readonly string ipAddress;
    private readonly int port;

    public TcpPrinter(string ipAddress, int? port)
    {
        this.ipAddress = ipAddress;
        this.port = port ?? 9100;
    }

    public async Task Print(string command)
    {
        using var client = new TcpClient();
        await client.ConnectAsync(ipAddress, port);

        using var stream = client.GetStream();
        var data = Encoding.UTF8.GetBytes(command);
        await stream.WriteAsync(data, 0, data.Length);
    }
}

