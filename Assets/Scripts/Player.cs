using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator), typeof(CharacterController))]
public class Player : MonoBehaviour
{
    [SerializeField]
    private CameraOrbitRotation cameraOrbit;

    private Sword sword;
    private CombatArm arm;

    [SerializeField]
    private GrabZone grabZone;

    [SerializeField]
    private AnimationCurve animationWalk;

    [SerializeField]
    private float interpolateSpeed = 1f;

    [SerializeField]
    private float moveSpeed = 1f;

    [SerializeField]
    private float runSpeed = 1f;

    [SerializeField]
    private float jumpForce = 1f;

    [SerializeField]
    private float jumpDrag = 1f;

    private Animator animator;
    private CharacterController characterController;

    private Rigidbody rigit;

    private float jumpVelosity = 0;

    private Vector2 current = Vector2.zero;

    private UnityAction grabbingItem = delegate { };
    private UnityAction startGrabbingItem = delegate { };

    [SerializeField]
    private StoppingState state = StoppingState.none;

    private enum StoppingState
    {
        none,
        Grab,
        Attack
    }

    private void OnEnable()
    {
        grabbingItem += grabZone.Grab;
        startGrabbingItem += grabZone.StartGrab;

        grabZone.GrabSword += GrabSword;
    }

    private void OnDisable()
    {
        if (sword is not null)
            sword.StoppingAttack -= StopAttack;

        grabbingItem -= grabZone.Grab;
        startGrabbingItem -= grabZone.StartGrab;
        grabZone.GrabSword -= GrabSword;
    }

    private void GrabSword(Sword newSword)
    {
        if (arm is not null)
        {
            if (sword is not null)
                sword.StoppingAttack -= StopAttack;

            if (arm.Constraint is not null)
                Destroy(arm.Constraint.gameObject);
        }

        sword = newSword;

        sword.StoppingAttack += StopAttack;

        var constraint = sword.transform.parent.GetComponentInChildren<SwordConstraint>();

        arm.SetConstraint(constraint);
    }

    public void swordE()
    {
        if (sword is not null)
        {
            sword.StartAttack();
            sword.IsActivateSword = true;
        }
        cameraOrbit.IsGrabState = true;

        if (characterController.isGrounded)
            state = StoppingState.Attack;
    }

    public void swordD()
    {
        if (sword is not null)
        {
            sword.IsActivateSword = false;
        }

        cameraOrbit.IsGrabState = false;

        state = StoppingState.none;
    }

    private void Start()
    {
        animator ??= GetComponent<Animator>();
        characterController ??= GetComponent<CharacterController>();
        cameraOrbit ??= GetComponent<CameraOrbitRotation>();
        rigit ??= GetComponent<Rigidbody>();
        arm ??= GetComponentInChildren<CombatArm>();

        if (animator.runtimeAnimatorController is null)
        {
            throw new System.Exception("Animator Controller is null");
        }

        if (cameraOrbit is null)
        {
            throw new System.Exception("cameraOrbit is null");
        }

        if (rigit is null)
        {
            throw new System.Exception("Rigitbody is null");
        }

        if (arm is null)
        {
            throw new System.Exception("CombatArm is null");
        }
    }

    public void EndAttack()
    {
        state = StoppingState.none;
        cameraOrbit.IsGrabState = false;
    }

    public void sGrabbing()
    {
        startGrabbingItem.Invoke();
    }

    public void Grabbing()
    {
        state = StoppingState.none;
        cameraOrbit.IsGrabState = false;

        grabbingItem.Invoke();
    }

    private void AnimatorUpdate(Vector2 point, bool isRun)
    {
        if (point.y > 0)
            point.y *= isRun ? 2 : 1;

        Vector2 direction = (point - current);

        float delta = (interpolateSpeed * Time.deltaTime);

        current += direction * delta;

        animator.SetFloat("x", animationWalk.Evaluate(Mathf.Abs(current.x)) * Mathf.Sign(current.x));
        animator.SetFloat("y", animationWalk.Evaluate(Mathf.Abs(current.y)) * Mathf.Sign(current.y));
    }

    private void Grab(bool isGrab)
    {
        if (isGrab)
        {
            state = StoppingState.Grab;

            cameraOrbit.IsGrabState = state != StoppingState.none;
            animator.SetTrigger("grab");
        }
    }

    public void StopAttack()
    {
        animator.SetTrigger("stop");
        EndAttack();
        swordD();
    }

    private void Attack(bool isAttack)
    {
        if (isAttack)
        {
            animator.SetTrigger("attack1");

            if (characterController.isGrounded)
                state = StoppingState.Attack;
        }
    }

    private void Jump(bool isJump)
    {
        if (isJump && characterController.isGrounded)
        {
            jumpVelosity = jumpForce;
        }
    }

    private void Walk(Vector2 direction, bool isRun)
    {
        float mult = moveSpeed;

        if (direction.y > 0 && isRun)
        {
            mult = runSpeed;
        }

        direction = direction.normalized;

        Vector3 dir = direction.y * transform.forward;
        Vector3 dir2 = direction.x * transform.right;

        dir = dir + dir2;

        characterController.Move((dir.normalized * Time.deltaTime * mult));

        jumpVelosity -= jumpVelosity * jumpDrag * Time.deltaTime;

        characterController.Move(Vector3.up * jumpVelosity * Time.deltaTime);
        characterController.Move(Vector3.down * 9.8f * Time.deltaTime);
    }

    public void TakeDamage(float damage)
    {
    }

    private void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        Grab(Input.GetKeyDown(KeyCode.E) && grabZone.GrabbingCount() != 0);
        Attack(Input.GetKeyDown(KeyCode.Mouse0));
        Jump(Input.GetKeyDown(KeyCode.Space));

        Vector2 point = new Vector2(x, y);

        if (state != StoppingState.none)
        {
            point = Vector2.zero;
        }

        animator.SetBool("isJump", !characterController.isGrounded);

        Walk(point, Input.GetKey(KeyCode.LeftShift));

        AnimatorUpdate(point, Input.GetKey(KeyCode.LeftShift));
    }
}