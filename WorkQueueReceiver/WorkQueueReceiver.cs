using System;
using System.Text;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace WorkQueueReceive
{
	public class WorkQueueReceiver
	{
		private const string QueueName = "task_queue";

		public static void Main()
		{
			var factory = new ConnectionFactory() { HostName = "localhost" };
			using (var connection = factory.CreateConnection())
			{
				using (var channel = connection.CreateModel())
				{
					channel.QueueDeclare(QueueName, true, false, false, null);

					//http://www.rabbitmq.com/tutorials/tutorial-two-dotnet.html
					//don't dispatch a new message to a worker until it has processed and acknowledged the previous one.
					//Instead, it will dispatch it to the next worker that is not still busy.
					channel.BasicQos(0, 1, false);

					var consumer = new QueueingBasicConsumer(channel);
					channel.BasicConsume(QueueName, false, consumer);

					Console.WriteLine(" [*] Waiting for messages." + "To exit press CTRL+C");
					while (true)
					{
						//Dequeue blocks until a message is available.
						var ea = (BasicDeliverEventArgs)consumer.Queue.Dequeue();

						var messageBody = Encoding.UTF8.GetString(ea.Body);
						var dots = messageBody.Split('.').Length - 1;
						Console.WriteLine(" [x] Received:{0}", messageBody);
						Thread.Sleep(dots * 1000);

						//Tell rabbitMQ this message is processed
						channel.BasicAck(ea.DeliveryTag, false);
					}
				}
			}
		}
	}
}
