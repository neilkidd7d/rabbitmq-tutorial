# rabbitmq-tutorial
c# rabbitMq tutorial from - https://www.rabbitmq.com/getstarted.html

This code was written during an innovation day. Call it internal training if you will.
It was not not test driven, just a voyage of discovery.

## Getting Started
* A local RabbitMq server must be running on the default ports
* Clone the repo and open the solution!
* Install the Rabbit client nuget package
  * VS -> Tools -> Library Package Manager -> Package Manager Console
  *    `PM> Install-Package RabbitMQ.Client`

* Build :smile:

## Solution Structure

The solution contains publisher / subscriber pairs of projects. I trust the naming is obvious.

Each project compiles to a command line application. Simply open a cmd window and execute as required.

## Main Findings (for our needs)
* In general - publishers should send messages to an exchange. They (should) have no idea of a queue.
* Clients create the queues. They tell RabbitMQ to create a queue, based on an exchange.
* In most cases we should be using topic exchanges.

## Worked Example
 We will use __1__ TopicExchangePublisher and __3__ TopicExchangeSubscribers.

__Top Tip__Take a look at your local RabbitMQ pages while this is running.

 The scenario:
 * Subscriber 1 listens for __sony__ and __warner__ messages
 * Subscriber 2 listens for __orchard__ messages
 * Subscriber 3 listens for __all__ messages

 * Open 4 cmd windows:
  * 1 at the TopicExchangePublisher.exe directory
  * 3 at the TopicExchangeSubscriber.exe directory

 __Start each of the Subscribers__
 * `TopicExchangeSubscriber\bin\Debug>TopicExchangeSubscriber.exe sony warner`
 * `TopicExchangeSubscriber\bin\Debug>TopicExchangeSubscriber.exe orchard`
 * `TopicExchangeSubscriber\bin\Debug>TopicExchangeSubscriber.exe #`

 __Publish Messages__
 * `TopicExchangePublisher\bin\Debug>TopicExchangePublisher.exe sony "a sony message"`
 * `TopicExchangePublisher\bin\Debug>TopicExchangePublisher.exe orchard "an orchard message"`
 * `TopicExchangePublisher\bin\Debug>TopicExchangePublisher.exe warner "a warner message"`
