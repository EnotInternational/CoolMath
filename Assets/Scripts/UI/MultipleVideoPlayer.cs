using System.Linq;
using EditorAttributes;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class MultipleVideoPlayer : MonoBehaviour
{
    public RawImage rawImage;
    public VideoPlayer videoPlayer;
    [SerializeField]
    private string[] videosUrls;
    
    private void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.prepareCompleted += PrepareCompleteHandler;
        rawImage.color = new Color(0, 0, 0, 0);

        // videoPlayer.targetTexture = rawImage.;
        // videoPlayer.url = System.IO.Path.Combine (Application.streamingAssetsPath, filename);
        // videoPlayer.Prepare();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void PlayVideoByName(string name)
    {
        string url = System.IO.Path.Combine(Application.streamingAssetsPath, name);
        if(videoPlayer.url == url)
            return;
        videoPlayer.url = url;
        videoPlayer.Prepare();
    }
    public void PlayVideoByIndex(int index)
    {
        if(index < 0 || index > videosUrls.Length)
        {
            Debug.LogWarning($"Video player list doesn`t have index {index}");
            return;
        }

    
        string url = System.IO.Path.Combine(Application.streamingAssetsPath, videosUrls[index]);
        if(videoPlayer.url == url)
            return;
        videoPlayer.url = url;
        videoPlayer.Prepare();
    }

    private void PrepareCompleteHandler(VideoPlayer source)
    {
        rawImage.texture = videoPlayer.texture;
        rawImage.color = Color.white;
        videoPlayer.Play();
    }
    private void OnDisable()
    {
        rawImage.color = new Color(0, 0, 0, 0);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
