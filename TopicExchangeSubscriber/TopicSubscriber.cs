using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace TopicExchangeSubscriber
{
	class TopicSubscriber
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
					var queueName = channel.QueueDeclare();

					if (args.Length < 1)
					{
						Console.Error.WriteLine("Usage: {0} [binding_key...]", Environment.GetCommandLineArgs()[0]);
						Environment.ExitCode = 1;
						return;
					}

					//1 .. n bindingkeys here. We could listen to multiple keys (sony,warner,theorchard etc)
					foreach (var bindingKey in args)
					{
						channel.QueueBind(queueName, ExchangeName, bindingKey);
					}

					Console.WriteLine(" [*] Waiting for messages. " + "To exit press CTRL+C");

					var consumer = new QueueingBasicConsumer(channel);
					channel.BasicConsume(queueName, true, consumer);

					while (true)
					{
						var ea = (BasicDeliverEventArgs)consumer.Queue.Dequeue();
						var body = ea.Body;
						var message = Encoding.UTF8.GetString(body);
						var routingKey = ea.RoutingKey;
						Console.WriteLine(" [x] Received '{0}':'{1}'", routingKey, message);
					}
				}
			}
		}
	}
}
