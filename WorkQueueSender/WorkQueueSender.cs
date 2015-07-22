using System;
using System.Text;
using RabbitMQ.Client;

namespace WorkQueueSend
{
	public class WorkQueueSender
	{
		private const bool Persistence = true;
		private const string QueueName = "task_queue";
		private const string DefaultNamelessExchange = "";

		public static void Main()
		{
			var factory = new ConnectionFactory() { HostName = "localhost" };
			using (var connection = factory.CreateConnection())
			{
				using (var channel = connection.CreateModel())
				{
					channel.QueueDeclare(QueueName, Persistence, false, false, null);
					var basicProperties = channel.CreateBasicProperties();
					basicProperties.SetPersistent(Persistence);

					while (true)
					{
						Console.WriteLine("Enter some dots or x ctrl-c to exit");
						var message = ReadMessage();
						channel.BasicPublish(DefaultNamelessExchange, QueueName, basicProperties, message);
						Console.WriteLine(" [x] Sent {0}", message);
					}	
				}
			}
		}

		private static byte[] ReadMessage()
		{
			var arg = Console.ReadLine();
			var message = String.IsNullOrEmpty(arg) ? "Hello World!" : arg;
			return Encoding.UTF8.GetBytes(message);
		}
	}

}
