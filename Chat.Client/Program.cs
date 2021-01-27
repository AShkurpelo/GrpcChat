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
            var userName = GreetUser();

			var channel = GrpcChannel.ForAddress("http://localhost:50051");
			var client = new Messaging.MessagingClient(channel);

            using (var chatClient = client.Join())
            {
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
                }

                await chatClient.RequestStream.CompleteAsync();
            }

            Console.WriteLine("Disconnected. Press any key to exit.");
            Console.ReadKey();
        }

        static string GreetUser()
		{
            Console.Write("Enter your chat-name: ");
            var userName = Console.ReadLine();
            Console.WriteLine($"\nWelcome, {userName}.");
            Console.WriteLine("Send empty message to quit.\n\n");

            return userName;
        }

        static void PrintMessage(Message message)
		{
            var hasUser = message.Name != null;

            var cursorPos = Console.GetCursorPosition();
            Console.SetCursorPosition(0, cursorPos.Top - 1);
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write($"{DateTime.Now:HH:mm:ss} - ");
            if (hasUser)
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
            Console.SetCursorPosition(cursorPos.Left, cursorPos.Top);
        }
	}
}
