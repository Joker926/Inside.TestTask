using Inside.TestTask.MC1.Services;
using Inside.TestTask.Model;
using Microsoft.AspNetCore.Mvc;
using OpenTracing;

namespace Inside.TestTask.MC1.Controllers;
[Route("api/[controller]")]
[ApiController]
public class MessageController : ControllerBase
{
    private readonly ITracer _tracer;
    private readonly IDataLayer _dataLayer;
    public MessageController(ITracer tracer, IDataLayer dataLayer)
    {
        _tracer = tracer;
        _dataLayer = dataLayer;
    }
    [HttpPut]
    public async Task<ActionResult>  AddMessage(Message msg)
    {
        using var scope = _tracer.BuildSpan("Return Message").StartActive(true);
        scope.Span.Log("Сообщение вернулось");
        try
        {
            msg.End_timestamp = DateTime.Now;
            await _dataLayer.UpdateMessage(msg);
        }
        catch(Exception ex)
        {
            scope.Span.Log($"Что-то пошло не так. {ex.Message}");
        }
        return Ok();
    }
}
