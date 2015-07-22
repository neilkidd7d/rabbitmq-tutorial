using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace PubSubSubscriber
{
	public class Subscriber
	{
		private const string EmptyRoutingKey = "";
		private const string ExchangeName = "logs";
		private const string ExchangeType = "fanout";

		public static void Main()
		{
			var factory = new ConnectionFactory() { HostName = "localhost" };
			using (var connection = factory.CreateConnection())
			using (var channel = connection.CreateModel())
			{
				channel.ExchangeDeclare(ExchangeName, ExchangeType);

				//non-durable, exclusive, autodelete queue with a generated name
				var anonymousQueueName = channel.QueueDeclare().QueueName;
				Console.WriteLine("anonymousQueueName is:" + anonymousQueueName);
				//Create a binding - makes the exchange filter to the queue
				channel.QueueBind(anonymousQueueName, ExchangeName, EmptyRoutingKey);

				Console.WriteLine(" [*] Waiting for logs.");

				var consumer = new EventingBasicConsumer(channel);
				consumer.Received += (model, ea) =>
				{
					var body = ea.Body;
					var message = Encoding.UTF8.GetString(body);
					Console.WriteLine(" [x] {0}", message);
				};
				channel.BasicConsume(anonymousQueueName, true, consumer);

				Console.WriteLine(" Press [enter] to exit.");
				Console.ReadLine();
			}
		}
	}
}
