using UnityEngine;

[System.Serializable]
public class DeleteTool : GraphTool
{
    #region StateChanging
    public override void Enable()
    {
        Collider2D hit = Physics2D.OverlapPoint(manager.mousePosition);
        if(hit == null)
            return;
        MonoBehaviour.Destroy(hit.gameObject);
    }
    public override void Disable()
    {
        
    }
    public override void Point()
    {
        
    }
    #endregion
}