using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Collections.Concurrent;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System;
using UnityEngine;
using System.Collections.Generic;

public class RpcClient : IAsyncDisposable
{
    private string QUEUE_NAME = "";
    private readonly IConnectionFactory _connectionFactory;
    private readonly ConcurrentDictionary<string, TaskCompletionSource<string>> _callbackMapper
        = new();
    private IConnection? _connection;
    private IChannel? _channel;
    private string? _replyQueueName;
    private string _tag = "";

    public RpcClient(string queueName)
    {
        _connectionFactory = new ConnectionFactory { HostName = "localhost" };
        this.QUEUE_NAME = queueName;
    }
    public RpcClient(string queueName, string tag)
    {
        _connectionFactory = new ConnectionFactory { HostName = "localhost" };
        this.QUEUE_NAME = queueName;
        this._tag = tag;
    }

    public async Task StartAsync()
    {
        Debug.Log("Creating connection...");
        _connection = await _connectionFactory.CreateConnectionAsync();
        Debug.Log("Connection created.");

        _channel = await _connection.CreateChannelAsync();
    
        QueueDeclareOk queueDeclareResult = await _channel.QueueDeclareAsync();
        _replyQueueName = queueDeclareResult.QueueName;
        Debug.Log($"Reply queue declared: {_replyQueueName}");
        var consumer = new AsyncEventingBasicConsumer(_channel);

        consumer.ReceivedAsync += (model, ea) =>
        {
            string? correlationId = ea.BasicProperties.CorrelationId;

            if (false == string.IsNullOrEmpty(correlationId))
            {
                if (_callbackMapper.TryRemove(correlationId, out var tcs))
                {
                    var body = ea.Body.ToArray();
                    var response = Encoding.UTF8.GetString(body);
                    tcs.TrySetResult(response);
                }
            }

            return Task.CompletedTask;
        };

        await _channel.BasicConsumeAsync(_replyQueueName, true, consumer);
    }

    public async Task<string> CallAsync(string message)
    {
        CancellationToken cancellationToken = default;
        if (_channel is null)
        {
            throw new InvalidOperationException();
        }

        string correlationId = Guid.NewGuid().ToString();
        var props = new BasicProperties
        {
            CorrelationId = correlationId,
            ReplyTo = _replyQueueName
        };
        props.Headers = new Dictionary<string, object>{ { "tag", _tag } };
        var tcs = new TaskCompletionSource<string>(
                TaskCreationOptions.RunContinuationsAsynchronously);
        _callbackMapper.TryAdd(correlationId, tcs);

        var messageBytes = Encoding.UTF8.GetBytes(message);
        await _channel.BasicPublishAsync(exchange: string.Empty, routingKey: QUEUE_NAME,
            mandatory: true, basicProperties: props, body: messageBytes);

        using CancellationTokenRegistration ctr =
            cancellationToken.Register(() =>
            {
                _callbackMapper.TryRemove(correlationId, out _);
                tcs.SetCanceled();
            });

        return await tcs.Task;
    }

    public async ValueTask DisposeAsync()
    {
        if (_channel is not null)
        {
            await _channel.CloseAsync();
        }

        if (_connection is not null)
        {
            await _connection.CloseAsync();
        }
    }
}