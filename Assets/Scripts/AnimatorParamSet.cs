using UnityEngine;

namespace EnliStandardAssets
{
    public class AnimatorParamSet : MonoBehaviour
    {
        public Animator animator;
        public string animatorParameterName = "IsChecked";

        public void SetAnimatorValue(bool value)
        {
            //Debug.Log(string.Format("Animujem: {0} na objekte {1}", value, gameObject.name));
            animator.SetBool(animatorParameterName, value);
        }

        public void CycleAnimatorValue()
        {
            animator.SetBool(animatorParameterName, !AnimatorParameterValue);
        }

        public bool AnimatorParameterValue { get { return animator.GetBool(animatorParameterName); } }
    }
}