using UnityEngine;

[System.Serializable]
public class MoveTool : GraphTool
{
    private Transform _toMove;

    #region StateChanging
    public override void Enable()
    {
        Collider2D hit = Physics2D.OverlapPoint(manager.mousePosition);
        if(!hit)
            return;
    
        _toMove = hit.transform;
    }
    public override void Disable()
    {
        _toMove = null;
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
