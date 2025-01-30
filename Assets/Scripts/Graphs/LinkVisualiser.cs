using UnityEngine;

public class LinkVisualizer : MonoBehaviour
{
    public LineRenderer lineRenderer;
    [SerializeField]private Link _link;
    public void SetLink(Link link)
    {
        _link = link;
        _link.OnUnlink.AddListener(() => Destroy(gameObject));
        _link.OnChanged.AddListener(()=>SyncPosiitons());
    }
    public void SyncPosiitons()
    {
        lineRenderer.SetPosition(0, _link.VertexA.transform.position);
        lineRenderer.SetPosition(1, _link.VertexB.transform.position);
    }
}
