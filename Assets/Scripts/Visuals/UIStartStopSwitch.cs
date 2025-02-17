using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIStartStopSwitch : MonoBehaviour
{
    public Button playButton;
    public Button stopButton;
    public AlgorithmManager algorithmManager;
    void Start()
    {
        playButton.onClick.AddListener(PlayButtonClickHandler);
        stopButton.onClick.AddListener(StopButtonClickHandler);
        algorithmManager.OnProcessChanged.AddListener(ProcessChangedHandler);
        stopButton.gameObject.SetActive(false);
        playButton.gameObject.SetActive(true);
    }
    private void PlayButtonClickHandler()
    {
        if(algorithmManager.StartSearch())
        {
            stopButton.gameObject.SetActive(true);
            playButton.gameObject.SetActive(false);
        }
    }
    private void StopButtonClickHandler()
    {
        if(algorithmManager.Pause())
        {
            stopButton.gameObject.SetActive(false);
            playButton.gameObject.SetActive(true);
        }
    }
    private void ProcessChangedHandler(bool inProcess)
    {
        if(!inProcess)
        {
            stopButton.gameObject.SetActive(false);
            playButton.gameObject.SetActive(true);
        }
        
    }
}
