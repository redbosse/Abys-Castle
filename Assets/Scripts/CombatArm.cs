using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatArm : MonoBehaviour
{
    [SerializeField]
    private SwordConstraint constraint;

    private Transform constraintTransform;

    public SwordConstraint Constraint { get => constraint; set => constraint = value; }

    public void SetConstraint(SwordConstraint inputConstraint)
    {
        constraint = inputConstraint;
        constraintTransform = inputConstraint.transform;
    }

    private void Update()
    {
        if (constraint is not null)
        {
            constraintTransform.position = transform.position;
            constraintTransform.rotation = transform.rotation;
        }
    }
}