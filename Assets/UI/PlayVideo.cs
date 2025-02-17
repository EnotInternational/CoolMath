using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class PlayVideo1 : MonoBehaviour
{
    public RawImage rawImage;
    public VideoPlayer videoPlayer;
    private void Awake(){
        videoPlayer.Prepare();
    }
    private void OnDisable()
    {
        rawImage.gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        rawImage.gameObject.SetActive(true);
    }
    public void PlayVideo()
    {
        rawImage.texture = videoPlayer.texture;
        videoPlayer.Play();
    }
}
