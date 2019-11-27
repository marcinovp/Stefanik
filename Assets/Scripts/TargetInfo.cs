using EnliStandardAssets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetInfo : MonoBehaviour
{
    public Text infoTitleText;
    public Text infoMessageText;
    public AnimatorParamSet panelAnimation;
    public EasyTween infoButtonAnimation;
    public List<InfoPair> infoPairs;

    void Start()
    {
        for (int i = 0; i < infoPairs.Count; i++)
        {
            int index = i;
            infoPairs[i].target.TargetFound += (x) => Target_TargetFound(index);
            infoPairs[i].target.TargetLost += (x) => Target_TargetLost(index);
        }

        infoButtonAnimation.gameObject.SetActive(false);
    }

    public void ShowHint(bool value)
    {
        panelAnimation.SetAnimatorValue(value);
        infoButtonAnimation.OpenCloseObjectAnimation(!value);
    }

    private void Target_TargetFound(int index)
    {
        SetInfo(index);
    }

    private void Target_TargetLost(int index)
    {
    }

    private void SetInfo(int index)
    {
        infoTitleText.text = infoPairs[index].infoTitle;
        infoMessageText.text = infoPairs[index].infoMessage;
        infoButtonAnimation.gameObject.SetActive(true);
    }

    private IEnumerator DelayAction(float delaySeconds, Action action)
    {
        yield return new WaitForSeconds(delaySeconds);
        action();
    }


    [Serializable]
    public class InfoPair
    {
        public EasyAR.ImageTargetBaseBehaviour target;
        public string infoTitle;
        public string infoMessage;
    }
}
