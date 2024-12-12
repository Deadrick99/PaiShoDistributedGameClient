using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Collections.Concurrent;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System;
using UnityEngine;
using System.Collections.Generic;

public class RabbitMQConnection : MonoBehaviour
{
    private ConnectionFactory factory;
    private IConnection connection;
    private IChannel channel;

    public async Task Connect()
    {
        try
        {
            Debug.Log("Initializing RabbitMQ connection.  ..");
            factory = new ConnectionFactory
            {
                HostName = "localhost",
                Port = 5672
            };
            this.connection = await factory.CreateConnectionAsync();
            Debug.Log("factory established.");
            this.channel = await connection.CreateChannelAsync();
            Debug.Log("channel established.");
        }
        catch (Exception ex)
        {
            Debug.LogError($"RabbitMQ initialization failed: {ex.Message}");
        }
    }
    
    public void Disconnect()
    {
        channel?.CloseAsync();
        connection?.CloseAsync();
    }

    public IChannel GetChannel()
    {
        return channel;
    }

}
