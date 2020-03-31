using EnliStandardAssets;
using EnliStandardAssets.Dialogs;
using EnliStandardAssets.SimpleMessaging;
using System.Collections;
using UnityEngine;

public class DialogObjectExample : MonoBehaviour
{
    [SerializeField] private DialogObject dialogObject;
    [SerializeField] private MonoBehaviour SimpleMessager;

    private ISimpleMessager simpleMessager;

    private void Start()
    {
        SimpleMessager?.GetInterface(out simpleMessager);
    }

    public void ShowDialog1()
    {
        DialogObject.Builder builder = new DialogObject.Builder(dialogObject);
        builder.SetMessage("Dialogova sprava c. 1")
            .SetTitle("Dialog 1")
            .SetPositiveButton("Hej", (a) => ShowResult("Positive"))
            .SetNeutralButton("Neutralny", (a) => ShowResult("Neutral"))
            .Show();
    }

    public void ShowDialog2()
    {
        DialogObject.Builder builder = new DialogObject.Builder(dialogObject);
        builder.SetMessage("Dialogova sprava c. 2. Vyskusajme si to.")
            .SetTitle("Dialog 2")
            .SetPositiveButton("Hej", (a) => ShowResult("Positive"))
            .SetNeutralButton("Neutralny", (a) => ShowResult("Neutral"))
            .SetNegativeButton("Neg", (a) => ShowResult("Negative"))
            .SetCancelButton("", (a) => ShowResult("Cancel bez buttonu"))
            .Show();
    }

    public void ShowObe()
    {
        StartCoroutine(DelayedAction());
    }

    public void ShowResult(string result)
    {
        Debug.Log(result);
        simpleMessager?.ShowMessage(result, ToastDuration.Short);
    }

    private IEnumerator DelayedAction()
    {
        ShowDialog1();
        yield return new WaitForSeconds(1);
        ShowDialog2();
    }
}
