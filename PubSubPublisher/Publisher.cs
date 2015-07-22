using System;
using System.Text;
using RabbitMQ.Client;

namespace PubSubPublisher
{
	public class Publisher
	{
		private const string EmptyRoutingKey = "";
		private const string ExchangeName = "logs";
		private const string ExchangeType = "fanout";

		public static void Main(string[] args)
		{
			var factory = new ConnectionFactory() { HostName = "localhost" };
			using (var connection = factory.CreateConnection())
			using (var channel = connection.CreateModel())
			{
				channel.ExchangeDeclare(ExchangeName, ExchangeType);

				while (true)
				{
					Console.WriteLine("Enter a log or x ctrl-c to exit");
					var message = ReadMessage();
					channel.BasicPublish(ExchangeName, EmptyRoutingKey, null, message);
					Console.WriteLine(" [Pub] Sending ...");
				}
			}
		}

		private static byte[] ReadMessage()
		{
			var arg = Console.ReadLine();
			var message = String.IsNullOrEmpty(arg) ? "Hello World!" : arg;
			Console.WriteLine("Log=" + message);
			return Encoding.UTF8.GetBytes(message);
		}
	}
}
