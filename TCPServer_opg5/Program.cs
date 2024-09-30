using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using System.Threading.Tasks;

Console.WriteLine("TCP Server:");

TcpListener listener = new TcpListener(IPAddress.Any, 7);
listener.Start();
Console.WriteLine("Server started...");

while (true)
{
    TcpClient socket = listener.AcceptTcpClient();
    Task.Run(() => HandleClient(socket));
}

void HandleClient(TcpClient socket)
{
    NetworkStream ns = socket.GetStream();
    StreamReader reader = new StreamReader(ns);
    StreamWriter writer = new StreamWriter(ns);
    writer.AutoFlush = true;

    while (socket.Connected)
    {
        try
        {
            string message = reader.ReadLine().ToLower();
            if (string.IsNullOrEmpty(message))
            {
                writer.WriteLine("need to input");
            }

            var jsonMessage = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(message);
            var method = jsonMessage["method"].GetString().ToLower();
            int x = jsonMessage["x"].GetInt32();
            int y = jsonMessage["y"].GetInt32();

            if (message == "stop")
            {
                writer.Close();
            }

            else if (message == "random")
            {

                writer.WriteLine("Input numbers");
                

                Random random = new Random();
                int result = random.Next(x, y);

                var response = new { result = result };
                writer.WriteLine(JsonSerializer.Serialize(response));
               

            }
     
            else if (message.Contains("add"))
            {
                writer.WriteLine("Input numbers");
                

                int result = x + y;
                var response = new { result = result };
                writer.WriteLine(JsonSerializer.Serialize(response));
               
            }
            else if (message.Contains("Subtract"))
            {
                writer.WriteLine("Input numbers");
     
                int result = x - y;
                var response = new { result = result };
                writer.WriteLine(JsonSerializer.Serialize(response));
               
            }
        }
        catch (ArgumentOutOfRangeException ex)
        {
            writer.WriteLine($"Error {ex.Message}");
        }
    }
}