﻿using Common;
using RabbitMQ.Client;
using System;

namespace WorkerQueue_Producer
{
    class Program
    {
        private static ConnectionFactory _factory;
        private static IConnection _connection;
        private static IModel _model;

        private const string QueueName = "WorkerQueue_Queue";
        static void Main(string[] args)
        {
            var payment1 = new Payment { AmountToPay = 25.0m, CardNumber = "1234123412341234", Name = "Mr S Haunts" };
            var payment2 = new Payment { AmountToPay = 5.0m, CardNumber = "1234123412341234", Name = "Mr S Haunts" };
            var payment3 = new Payment { AmountToPay = 2.0m, CardNumber = "1234123412341234", Name = "Mr S Haunts" };
            var payment4 = new Payment { AmountToPay = 17.0m, CardNumber = "1234123412341234", Name = "Mr S Haunts" };
            var payment5 = new Payment { AmountToPay = 300.0m, CardNumber = "1234123412341234", Name = "Mr S Haunts" };
            var payment6 = new Payment { AmountToPay = 350.0m, CardNumber = "1234123412341234", Name = "Mr S Haunts" };
            var payment7 = new Payment { AmountToPay = 295.0m, CardNumber = "1234123412341234", Name = "Mr S Haunts" };
            var payment8 = new Payment { AmountToPay = 5625.0m, CardNumber = "1234123412341234", Name = "Mr S Haunts" };
            var payment9 = new Payment { AmountToPay = 2.0m, CardNumber = "1234123412341234", Name = "Mr S Haunts" };
            var payment10 = new Payment { AmountToPay = 12.0m, CardNumber = "1234123412341234", Name = "Mr S Haunts" };

            CreateConnection();

            SendMessage(payment1);
            SendMessage(payment2);
            SendMessage(payment3);
            SendMessage(payment4);
            SendMessage(payment5);
            SendMessage(payment6);
            SendMessage(payment7);
            SendMessage(payment8);
            SendMessage(payment9);
            SendMessage(payment10);

            Console.ReadLine();
        }

        private static void CreateConnection()
        {
            _factory = new ConnectionFactory { HostName = "localhost", UserName = "guest", Password = "guest" };
            _connection = _factory.CreateConnection();
            _model = _connection.CreateModel();

            _model.QueueDeclare(QueueName, true, false, false, null);
        }

        private static void SendMessage(Payment message)
        {
            _model.BasicPublish("", QueueName, null, message.Serialize());
            Console.WriteLine($"Payment sent {message.CardNumber}, ${message.AmountToPay}");
        }
    }
}
