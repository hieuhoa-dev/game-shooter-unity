using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMovementGrappling : MonoBehaviour
{
    // GLOBAL
    private CharacterController controller;
    private Vector3 currentVelocity;
    private Vector3 velocityToSet;
    private bool enableMovementOnNextTouch;

    [SerializeField]
    bool activeGrapple;
    
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }
    
    private void Update()
    {
        ApplyGrappleVelocity();
        CheckLanding();
    }

    
    private float SafeSqrt(float v)
    {
        return Mathf.Sqrt(Mathf.Max(v, 0.0001f));
    }

    public Vector3 CalculateJumpVelocity(Vector3 startPoint, Vector3 endPoint, float trajectoryHeight)
    {
        float gravity = -Physics.gravity.y;
        float displacementY = endPoint.y - startPoint.y;
        Vector3 displacementXZ = new Vector3(endPoint.x - startPoint.x, 0f, endPoint.z - startPoint.z);

        float timeUp = SafeSqrt(2 * trajectoryHeight / gravity);
        float timeDown = SafeSqrt(2 * (trajectoryHeight - displacementY) / gravity);
        float totalTime = timeUp + timeDown;

        Vector3 velocityXZ = displacementXZ / totalTime;
        Vector3 velocityY = Vector3.up * SafeSqrt(2 * gravity * trajectoryHeight);

        return velocityXZ + velocityY;
    }

    
    public void JumpToPosition(Vector3 targetPosition, float trajectoryHeight)
    {
        activeGrapple = true;
        enableMovementOnNextTouch = true;

        velocityToSet = CalculateJumpVelocity(transform.position, targetPosition, trajectoryHeight);
    }

    
    private void ApplyGrappleVelocity()
    {
        if (!activeGrapple) return;

        currentVelocity = velocityToSet;
        currentVelocity += Physics.gravity * Time.deltaTime;

        controller.Move(currentVelocity * Time.deltaTime);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (enableMovementOnNextTouch)
        {
            enableMovementOnNextTouch = false;
            ResetRestrictions();

            GetComponent<Grappling>().StopGrapple();
        }
    }
    
    private void CheckLanding()
    {
        if (activeGrapple && enableMovementOnNextTouch && controller.isGrounded)
        {
            enableMovementOnNextTouch = false;
            ResetRestrictions();
            GetComponent<Grappling>().StopGrapple();
        }
    }
    
    public void ResetRestrictions()
    {
        activeGrapple = false;
        currentVelocity = Vector3.zero;
    }
}