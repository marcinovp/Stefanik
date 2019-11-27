using EasyAR;
using UnityEngine;
using UnityEngine.Video;

[RequireComponent(typeof(VideoPlayer))]
[RequireComponent(typeof(MeshRenderer))]
public class TargetVideoPlayer : MonoBehaviour
{
    public ImageTargetBaseBehaviour imageTargetBehaviour;
    public float startFromTime = 0f;

    public VideoPlayer VideoPlayer { get; private set; }
    private MeshRenderer meshRenderer;

    void Awake()
    {
        //Debug.Log("Awake");
        VideoPlayer = GetComponent<VideoPlayer>();
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.enabled = false;
    }
    
    private void Start()
    {
        //Debug.Log("Start");
        VideoPlayer.time = startFromTime;
        VideoPlayer.prepareCompleted += VideoPlayer_prepareCompleted;
        imageTargetBehaviour.TargetFound += ImageTargetBehaviour_TargetFound;
        imageTargetBehaviour.TargetLost += ImageTargetBehaviour_TargetLost;
    }

    private void VideoPlayer_prepareCompleted(VideoPlayer source)
    {
        Debug.Log("VideoPlayer_prepareCompleted");

        meshRenderer.enabled = true;
    }
    
    private void ImageTargetBehaviour_TargetLost(TargetAbstractBehaviour obj)
    {
        Debug.Log("Target lost");
        VideoPlayer?.Pause();
        meshRenderer.enabled = false;
    }

    private void ImageTargetBehaviour_TargetFound(TargetAbstractBehaviour obj)
    {
        Debug.Log("Target found");
        VideoPlayer.Play();

        if (VideoPlayer.isPrepared)
        {
            meshRenderer.enabled = true;
        }
    }
}
