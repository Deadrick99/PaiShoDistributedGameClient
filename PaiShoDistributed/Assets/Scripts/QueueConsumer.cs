using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Collections.Concurrent;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System;
using UnityEngine;
using System.Collections.Generic;
public class QueueConsumer : MonoBehaviour
{
    AsyncEventingBasicConsumer consumer { get; set; }
    public RabbitMQConnection rabbitMQConnection { get; set; }
    private IChannel channel;
    private TaskCompletionSource<string> taskCompletion;
    private string queueName;
    public async Task DeclareQueue(string queueName)
    {
        rabbitMQConnection = new RabbitMQConnection();
        await rabbitMQConnection.Connect();
        channel = rabbitMQConnection.GetChannel();
        this.queueName = queueName;
        await channel.QueueDeclareAsync(queue: queueName, durable: false, exclusive: false, autoDelete: false,
            arguments: null);
    }

    public async Task InitConsumer()
    {  
        consumer = new AsyncEventingBasicConsumer(channel);

        consumer.ReceivedAsync += async (model, ea) =>
        {
            var body = ea.Body.ToArray(); 
            var message = Encoding.UTF8.GetString(body);

            if (taskCompletion != null && !taskCompletion.Task.IsCompleted)
            {
                taskCompletion.SetResult(message);
            }

            Console.WriteLine($"Message Consumed: {message}");

            // Optionally acknowledge the message if autoAck is false
            // channel.BasicAck(ea.DeliveryTag, multiple: false);

            await Task.CompletedTask;
        };

        // Start consuming messages
        await channel.BasicConsumeAsync(queue: queueName, autoAck: true, consumer: consumer);
    }
    public async Task<string> ConsumeMessageAsync()
    {
        taskCompletion = new TaskCompletionSource<string>();
        return await taskCompletion.Task;
    }
}