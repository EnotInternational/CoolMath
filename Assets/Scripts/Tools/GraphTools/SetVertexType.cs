using System;
using UnityEngine;
using UnityEngine.Events;
[System.Serializable]
public class SetVertexType<T> : ITool where T : Vertex
{
    private Action<IVertexVisualizer<T>> _setAction;
    public SetVertexType(Action<IVertexVisualizer<T>> setAction)
    {
        _setAction = setAction;
    }
    public void Enable()
    {
        InteractionsManager.instance.OnObjectPointed.AddListener(ObjectClickedhandler);
    }
    private void ObjectClickedhandler(Transform other)
    {
        if(!other.TryGetComponent<IVertexVisualizer<T>>(out IVertexVisualizer<T> vertexVisualizer))
        {
            Debug.Log("Failed to get vertex");
            return;
        }
        _setAction(vertexVisualizer);
    }
    public void Disable()
    {
        InteractionsManager.instance.OnObjectPointed.RemoveListener(ObjectClickedhandler);
    }
    public void Point()
    {
        
    }

    public void Initialize(IToolManager toolManager)
    {
        
    }
}