using UnityEngine;

[System.Serializable]
public class MoveTool : GraphTool
{
    private Transform _toMove;
    [SerializeField]private LayerMask _layerMask;

    #region StateChanging
    public override void Enable()
    {
        InteractionsManager.instance.OnClickDown.AddListener(ClickDownHandler);
        InteractionsManager.instance.OnClickUp.AddListener(ClickUpHandler);
    }
    private void ClickDownHandler()
    {
        if(!manager.AlgorithmManager.clean)
        {
            manager.AlgorithmManager.ClearResults();
        }
        
        Collider2D hit = Physics2D.OverlapPoint(InteractionsManager.instance.marginedMousePosition, _layerMask);
        if(!hit)
            return;
        _toMove = hit.transform;
    }
    private void ClickUpHandler()
    {
        _toMove = null;
    }
    public override void Disable()
    {
        InteractionsManager.instance.OnClickDown.RemoveListener(ClickDownHandler);
        InteractionsManager.instance.OnClickUp.RemoveListener(ClickUpHandler);
    }
    public override void Point()
    {
        if(_toMove)
        {
            _toMove.position = InteractionsManager.instance.marginedMousePosition;
            _toMove.GetComponent<IVertexVisualizer<GraphVertex>>().UpdatePosition();
            if(manager.AutoWeights)
            {
                foreach(var link in _toMove.GetComponent<GraphVertexVisualizer>().vertex.Links)
                {
                    link.weight = Mathf.RoundToInt(Vector2.Distance(link.VertexA.position, link.VertexB.position));
                }
            }
        }
    }
    #endregion
}
