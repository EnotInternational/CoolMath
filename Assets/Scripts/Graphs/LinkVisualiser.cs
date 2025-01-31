using UnityEngine;

public class LinkVisualizer : MonoBehaviour
{
    public LineRenderer lineRenderer;
    [SerializeField]private Link _link;
    [SerializeField]private LinkColorSettings _linkColorSettings;
    public void SetLink(Link link)
    {
        _link = link;
        lineRenderer.material = _linkColorSettings.standartMaterial;
        _link.OnUnlink.AddListener(() => Destroy(gameObject));
        _link.OnChanged.AddListener(()=>SyncPosiitons());
        _link.OnSetAsPath.AddListener(SetPath);
        _link.OnSeen.AddListener(SetSeen);
    }
    private void SetPath()
    {
        lineRenderer.material = _linkColorSettings.pathMaterial;
    }
    private void SetSeen()
    {
        lineRenderer.material = _linkColorSettings.seenMaterial;
    }
    public void SyncPosiitons()
    {
        lineRenderer.SetPosition(0, _link.VertexA.transform.position);
        lineRenderer.SetPosition(1, _link.VertexB.transform.position);
    }
}
