using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class AlgorithmManager : MonoBehaviour
{
    [SerializeField]private SearchAlgorithm[] algorithms;
    [SerializeField]private SearchAlgorithm currentAlgorithm;
    public Vertex startVertex;
    public Vertex goalVertex;
    public int iterationsPerSecond
    {
        get => _iterationsPerSecond;
    }
    [SerializeField]private int _iterationsPerSecond = 0;
    private List<Vertex> processedInStepVertexes;
    public bool inProcess{get =>_inProcess;}
    private bool _inProcess = false;
    private void Start()
    {
        algorithms = new SearchAlgorithm[]
        {
            new BFS(this)
        };
    }
    [EditorAttributes.Button]
    public void StartBFS()
    {
        ChangeAlgorithm<BFS>();
        StartSearch();
    }
    public void StartSearch()
    {
        if(!CanStartSearch()) return;
        
        startVertex.vertexVisualizer.SetStart();
        goalVertex.vertexVisualizer.SetGoal();

        _inProcess = true;
        currentAlgorithm.StartSearch(goalVertex, startVertex);

        currentAlgorithm.OnComplete.AddListener(AlgorithmCompleteHandler);
        currentAlgorithm.OnFail.AddListener(AlgorithmFailHandler);
        currentAlgorithm.OnStepComplete.AddListener(AlgorithmStepHandler);
    }
    private bool CanStartSearch()
    {
        if(_inProcess)
        {
            Debug.LogWarning($"Can`t start search because other algorithm is in process");
            return false;
        }

        if(!startVertex || !goalVertex)
        {
            Debug.LogWarning($"Can`t start search because start or goal vertexes are not assigned");
            return false;
        }
        if(currentAlgorithm == null)
        {
            Debug.LogWarning($"Can`t start search because current algirithm is null");
            return false;
        }
        return true;

    }
    [EditorAttributes.Button]
    public void MakeOneIteration()
    {
        if(currentAlgorithm == null) return;

        currentAlgorithm.MakeOneIteration();
    }
    public void ChangeAlgorithm<T>() where T : SearchAlgorithm
    {
        foreach (var algorithm in algorithms)
        {
            if(algorithm.GetType() == typeof(T))
            {
                currentAlgorithm = algorithm;
                return;
            }
        }
        Debug.LogWarning($"Algorithm type of {typeof(T)} not found");
    }
    private void AlgorithmFailHandler()
    {
        currentAlgorithm.OnComplete.RemoveListener(AlgorithmCompleteHandler);
        currentAlgorithm.OnFail.RemoveListener(AlgorithmFailHandler);
        Debug.Log("Fail!");
    }
    private void AlgorithmCompleteHandler(List<Vertex> path)
    {
        currentAlgorithm.OnComplete.RemoveListener(AlgorithmCompleteHandler);
        currentAlgorithm.OnFail.RemoveListener(AlgorithmFailHandler);

        foreach(var vertex in path)
        {
            vertex.vertexVisualizer.SetPath();
        }
        Debug.Log("Complete!");
    }
    private void AlgorithmStepHandler()
    {
        
    }
    
}
