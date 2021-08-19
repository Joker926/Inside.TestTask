
using Confluent.Kafka;
using Inside.TestTask.Model;
using OpenTracing;
using Org.OpenAPITools.Api;
using System.Text.Json;

namespace Inside.TestTask.MC3.Services;
public class KafkaConsumerHostedService : IHostedService, IDisposable
{
    private readonly IConfiguration _config;
    private readonly ITracer _tracer;
    private IScope _scope;
    private readonly IMessageApi _messageApi;
    public KafkaConsumerHostedService(IConfiguration config, ITracer tracer, IMessageApi messageApi)
    {
        _config = config;
        _tracer = tracer;
        _scope = _tracer.BuildSpan("KafkaConsumer").StartActive(true);
        _messageApi = messageApi;
    }

    public void Dispose()
    {
        _scope.Dispose();
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        string topic = _config["KAFKA:topic_name"];
        var conf = new ConsumerConfig
        {
            GroupId = "st_consumer_group",
            BootstrapServers = _config["KAFKA:server"],
            AutoOffsetReset = AutoOffsetReset.Earliest
        };
        using var builder = new ConsumerBuilder<string, string>(conf).Build();

        builder.Subscribe(topic);
        var cancelToken = new CancellationTokenSource();
        try
        {
            while (true)
            {
                var consumer = builder.Consume(cancelToken.Token);
                var objResult = JsonSerializer.Deserialize<TestTask.Model.Message>(consumer.Message.Value);
                Console.WriteLine($"Message: {consumer.Message.Value} received from {consumer.TopicPartitionOffset}");
                objResult.MC3_timestamp = DateTime.Now;
                try
                {
                    await _messageApi.ApiMessagePutAsync(new Org.OpenAPITools.Model.Message
                    {
                        Id = objResult.Id,
                        EndTimestamp = objResult.End_timestamp,
                        MC1Timestamp = objResult.MC1_timestamp,
                        MC2Timestamp = objResult.MC2_timestamp,
                        MC3Timestamp = objResult.MC3_timestamp,
                        SessionId = objResult.Session_id
                    },
                    CancellationToken.None);
                }
                catch(Exception ex)
                {
                    _scope.Span.Log($"Что-то пошло не так. {ex.Message}") ;
                }
                
            }
        }
        catch (Exception)
        {
            builder.Close();
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
