using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    private bool _deleting = false;
    public void OnClickDown() 
    { 
        MonoBehaviour.Destroy(GameObject.Find("Vertex2D(Clone)"));
    }

}
