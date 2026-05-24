using ConsoleApp1.Logger;
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

    public override void HandleKey(ConsoleKey keyInfo)
    {
        if (keyInfo == KeyConsts.LogView.key)
        {
            if(!_logRenderer.IsRenderingAllLogs)_logRenderer.RenderAll();
            else
            {
                _render.RenderAll();
                _logRenderer.IsRenderingAllLogs = false;
            }
        }
        else
        { 
            NextKeyNode.HandleKey(keyInfo);
        }
    }
}