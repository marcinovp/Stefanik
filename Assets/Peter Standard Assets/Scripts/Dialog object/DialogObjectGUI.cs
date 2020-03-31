using UnityEngine;

namespace EnliStandardAssets.Dialogs
{
    public class DialogObjectGUI : MonoBehaviour
    {
        [SerializeField] private Animator animator;

        private string animatorParameterName = "IsChecked";
        
        public void Open()
        {
            animator.SetBool(animatorParameterName, true);
        }

        public void Close()
        {
            animator.SetBool(animatorParameterName, false);
        }

        public bool IsShowing { get { return animator.GetBool(animatorParameterName); } }
    }
}