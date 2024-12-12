using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Collections.Concurrent;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System;
using UnityEngine;
using System.Collections.Generic;

public class QueueProducer : MonoBehaviour
{
    public RabbitMQConnection rabbitMQConnection { get; private set; }
    private string queueName;
    private IChannel channel;

    public async Task DeclareQueue(string queueName)
    {
        rabbitMQConnection = new RabbitMQConnection();
        await rabbitMQConnection.Connect();
        channel = rabbitMQConnection.GetChannel();
        this.queueName = queueName;
        await channel.QueueDeclareAsync(queue: queueName, durable: false, exclusive: false, autoDelete: false,
            arguments: null);
    }

    public async Task SendMessage(byte [] message)
    {
        await channel.BasicPublishAsync(exchange: string.Empty, routingKey: queueName, body: message);
        Debug.Log($"Message sent: {message}");
    }
}
