using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController), typeof(Animator))]
public class RM_CharacterController : MonoBehaviour
{
    [SerializeField]
    private float gravity = 1;

    private CharacterController controller = null;
    private Animator animator = null;

    private void Start()
    {
        if (controller is null)
        {
            controller = GetComponent<CharacterController>();
        }

        if (animator is null)
        {
            animator = GetComponent<Animator>();
        }
    }

    private void Update()
    {
        controller.Move(animator.deltaPosition);
        controller.transform.rotation *= animator.deltaRotation;
    }
}