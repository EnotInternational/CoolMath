using System.Linq;
using EditorAttributes;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class MultipleVideoPlayer : MonoBehaviour
{
    public RawImage rawImage;
    public VideoPlayer videoPlayer;
    [SerializeField, DataTable]
    private NamedVideo[] namedVideo;
    
    private void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        // videoPlayer.url = System.IO.Path.Combine (Application.streamingAssetsPath, filename);
        // videoPlayer.Prepare();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void PlayVideoByName(string name)
    {
        string filename = "";
        foreach (var video in namedVideo)
        {
            if(video.name == name)
            {
                filename = video.url;
            }
        }
        if(filename == "")
        {
            Debug.LogWarning($"Video with name {name} not found in the video player list", this);
            return;
        }
        string url = System.IO.Path.Combine(Application.streamingAssetsPath, filename);
        videoPlayer.url = url;
        videoPlayer.Play();
    }
    public void PlayVideoByIndex(int index)
    {
        if(index < 0 || index > namedVideo.Length)
        {
            Debug.LogWarning($"Video player list doesn`t have index {index}");
            return;
        }

    
        string url = System.IO.Path.Combine(Application.streamingAssetsPath, namedVideo[index].url);
        videoPlayer.url = url;
        videoPlayer.Play();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    [System.Serializable]
    private struct NamedVideo
    {
        public string name;
        public string url;

    }
}
