using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public abstract class SearchAlgorithm
{
    public UnityEvent<List<Vertex>> OnComplete = new();
    public UnityEvent OnFail = new();
    public UnityEvent OnStepComplete = new();
    public bool InProcess{get => _inProcess;}
    private bool _inProcess;
    protected Vertex goalVertex;
    protected Vertex startVertex;
    public Coroutine searchCoroutine;
    protected AlgorithmManager _manager;
    public SearchAlgorithm(AlgorithmManager searchAlgorithmManager)
    {
        _manager = searchAlgorithmManager;
    }
    public void StartSearch(Vertex goalVertex, Vertex startVertex)
    {
        this.goalVertex = goalVertex;
        this.startVertex = startVertex;

        BeforeFirstIteration();
        searchCoroutine = _manager.StartCoroutine(SearchCoroutine());
    }   
    public void MakeOneIteration()
    {
        Iterate();
    }
    public void Complete(List<Vertex> path)
    {
        _inProcess = false;
        OnComplete.Invoke(path);
    }
    public void Fail()
    {
        _inProcess = false;
        OnFail.Invoke();
    }
    protected abstract void BeforeFirstIteration(); 
    protected abstract void Iterate();
    protected IEnumerator SearchCoroutine()
    {
        _inProcess = true;
        while(InProcess)
        {
            if(_manager.iterationsPerSecond == 0)
            {
                yield return new WaitWhile(()=>{return _manager.iterationsPerSecond == 0;});
            }
            yield return new WaitForSeconds(1/_manager.iterationsPerSecond);
            Iterate();
        }
    }
}