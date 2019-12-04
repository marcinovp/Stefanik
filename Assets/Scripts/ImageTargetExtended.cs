using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageTargetExtended : ImageTargetController
{
    //public bool performScaleCorrection = true;
    public event Action<ImageTargetController> TargetFound;
    public event Action<ImageTargetController> TargetLost;

    private void Awake()
    {
        //float scaleRatio = transform.localScale.x / transform.localScale.y;
        //Debug.Log("Initial scale ratio " + scaleRatio + ", scale: " + transform.localScale);

        //if (performScaleCorrection)
        //{
        //    foreach (Transform child in transform)
        //    {
        //        Vector3 childScale = child.localScale;
        //        childScale.y = childScale.y / scaleRatio;
        //        child.localScale = childScale;
        //    }
        //}
    }

    public override void OnFound()
    {
        base.OnFound();

        Debug.Log(string.Format("Image target found: {0}", gameObject.name));
        TargetFound?.Invoke(this);
    }

    public override void OnLost()
    {
        base.OnLost();

        Debug.Log(string.Format("Image target lost: {0}", gameObject.name));
        TargetLost?.Invoke(this);
    }
}
