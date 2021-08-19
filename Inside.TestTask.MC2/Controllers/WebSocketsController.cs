using Inside.TestTask.MC2.Services;
using Inside.TestTask.Model;
using Microsoft.AspNetCore.Mvc;
using OpenTracing;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace Inside.TestTask.MC2.Controllers;
[Route("api/[controller]")]
[ApiController]
public class WebSocketsController : ControllerBase
{
    private readonly ITracer _tracer;
    private readonly KafkaProducer _producer;
    public WebSocketsController(ITracer tracer, KafkaProducer producer)
    {
        _tracer = tracer;
        _producer = producer;
    }

    [HttpGet("ws")]
    public async Task GetWebSocket()
    {
        if (HttpContext.WebSockets.IsWebSocketRequest)
        {
            using var scope = _tracer.BuildSpan("GetWebSocket").StartActive(true);
            using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
            scope.Span.Log("Соединение установлено");
            await Echo(webSocket);
        }
        else
        {
            HttpContext.Response.StatusCode = 400;
        }
    }

    private async Task Echo(WebSocket webSocket)
    {
        using var scope = _tracer.BuildSpan("Echo").StartActive(true);
        var buffer = new byte[1024 * 4];
        var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
        
        while (!result.CloseStatus.HasValue)
        {
            var serverMsg = Encoding.UTF8.GetBytes($"Server: Hello. You said: {Encoding.UTF8.GetString(buffer)}");
            await webSocket.SendAsync(new ArraySegment<byte>(serverMsg, 0, serverMsg.Length), result.MessageType, result.EndOfMessage, CancellationToken.None);
            scope.Span.Log("Сообщение отправлено");
            result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            var strResult = Encoding.UTF8.GetString(buffer, 0, result.Count);
            scope.Span.Log($"Получено сообщение: {strResult}");
            try
            {
                var objResult = JsonSerializer.Deserialize<Message>(strResult);
                objResult.MC2_timestamp = DateTime.Now;
                await _producer.SendToKafka(objResult);
            }
            catch (JsonException)
            {
                scope.Span.Log($"Сообщение не JSON и пропускается");
            }
            
        }
        await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        scope.Span.Log("соединение закрыто");
    }
}
