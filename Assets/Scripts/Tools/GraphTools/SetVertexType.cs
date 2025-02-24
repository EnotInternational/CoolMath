using System;
using UnityEngine;
using UnityEngine.Events;
[System.Serializable]
public class SetVertexType<T> : ITool where T : Vertex
{
    public bool block { get; set; }
    private Action<IVertexVisualizer<T>> _setAction;
    public SetVertexType(Action<IVertexVisualizer<T>> setAction)
    {
        _setAction = setAction;
    }
    public void Enable()
    {
        InteractionsManager.instance.OnObjectPointed.AddListener(ObjectPointedhandler);
    }
    private void ObjectPointedhandler(Transform other)
    {
        if(block)
            return;
        if(InteractionsManager.instance.IsOverUI())
            return;
        if(!other.TryGetComponent<IVertexVisualizer<T>>(out IVertexVisualizer<T> vertexVisualizer))
        {
            // Debug.Log("Failed to get vertex");
            return;
        }
        _setAction(vertexVisualizer);
    }
    public void Disable()
    {
        InteractionsManager.instance.OnObjectPointed.RemoveListener(ObjectPointedhandler);
    }
    public void Point()
    {
        
    }

    public void Initialize(ToolManager toolManager)
    {
        
    }
}