using UnityEngine;


public class PlayerAnimatorStateRelay : StateMachineBehaviour
{
    [SerializeField] private PlayerLocomotionStateId stateId;
    [SerializeField] private float completeNormalizedTime = 1.0f;
    [SerializeField] private bool enableCompleteEvent = true;
    private bool hasSentComplete = false;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateId == PlayerLocomotionStateId.Null)
        {
            return;
        }

        if (animator.TryGetComponent<PlayerAnimationBridge>(out var aniBridge))
        {
            aniBridge.OnAnimationEnterEvent(stateId);
        }

        hasSentComplete = false;
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateId == PlayerLocomotionStateId.Null)
        {
            return;
        }

        if (animator.TryGetComponent<PlayerAnimationBridge>(out var aniBridge))
        {
            aniBridge.OnAnimationExitEvent(stateId);
        }

        hasSentComplete = false;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (enableCompleteEvent && !hasSentComplete && stateInfo.normalizedTime >= completeNormalizedTime)
        {
            if (animator.TryGetComponent<PlayerAnimationBridge>(out var aniBridge))
            {
                aniBridge.OnAnimationCompleteEvent(stateId);
            }
            hasSentComplete = true;
        }
    }
}