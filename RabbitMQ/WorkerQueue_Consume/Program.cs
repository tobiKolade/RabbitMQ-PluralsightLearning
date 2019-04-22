﻿using Common;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;

namespace WorkerQueue_Consume
{
    class Program
    {
        private static ConnectionFactory _factory;
        private static IConnection _connection;

        private const string QueueName = "WorkerQueue_Queue";

        static void Main(string[] args)
        {
            Receive();

            Console.ReadLine();
        }

        public static void Receive()
        {
            _factory = new ConnectionFactory { HostName = "localhost", UserName = "guest", Password = "guest" };
            using(_connection = _factory.CreateConnection())
            {
                using (var channel = _connection.CreateModel())
                {
                    channel.QueueDeclare(QueueName, true, false, false, null);
                    channel.BasicQos(0, 1, false);

                    var consumer = new QueueingBasicConsumer(channel);
                    channel.BasicConsume(QueueName, false, consumer);

                    while (true)
                    {
                        var ea = consumer.Queue.Dequeue();
                        var message = (Payment)ea.Body.DeSerialize(typeof(Payment));
                        channel.BasicAck(ea.DeliveryTag, false);

                        Console.WriteLine($"----- Payment Processed {message.CardNumber} : {message.AmountToPay}");
                    }
                }
            }
        }
    }
}
