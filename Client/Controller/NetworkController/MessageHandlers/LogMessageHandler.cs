using System.Text.Json;
using ConsoleApp1.Logger;
using ConsoleApp1.Shared.DTO.ServerAnswers.GameChangedBroadcast;
using ConsoleApp1.View;

namespace ConsoleApp1.NetworkController;

public class LogMessageHandler : IMessageHandler
{
    private string? _text;
    private LogRenderer _logRenderer;

    public LogMessageHandler(string serialisedObject, LogRenderer logRenderer)
    {
        if (serialisedObject == null) throw new Exception("serialisedObject is null");
        _text = JsonSerializer.Deserialize<LogMessege>(serialisedObject)?.Text;
        _logRenderer = logRenderer;
    }

    public void Handle()
    {
        var logger = EventLog.GetEventLog();
        if (_text == null)
        {
            logger.Log("Received empty log");
        }
        else
        {
            logger.Log(_text);
            if (!_logRenderer.IsRenderingAllLogs)
            {
                _logRenderer.RenderLast();
            }
            else
            {
                _logRenderer.RenderAll();
            }
        }
    }
}