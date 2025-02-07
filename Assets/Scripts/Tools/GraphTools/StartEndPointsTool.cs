using UnityEngine;
using UnityEngine.Events;
[System.Serializable]
public class StartEndPointTool : GraphTool
{
    [SerializeField]private LayerMask _layerMask;
    private PointType _pointType = PointType.Null;
    public UnityEvent OnEndPlacing = new UnityEvent();
    public void ChangeSettingPoint(PointType type)
    {
        _pointType = type;
    }
    public override void Enable()
    {
        manager.OnClickDown.AddListener(Click);
    }
    private void Click()
    {
        if(_pointType == PointType.Null)
        {
            Debug.Log("Point type null");
            return;
        }

        Collider2D hit = Physics2D.OverlapPoint(manager.mousePosition, _layerMask);
        if(!hit)
        {
            Debug.Log("No hit");
            return;
        }
        Vertex vertex = hit.transform.GetComponent<Vertex>();
        
        AlgorithmManager alogthmManager = manager.alogthmManager;
        if(_pointType == PointType.Goal)
        {
            Debug.Log("Setting goal");
            ReplaceVertex(alogthmManager.goalVertex, vertex);
            vertex.vertexVisualizer.SetGoal();
        }
        else if(_pointType == PointType.Start)
        {
            Debug.Log("Setting start");
            ReplaceVertex(alogthmManager.startVertex, vertex);
            vertex.vertexVisualizer.SetStart();
        }

        Debug.Log("End");
        _pointType = PointType.Null;
        OnEndPlacing.Invoke();
        manager.OnClickDown.RemoveListener(Click);

        void ReplaceVertex(Vertex targetVertex, Vertex newVertex)
        {
            if(targetVertex)
            {
                targetVertex.vertexVisualizer.SetUnseen();
            }
            targetVertex = newVertex;
        }

    }
    public override void Disable()
    {
        manager.OnClickDown.RemoveListener(Click);
        _pointType = PointType.Null;
    }
    public override void Point()
    {
        
    }
    public enum PointType{Null, Goal, Start};
}