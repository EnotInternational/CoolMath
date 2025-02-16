using System.Collections;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class PlayVideo1 : MonoBehaviour
{
    public RawImage rawImage;
    public VideoPlayer videoPlayer;
    void OnEnable(){
        StartCoroutine(PlayVideos());
    }
    IEnumerator PlayVideos(){
        videoPlayer.Prepare();
        WaitForSeconds waitForSeconds = new WaitForSeconds(0);
        while (!videoPlayer.isPrepared)
        {
            yield return waitForSeconds;
            
        }
        rawImage.texture = videoPlayer.texture;
        videoPlayer.Play();
        
    }

}
