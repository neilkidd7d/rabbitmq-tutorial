using System;
using System.Linq;
using System.Text;
using RabbitMQ.Client;

namespace TopicExchangePublisher
{
	class TopicPublisher
	{
		private const string ExchangeName = "topic_logs";
		private const string ExchangeType = "topic";

		public static void Main(string[] args)
		{
			var factory = new ConnectionFactory() { HostName = "localhost" };
			using (var connection = factory.CreateConnection())
			{
				using (var channel = connection.CreateModel())
				{
					channel.ExchangeDeclare(ExchangeName, ExchangeType);

					var routingKey = (args.Length > 0) ? args[0] : "anonymous.info";
					var message = (args.Length > 1) ? string.Join(" ", args.Skip(1).ToArray()) : "Hello World!";
					var body = Encoding.UTF8.GetBytes(message);
					channel.BasicPublish(ExchangeName, routingKey, null, body);
					Console.WriteLine(" [x] Sent '{0}':'{1}'", routingKey, message);
				}
			}
		}
	}
}
