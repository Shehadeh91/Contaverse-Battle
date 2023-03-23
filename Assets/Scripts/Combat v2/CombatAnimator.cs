using UnityEngine;
using Sirenix.OdinInspector;

namespace Contaquest.Metaverse.Combat2
{
    public class CombatAnimator : MonoBehaviour
    {
        // [Header ("Runtime")]
        /*[ShowInInspector, ReadOnly] private bool isShowing;
        [ShowInInspector, ReadOnly] private bool isBlocked;*/

        [Header ("References")]
        [SerializeField] private Animator animator;
        [SerializeField] private string animatorMirrorParameterName;
        [SerializeField, ReadOnly] private int animatorMirrorParameterHash;
        /*private float shownTimer;
        private float blockTimer;*/
        
        private static readonly int GlobalActionSpeedMultiplier = Animator.StringToHash("GlobalActionSpeedMultiplier");

        protected void OnValidate()
        {
            if (animator == null)
                animator = GetComponentInChildren<Animator>();
            if (!string.IsNullOrEmpty(animatorMirrorParameterName))
            {
                animatorMirrorParameterHash = Animator.StringToHash(animatorMirrorParameterName);
            }
        }
        public void Play(CombatActionContext context)
        {
            bool mirrorAnimation = context.CombatAction.mirrorAnimation ^ (context.SourceRobotPart.equippableData.equipSlot == EquipSlot.LeftArm);
            animator.SetBool(animatorMirrorParameterHash, mirrorAnimation);
            Play(context.CombatAction.animationStateHash, context.CombatAction.CalculatedSpeed, context.CombatAction.transitionDuration);
        }
        public void Play(string action)
        {
            animator.SetBool(animatorMirrorParameterHash, false);
            Play(Animator.StringToHash(action), 1, 0.25f);
        }

        private void Play(int stateHash, float speed, float transitionDuration)
        {
            animator.SetFloat(GlobalActionSpeedMultiplier, speed);
            animator.CrossFadeInFixedTime(stateHash, transitionDuration);
        }
    }
}