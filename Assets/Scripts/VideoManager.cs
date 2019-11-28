﻿using EasyAR;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoManager : MonoBehaviour
{
    public List<ImageTargetBaseBehaviour> imageTargetBehaviour;
    public List<TargetVideoPlayer> targetVideoPlayers;

    void Start()
    {
        for (int i = 0; i < imageTargetBehaviour.Count; i++)
        {
            int index = i;
            imageTargetBehaviour[i].TargetFound += (x) => Target_TargetFound(imageTargetBehaviour[index]);
            //videos[i].imageTargetBehaviour.TargetLost += (x) => Target_TargetLost(index);
        }
    }

    private void Target_TargetFound(ImageTargetBaseBehaviour imageTargetFound)
    {
        foreach (var targetVideoPlayer in targetVideoPlayers)
        {
            if (targetVideoPlayer.imageTargetBehaviour != imageTargetFound)
            {
                targetVideoPlayer.VideoPlayer.Stop();
            }
        }
    }

    private void Target_TargetLost(int index)
    {
        
    }

    private IEnumerator DelayAction(float delaySeconds, Action action)
    {
        yield return new WaitForSeconds(delaySeconds);
        action();
    }
}