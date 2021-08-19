
using Inside.TestTask.MC1.Model;
using Inside.TestTask.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using OpenTracing;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Websocket.Client;

namespace Inside.TestTask.MC1.Services;
public class TimedHostedService : IDisposable, ITimerService
{
    private  Timer _timer;
    private readonly IConfiguration _config;
    private readonly ITracer _tracer;
    private readonly WebsocketClient _wsClient;
    private Boolean _isEnabled;
    private readonly IDataLayer _dataLayer;
    private int _sessionId;

    private DateTime _startTime;
    private DateTime _endDate = DateTime.MinValue;
    private int _hitCount = 0;
    
    public TimedHostedService(IConfiguration config, ITracer tracer, WebsocketClient wsClient, IDataLayer dataLayer)
    {
        _config = config;
        _tracer = tracer;
        _wsClient = wsClient;
        _dataLayer = dataLayer;
    }
    public void Dispose()
    {
        _timer.Dispose();
        _wsClient.Dispose();
    }

    public async Task StartAsync()
    {
        using var scope = _tracer.BuildSpan("TimeService:StartAsync").StartActive(true);
        var period = TimeSpan.FromSeconds(int.Parse(_config["TimerPeriodInSec"]));
        if (!_isEnabled)
        {
            _startTime = DateTime.Now;
            _hitCount = 0;
            _sessionId = await _dataLayer.StartNewSession();
            await _wsClient.Start();
            await Task.Run(() => _wsClient.Send("1"));
            _timer = new Timer(SendMessage, null, TimeSpan.Zero, period);
            _isEnabled = true;
            _endDate = DateTime.MinValue;
            scope.Span.Log("Таймер запущен");
            
        }
        else
            scope.Span.Log("Таймер не запущен");
    }

    private async void SendMessage(object state)
    {
        using var scope = _tracer.BuildSpan("SendMessage").StartActive(true);
        var message = await PrepareMessage();
        var json = JsonSerializer.Serialize<Message>(message);
        scope.Span.Log($"запущен клиент. Контент запроса {json}");
        await Task.Run(() => _wsClient.Send(json));
        scope.Span.Log("Передача состоялась успешно");
        _hitCount += 1;
    }

    private async Task<Message> PrepareMessage()
    {
        var message = new Message()
        {
            MC1_timestamp = DateTime.Now,
            Session_id = _sessionId
        };
        message = await _dataLayer.AddNewMessage(message);
        return message;
    }

    public async Task<Statistics> StopAsync()
    {
        using var scope = _tracer.BuildSpan("TimeService:Stop").StartActive(true);
        _isEnabled = false;
        _timer.Change(Timeout.Infinite, 0);
        if (_endDate == DateTime.MinValue)
            _endDate = DateTime.Now;
        var dateDiff = Utilities.Simulate.DateDiff(Utilities.Simulate.DateInterval.Minute, _startTime, _endDate);
        var stats = new Statistics { HitCount = _hitCount, TimeOfRunInMinutes = dateDiff };
        await _wsClient.Stop(System.Net.WebSockets.WebSocketCloseStatus.NormalClosure, "окончание передачи данных");
        Console.WriteLine($"Количество начатых циклов сообщений: {stats.HitCount}");
        Console.WriteLine($"Времени прошло (мин.): {stats.TimeOfRunInMinutes}");
        scope.Span.Log("Таймер остановлен");
        return stats;
    }


}
