﻿using Common;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectRouting_Subscriber1
{
    class Program
    {
        private static ConnectionFactory _factory;
        private static IConnection _connection;

        private const string ExchangeName = "DirectRouting_Exchange";
        private const string CardPaymentQueueName = "CardPaymentDirectRouting_Queue";

        static void Main(string[] args)
        {
            _factory = new ConnectionFactory { HostName = "localhost", UserName = "guest", Password = "guest" };
            using (_connection = _factory.CreateConnection())
            {
                using (var channel = _connection.CreateModel())
                {
                    channel.ExchangeDeclare(ExchangeName, "direct");
                    channel.QueueDeclare(CardPaymentQueueName, true, false, false, null);
                    channel.QueueBind(CardPaymentQueueName, ExchangeName, "CardPayment");
                    channel.BasicQos(0, 1, false);

                    var consumer = new QueueingBasicConsumer(channel);
                    channel.BasicConsume(CardPaymentQueueName, false, consumer);

                    while(true)
                    {
                        var ea = consumer.Queue.Dequeue();
                        var message = (Payment)ea.Body.DeSerialize(typeof(Payment));
                        var routingKey = ea.RoutingKey;
                        channel.BasicAck(ea.DeliveryTag, false);
                        Console.WriteLine($"---- Payment - Routing Key <{routingKey}> : {message.CardNumber} : ${message.AmountToPay} ");
                    }
                }
            }
        }
    }
}
