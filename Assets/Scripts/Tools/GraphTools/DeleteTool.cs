using UnityEngine;

[System.Serializable]
public class DeleteTool : GraphTool
{
    private bool _deleting = false;
    #region StateChanging
    public override void Enable()
    {
        manager.OnClickDown.AddListener(ClickDownHandler);
        manager.OnClickUp.AddListener(ClickUpHandler);
    }
    public void Delete()
    {
        Collider2D hit = Physics2D.OverlapPoint(manager.mousePosition);
        if(hit == null)
        {
            return;
        }
        
        MonoBehaviour.Destroy(hit.gameObject);
    }
    // protected void ClickUpHandler()
    // {
        
    // }
    public override void Disable()
    {
        _deleting = false;
        manager.OnClickDown.RemoveListener(ClickDownHandler);
        manager.OnClickUp.RemoveListener(ClickUpHandler);
    }
    public override void Point()
    {   
        if(_deleting)
        {
            Delete();
        }
    }
    #endregion
    private void ClickDownHandler()
    {
        _deleting = true;
    }
    private void ClickUpHandler()
    {
        _deleting = false;
    }
}