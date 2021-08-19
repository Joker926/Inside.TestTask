
using Confluent.Kafka;
using OpenTracing;
using System.Text.Json;

namespace Inside.TestTask.MC2.Services;
public class KafkaProducer
{
    private readonly ProducerConfig _producerConfig;
    private readonly ITracer _tracer;
    private readonly IConfiguration _config;

    public KafkaProducer(IConfiguration config, ITracer tracer)
    {
        _config = config;
        _tracer = tracer;
        _producerConfig = new ProducerConfig
        { BootstrapServers = _config["KAFKA:server"] };
    }

    public async Task SendToKafka(Model.Message message)
    {
        using var scope = _tracer.BuildSpan("SendToKafka").StartActive(true);
        var topic = _config["KAFKA:topic_name"];
        var msgStr = JsonSerializer.Serialize(message);
        scope.Span.Log(msgStr);
        using var producer = new ProducerBuilder<string, string>(_producerConfig).Build();
        try
        {
            await producer.ProduceAsync(topic, new Message<string, string>
            {
                Key = Guid.NewGuid().ToString(),
                Value = msgStr
            });
        }
        catch (Exception e)
        {
            var msg = $"Что-то пошло не так: {e}";
            scope.Span.Log(msg);
            Console.WriteLine(msg);
        }

    }
}
