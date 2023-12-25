using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationMatchTarget : MonoBehaviour
{
    [SerializeField]
    private float offset;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private AvatarTarget targ;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (animator.isMatchingTarget)
                return;

            animator.SetTrigger("jump");
            animator.MatchTarget(transform.position + transform.forward * offset, transform.rotation, targ,
                                                      new MatchTargetWeightMask(Vector3.one, 0.5f), 0.288f, 0.968f);
        }
    }
}