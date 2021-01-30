using Grpc.Core;
using Grpc.Net.Client;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Chat.Client
{
	class Program
	{
		static async Task Main(string[] args)
		{
            var userName = UserIntro();

			var channel = GrpcChannel.ForAddress("http://localhost:50051");
			var client = new Messaging.MessagingClient(channel);

            using (var chatClient = client.Join())
            {
                var initMessage = new Message { Name = userName };
                await chatClient.RequestStream.WriteAsync(initMessage);

                _ = Task.Run(async () =>
                {
                    await foreach (var message in chatClient.ResponseStream.ReadAllAsync())
                    {
                        PrintMessage(message);
                    }
                });

                var line = Console.ReadLine();
                while (!string.IsNullOrEmpty(line))
                {
                    await chatClient.RequestStream.WriteAsync(new Message { Name = userName, Message_ = line });
                    line = Console.ReadLine();
                    Console.CursorTop--;
                }

                await chatClient.RequestStream.CompleteAsync();
            }

            Console.WriteLine("Disconnected. Press any key to exit.");
            Console.ReadKey();
        }

        static string UserIntro()
		{
            Console.Write("Enter your chat-name: ");
            var userName = Console.ReadLine();
            Console.WriteLine($"\nWelcome, {userName}.");
			Console.WriteLine("Use input to send messages.");
            Console.WriteLine("Send empty message to quit.\n\n");

            return userName;
        }

        static void PrintMessage(Message message)
		{
            Console.SetCursorPosition(0, Console.CursorTop);

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write($"{DateTime.Now:HH:mm:ss} - ");

            if (!string.IsNullOrEmpty(message.Name))
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write($"{message.Name}: ");
                Console.ResetColor();
            }
			else // system message
			{
                Console.ForegroundColor = ConsoleColor.Cyan;
			}

            Console.WriteLine(message.Message_);
            Console.ResetColor();
            Console.SetCursorPosition(0, Console.CursorTop + 1);
        }
	}
}
