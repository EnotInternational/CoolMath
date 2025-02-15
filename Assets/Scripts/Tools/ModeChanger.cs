using System;
using EditorAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
public class ModeChanger : MonoBehaviour
{
    public Mode graphMode;
    public Mode gridMode;
    [SerializeField]
    private AlgorithmManager _algorithmManager;
    private Mode currentMode;
    private void Awake()
    {
        currentMode = graphMode;
    }
    public void ChangeMode(Mode mode)
    {   
        if(currentMode!=null)
        {
            currentMode.Disable(_algorithmManager);
        }
        currentMode = mode;
        currentMode.Enable(_algorithmManager);
    }
    [Button]
    public void SetGraphs()
    {
        ChangeMode(graphMode);
    }
    [Button]
    public void SetGrids()
    {
        ChangeMode(gridMode);
    }
    [Serializable]
    public class Mode
    {
        public ToolManager toolManager;
        public GameObject[] gameObjects = new GameObject[]{};
        public Vertex startVertex;
        public Vertex goalVertex;
        public void Enable(AlgorithmManager algorithmManager)
        {
            algorithmManager.startVertex = startVertex;
            algorithmManager.goalVertex = goalVertex;
            toolManager.enabled = true;
            foreach (var gameObject in gameObjects)
            {
                gameObject.SetActive(true);
            }
        }
        public void Disable(AlgorithmManager algorithmManager)
        {
            startVertex = algorithmManager.startVertex;
            goalVertex = algorithmManager.goalVertex;
            
            toolManager.enabled = false;
            foreach (var gameObject in gameObjects)
            {
                gameObject.SetActive(false);
            }
        }
    }
}