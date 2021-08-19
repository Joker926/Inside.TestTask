using Inside.TestTask.MC1.Model;
using Inside.TestTask.MC1.Services;
using Microsoft.AspNetCore.Mvc;
using OpenTracing;
using Websocket.Client;

namespace Inside.TestTask.MC1.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ManagementController : ControllerBase
{
    private readonly ITimerService _timedService;
    private readonly ITracer _tracer;
    public ManagementController(ITimerService timedService, ITracer tracer)
    {
        _tracer = tracer;
        _timedService = timedService;
    }
    

    [HttpGet("start")]
    public async Task<ActionResult> Start()
    {
        using var scope = _tracer.BuildSpan("Start").StartActive(true);
        scope.Span.Log($"Попытка запустить таймер");
        try
        {
            await _timedService.StartAsync();
            scope.Span.Log("Таймер запущен");
            return Ok();
        }
        catch(Exception ex)
        {
            scope.Span.Log($"Запуск провален: {ex.Message}");
            return StatusCode(500, ex.Message);
        }
        
    }

    [HttpGet("stop")]
    public async  Task<ActionResult<Statistics>> StopAsync()
    {
        using var scope = _tracer.BuildSpan("Stop").StartActive(true);
        scope.Span.Log($"Попытка остановить таймер");
        var stats = await _timedService.StopAsync();
        scope.Span.Log($"Таймер остановлен");
        return Ok(stats);

    }
}
