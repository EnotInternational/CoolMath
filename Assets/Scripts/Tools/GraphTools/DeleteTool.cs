using UnityEngine;

[System.Serializable]
public class DeleteTool : GraphTool
{
    private bool _deleting = false;
    #region StateChanging
    public override void Enable()
    {
        InteractionsManager.instance.OnClickDown.AddListener(ClickDownHandler);
        InteractionsManager.instance.OnClickUp.AddListener(ClickUpHandler);
    }
    public void Delete()
    {
        Collider2D hit = Physics2D.OverlapPoint(InteractionsManager.instance.mousePosition);
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
        InteractionsManager.instance.OnClickDown.RemoveListener(ClickDownHandler);
        InteractionsManager.instance.OnClickUp.RemoveListener(ClickUpHandler);
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