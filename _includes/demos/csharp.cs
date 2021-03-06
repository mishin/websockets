using System;
using System.Text;
using System.Threading.Tasks;
using System.Net.WebSockets;
using System.Threading;
using System.Net;

namespace ConsoleApp
{
    class Program
    {
        static async Task SendTicksRequest()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;
            var ws = new ClientWebSocket();
            var uri = new Uri("wss://ws.binaryws.com/websockets/v3");

            await ws.ConnectAsync(uri, CancellationToken.None);

            var reqAsBytes = Encoding.UTF8.GetBytes("{\"ticks\":\"R_100\"}");
            var ticksRequest = new ArraySegment<byte>(reqAsBytes);

            await ws.SendAsync(ticksRequest,
                WebSocketMessageType.Text,
                true,
                CancellationToken.None);

            var buffer = new ArraySegment<byte>(new byte[1024]);
            var result = await ws.ReceiveAsync(buffer, CancellationToken.None);

            string response = Encoding.UTF8.GetString(buffer.Array, 0, result.Count);
            Console.WriteLine(response);
        }

        static void Main(string[] args)
        {
            SendTicksRequest();
            Console.ReadLine();
        }
    }
}
