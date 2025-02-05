using System.Collections.Generic;
using UnityEngine;

public class VertexVisualizer : IVertexVisualizer
{
    private VertexColorSettings _settings;
    private SpriteRenderer _renderer;
    private bool _processable = true;
    public VertexVisualizer(VertexColorSettings settings, SpriteRenderer renderer)
    {
        _settings = settings;
        _renderer = renderer;
    }
    public void SetGoal()
    {
        _processable = false;
        _renderer.color = _settings.goalColor;
    }
    public void SetStart()
    {
        _processable = false;
        _renderer.color = _settings.startColor;
    }
    public void SetCommon()
    {
        _processable = true;
        _renderer.color = _settings.unseenColor;
    }

    public void SetProcessed()
    {
        if(!_processable)
            return;

        _renderer.color = _settings.processedColor;
    }
    public void SetPath()
    {
        if(!_processable)
            return;

        _renderer.color = _settings.pathColor;
    }
    public void SetSeen()
    {
        if(!_processable)
            return;

        _renderer.color = _settings.seenColor;
    }
    public void SetUnseen()
    {
        if(!_processable)
            return;

        _renderer.color = _settings.unseenColor;
    }
}
