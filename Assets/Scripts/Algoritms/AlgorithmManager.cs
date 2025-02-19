using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class AlgorithmManager : MonoBehaviour
{
    [SerializeField]private SearchAlgorithm[] algorithms;
    [SerializeField]private SearchAlgorithm currentAlgorithm;

    public Vertex startVertex;
    public Vertex goalVertex;
    public float IterationsPerSecond
    {
        get => _iterationsPerSecond;
        private set => _iterationsPerSecond = value;
    }
    [SerializeField]private float _iterationsPerSecond = 0;
    [SerializeField]private bool _paused = false;
    [SerializeField]private Slider slider;
    [SerializeField]private TMP_InputField inputField;
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
        ChangeAlgorithm<BFS>();
    }
    private void OnEnable()
    {
        slider.onValueChanged.AddListener(SetSimulationSpeedBySlider);
        inputField.onValueChanged.AddListener(SetSimulationSpeedByInputField);
        InteractionsManager.instance.OnObjectPointed.AddListener(ObjectPointedHandler);
    }
    private void OnDisable()
    {
        slider.onValueChanged.RemoveListener(SetSimulationSpeedBySlider);
        inputField.onValueChanged.RemoveListener(SetSimulationSpeedByInputField);
        InteractionsManager.instance.OnObjectPointed.RemoveListener(ObjectPointedHandler);
    }
    private void ObjectPointedHandler(Transform clicked)
    {
        if(_inProcess)
            return;
        if(currentAlgorithm != null)
        {
            currentAlgorithm.Clear();
        }
    }

    public bool Pause()
    {
        _paused = true;
        if(!_inProcess)
        {
            return false;
        }
        currentAlgorithm.Paused = true;
        return true;
    }
    public bool Unpause()
    {
        _paused = false;
        currentAlgorithm.Paused = false;
        return true;
    }
    public void SetSimulationSpeedBySlider(float value)
    {
        _iterationsPerSecond = value;
        inputField.SetTextWithoutNotify(Mathf.RoundToInt(value).ToString());
    }
    public void SetSimulationSpeedByInputField(string text)
    {
        if(!float.TryParse(text, out float number))
        {
            string result = new string(text.Where(t => char.IsDigit(t)).ToArray());
            
            if(result == string.Empty)
                return;
            number = float.Parse(result);
            Debug.Log("Wrond text, new is: " + result);
            inputField.SetTextWithoutNotify(result);
        }
        Debug.Log("Sussessful " + number);
        if(number > 100)
        {
            number = 100;
            inputField.SetTextWithoutNotify(number.ToString());
        }
        slider.SetValueWithoutNotify(number);
        _iterationsPerSecond = number;
    }
    public void SetBFS()
    {
        ChangeAlgorithm<BFS>();
    }
    public void SetDijkstra()
    {
        ChangeAlgorithm<Dijkstra>();
    }
    public void SetGreedy()
    {
        // ChangeAlgorithm<Dijkstra>();
    }
    public void SetAStar()
    {
        // ChangeAlgorithm<Dijkstra>();
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

    public bool StartSearch()
    {
        if(_paused)
        {
            Unpause();
            return true;
        }

        if(!CanStartSearch()) return false;
        
        ClearResults();

        inProcess = true;
        currentAlgorithm.StartSearch(goalVertex, startVertex);

        currentAlgorithm.OnComplete.AddListener(AlgorithmCompleteHandler);
        currentAlgorithm.OnFail.AddListener(AlgorithmFailHandler);
        currentAlgorithm.OnStepComplete.AddListener(AlgorithmStepHandler);
        return true;
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
        _paused = false;
        inProcess = false;
        currentAlgorithm.OnComplete.RemoveListener(AlgorithmCompleteHandler);
        currentAlgorithm.OnFail.RemoveListener(AlgorithmFailHandler);
        // Debug.Log("Fail!");
    }
    private void AlgorithmCompleteHandler(List<Vertex> path)
    {
        _paused = false;
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
