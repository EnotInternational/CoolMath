using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CellVertexVisualizer : MonoBehaviour, IVertexVisualizer<CellVertex>
{
    [SerializeField]private VertexColorSettings _settings;
    [SerializeField]private TextMeshPro text;
    [SerializeField]private int maxWeight;
    [SerializeField]private CellVertex _vertex;
    public CellVertex vertex
    {
        get => _vertex;
        set
        {
            _vertex = value;
            Debug.Log(_vertex);
            _vertex.OnProcessStateChanged.AddListener(ProcessStateChangedHandler);
            _vertex.OnCellStateChanged.AddListener(CellStateChangedHandler);
            UpdatePosition();
        }
    }
    [SerializeField]private SpriteRenderer _renderer;
    private bool _processable = true;
    private void ProcessStateChangedHandler(Vertex.ProcessState processState)
    {
        if(!_processable)
        {
            return;
        }
        if(processState == Vertex.ProcessState.NotSeen)
        {
            if(_vertex.cellState == CellVertex.CellState.Weighted)
            {
                _renderer.color = _settings.GetColorFromWeightGradient(_vertex.weight / maxWeight);
            }
            else
            {
                _renderer.color = _settings.GetProcessColor(processState);
            }
        }
        else
        {
            _renderer.color = _settings.GetProcessColor(processState);
        }
    }
    private void CellStateChangedHandler(CellVertex.CellState cellState)
    {
        if(cellState == CellVertex.CellState.Common || cellState == CellVertex.CellState.Weighted)
        {
            _processable = true;
        }
        else
        {
            _processable = false;
        }
        if(cellState == CellVertex.CellState.Weighted)
        {
            _renderer.color = _settings.GetColorFromWeightGradient(_vertex.weight / maxWeight);
            text.text = "" + _vertex.weight;
            return;
        }
        text.text = "";
        _renderer.color = _settings.GetCellColor(cellState);
    }
    
    public void UpdatePosition()
    {
        _vertex.position = transform.position;
    }
    private void OnDestroy()
    {
        vertex.UnlinkAll();
    }
}
