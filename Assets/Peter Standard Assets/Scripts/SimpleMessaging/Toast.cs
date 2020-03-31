using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace EnliStandardAssets.SimpleMessaging
{
	public class Toast : MonoBehaviour, ISimpleMessager
    {
        [SerializeField] private Animator toastAnimation;
        [SerializeField] private Text toastText;

        private Coroutine lifecycleCoroutine;

#if UNITY_ANDROID
        AndroidJavaObject currentActivity;
        AndroidJavaObject androidPlugin;
#endif

        void Start ()
        {
            //go to end of current (default) state
            toastAnimation.Play(toastAnimation.GetCurrentAnimatorStateInfo(0).shortNameHash, 0, 1f);

#if UNITY_ANDROID
            if (Application.isMobilePlatform)
            {
                using (var javaUnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                {
                    currentActivity = javaUnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                    androidPlugin = new AndroidJavaObject("com.enli.unity_plugins.ToastPlugin", currentActivity);
                }
            }
#endif
        }

        public void ShowMessage(string message, ToastDuration duration)
        {
            float durationTime = 2f;
            switch (duration)
            {
                case ToastDuration.Short:
                    durationTime = 2f;
                    break;
                case ToastDuration.Long:
                    durationTime = 3.5f;
                    break;
            }

            ShowMessage(message, durationTime);
        }

        public void ShowMessage(string message, float duration)
        {
#if !UNITY_ANDROID || UNITY_EDITOR
            toastText.text = message;
            gameObject.SetActive(true);

            if (lifecycleCoroutine != null)
                StopCoroutine(lifecycleCoroutine);
            lifecycleCoroutine = StartCoroutine(ToastLifecycle(duration));
#else
            androidPlugin.Call("ShowToast", message, duration > 2.5f); //bool je ci to ma byt longToast
#endif
        }

        private IEnumerator ToastLifecycle(float duration)
        {
            float endTime = Time.time + duration;
            toastAnimation.SetBool(toastAnimation.parameters[0].name, true);

            while (Time.time < endTime)
                yield return null;
            
            toastAnimation.SetBool(toastAnimation.parameters[0].name, false);
            lifecycleCoroutine = null;
        }
    }
}