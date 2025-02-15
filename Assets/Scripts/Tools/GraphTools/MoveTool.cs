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
        Collider2D hit = Physics2D.OverlapPoint(InteractionsManager.instance.mousePosition, _layerMask);
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
            _toMove.position = InteractionsManager.instance.mousePosition;
            _toMove.GetComponent<IVertexVisualizer<GraphVertex>>().UpdatePosition();
        }
    }
    #endregion
}
