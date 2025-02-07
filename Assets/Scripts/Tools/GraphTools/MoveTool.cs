using UnityEngine;

[System.Serializable]
public class MoveTool : GraphTool
{
    private Transform _toMove;
    [SerializeField]private LayerMask _layerMask;

    #region StateChanging
    public override void Enable()
    {
        manager.OnClickDown.AddListener(ClickDownHandler);
        manager.OnClickUp.AddListener(ClickUpHandler);
    }
    private void ClickDownHandler()
    {
        Collider2D hit = Physics2D.OverlapPoint(manager.mousePosition, _layerMask);
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
        manager.OnClickDown.RemoveListener(ClickDownHandler);
        manager.OnClickUp.RemoveListener(ClickUpHandler);
    }
    public override void Point()
    {
        if(_toMove)
        {
            _toMove.GetComponent<Vertex>().UpdateAllLinks();
            _toMove.position = manager.mousePosition;
        }
    }
    #endregion
}
