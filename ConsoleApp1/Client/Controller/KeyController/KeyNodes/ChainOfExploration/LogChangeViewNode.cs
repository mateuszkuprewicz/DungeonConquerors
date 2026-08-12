using ConsoleApp1.View;

namespace ConsoleApp1.ChainOfKeyOperations;

public class LogChangeViewNode : AbstractKeyNode
{
    private LogRenderer _logRenderer;
    private Render _render;
    
    public LogChangeViewNode(LogRenderer logRenderer, Render render)
    {
        _logRenderer = logRenderer;
        _render = render;
    }

    public override Task HandleKey(ConsoleKey keyInfo)
    {
        if (keyInfo == KeyConsts.LogView.key)
        {
            if(!_logRenderer.IsRenderingAllLogs)
            {
                Render.IsRenderingFullScreenMode = true;
                
                _logRenderer.RenderAll();
            }
            else
            {
                Render.IsRenderingFullScreenMode = false;
                
                _render.RenderAll();
                _logRenderer.RenderLast();
                _logRenderer.IsRenderingAllLogs = false;
            }
            return Task.CompletedTask;
        }
        else
        { 
            return NextKeyNode?.HandleKey(keyInfo) ?? Task.CompletedTask;
        }
    }
}