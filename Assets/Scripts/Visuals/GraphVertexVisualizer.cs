using System.Collections.Generic;
using UnityEngine;

public class GraphVertexVisualizer : MonoBehaviour, IVertexVisualizer<GraphVertex>
{
    [SerializeField]private VertexColorSettings _settings;
    [SerializeField]private GraphVertex _vertex;
    public GraphVertex vertex
    {
        get => _vertex;
        set
        {
            _vertex = value;
            _vertex.OnProcessStateChanged.AddListener(ProcessStateChangedHandler);
            _vertex.OnGraphStateChanged.AddListener(GraphStateChangedHandler);
            UpdatePosition();
        }
    }
    [SerializeField]private SpriteRenderer _renderer;
    private bool _processable = true;
    private void ProcessStateChangedHandler(Vertex.ProcessState processState)
    {
        if(!_processable)
            return;
        _renderer.color = _settings.GetProcessColor(processState);
    }
    public void UpdatePosition()
    {
        _vertex.position = transform.position;
    }
    private void GraphStateChangedHandler(GraphVertex.GraphState graphState)
    {
        if(graphState != GraphVertex.GraphState.Common)
        {
            _processable = false;
        }
        else
        {
            _processable = true;
        }
        _renderer.color = _settings.GetGraphColor(graphState);
    }
    private void OnDestroy()
    {
        vertex.UnlinkAll();
    }
}
