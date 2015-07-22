using System;
using System.Text;
using RabbitMQ.Client;

namespace DirectExchangePublisher
{
	public class EmitLogDirect
	{
		public static void Main(string[] args)
		{
			var factory = new ConnectionFactory() {HostName = "localhost"};
			using (var connection = factory.CreateConnection())
			{
				using (var channel = connection.CreateModel())
				{
					channel.ExchangeDeclare("direct_logs", "direct");
					while (true)
					{
						var severity = ReadSeverity();
						var message = ReadMessage();
						var body = Encoding.UTF8.GetBytes(message);
						channel.BasicPublish("direct_logs", severity, null, body);
						Console.WriteLine(" [x] Sent '{0}:{1}'", severity, message);
					}
				}
			}
		}

		private static string ReadMessage()
		{
			Console.WriteLine("Enter a log message.");
			var arg = Console.ReadLine();
			var message = String.IsNullOrEmpty(arg) ? "Hello World!" : arg;
			Console.WriteLine("Log=" + message);
			return message;
		}

		private static string ReadSeverity()
		{
			Console.WriteLine("Enter a severity (info , warning , error)");
			var arg = Console.ReadLine().ToLower();
			var severity = String.IsNullOrEmpty(arg) ? "info" : arg;
			return severity;
		}
	}
}
