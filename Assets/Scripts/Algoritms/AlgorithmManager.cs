using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class AlgorithmManager : MonoBehaviour
{
    [SerializeField]private SearchAlgorithm[] algorithms;
    [SerializeField]private SearchAlgorithm currentAlgorithm;

    public Vertex startVertex;
    public Vertex goalVertex;
    public float iterationsPerSecond
    {
        get => _iterationsPerSecond;
    }
    [SerializeField]private float _iterationsPerSecond = 0;
    private List<Vertex> processedInStepVertexes;
    public bool inProcess
    {
        get => _inProcess; 
        private set
        {
            _inProcess = value;
            OnProcessChanged.Invoke(_inProcess);
        }
    }
    private bool _inProcess;
    public UnityEvent<bool> OnProcessChanged = new UnityEvent<bool>();
    private void Start()
    {
        algorithms = new SearchAlgorithm[]
        {
            new BFS(this),
            new Dijkstra(this)
        };
    }
    [EditorAttributes.Button]
    public void StartBFS()
    {
        ChangeAlgorithm<BFS>();
        StartSearch();
    }
    [EditorAttributes.Button]
    public void StartDijkstra()
    {
        ChangeAlgorithm<Dijkstra>();
        StartSearch();
    }

    public void StartSearch()
    {
        if(!CanStartSearch()) return;
        

        inProcess = true;
        currentAlgorithm.StartSearch(goalVertex, startVertex);

        currentAlgorithm.OnComplete.AddListener(AlgorithmCompleteHandler);
        currentAlgorithm.OnFail.AddListener(AlgorithmFailHandler);
        currentAlgorithm.OnStepComplete.AddListener(AlgorithmStepHandler);
    }
    private bool CanStartSearch()
    {
        if(inProcess)
        {
            Debug.LogWarning($"Can`t start search because other algorithm is in process");
            return false;
        }

        if(startVertex == null || goalVertex == null)
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
        if(!inProcess)
            return;
            
        if(currentAlgorithm == null) return;

        currentAlgorithm.MakeOneIteration();
    }
    [EditorAttributes.Button]
    public void ClearResults()
    {
        if(inProcess)
        {
            inProcess = false;
            currentAlgorithm.Stop();
        }
        if(currentAlgorithm != null)
        {
            currentAlgorithm.Clear();
        }
    }
    public void ChangeAlgorithm<T>() where T : SearchAlgorithm
    {
        if(currentAlgorithm != null)
        {
            currentAlgorithm.Clear();
        }
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
        inProcess = false;
        currentAlgorithm.OnComplete.RemoveListener(AlgorithmCompleteHandler);
        currentAlgorithm.OnFail.RemoveListener(AlgorithmFailHandler);
        // Debug.Log("Fail!");
    }
    private void AlgorithmCompleteHandler(List<Vertex> path)
    {
        inProcess = false;
        currentAlgorithm.OnComplete.RemoveListener(AlgorithmCompleteHandler);
        currentAlgorithm.OnFail.RemoveListener(AlgorithmFailHandler);

        foreach(var vertex in path)
        {
            vertex.processState = Vertex.ProcessState.Path;
        }
        // Debug.Log("Complete!");
    }
    private void AlgorithmStepHandler()
    {
        
    }
    
}
