using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public Player player;
    public Transform rightArm;
    public Transform rightArmPivot;
    public Transform leftArm;
    public Transform leftArmPivot;

    bool armMovingForward = true;
    bool armFlappingUp = true;

    public bool forceWalkingAnimation;
    public bool forceFlappingAnimation;

    // Update is called once per frame
    void Update()
    {
        // Vertical movement without gravity
        if (forceFlappingAnimation || player.isActiveAndEnabled && player.IsFlappingEnabled())
        {
            FlappingArmMovement(-0.3f, -0.9f);
        }
        else
        {
            if (leftArm.localRotation.z > -0.1f || leftArm.localRotation.z < -0.2f)
            {
                FlappingArmMovement(-0.1f, -0.2f);  // Reset arms to original position
            }
        }

        // Walking animation movement
        if (forceWalkingAnimation || player.isActiveAndEnabled && player.IsWalkingEnabled())
        {
            WalkingArmMovement(0.25f);
        }
        else
        {
            if (leftArm.localRotation.x > 0.01f || leftArm.localRotation.x < -0.01f)
            {
                WalkingArmMovement(0.01f); // Reset arms to original position
            }
        }
    }

    void WalkingArmMovement(float maxAngle)
    {
        if (armMovingForward)
        {
            leftArm.RotateAround(leftArmPivot.position, leftArm.TransformDirection(Vector3.right), 200 * Time.deltaTime);
            rightArm.RotateAround(rightArmPivot.position, rightArm.TransformDirection(Vector3.left), 200 * Time.deltaTime);
        }
        else
        {
            leftArm.RotateAround(leftArmPivot.position, leftArm.TransformDirection(Vector3.left), 200 * Time.deltaTime);
            rightArm.RotateAround(rightArmPivot.position, rightArm.TransformDirection(Vector3.right), 200 * Time.deltaTime);
        }

        if (leftArm.localRotation.x > maxAngle)
        {
            armMovingForward = false;
        }
        else if (leftArm.localRotation.x < -maxAngle)
        {
            armMovingForward = true;
        }
    }

    void FlappingArmMovement(float maxAngle, float minAngle)
    {
        if (armFlappingUp)
        {
            leftArm.RotateAround(leftArmPivot.position, leftArm.TransformDirection(Vector3.forward), 400 * Time.deltaTime);
            rightArm.RotateAround(rightArmPivot.position, rightArm.TransformDirection(Vector3.back), 400 * Time.deltaTime);
        }
        else
        {
            leftArm.RotateAround(leftArmPivot.position, leftArm.TransformDirection(Vector3.back), 400 * Time.deltaTime);
            rightArm.RotateAround(rightArmPivot.position, rightArm.TransformDirection(Vector3.forward), 400 * Time.deltaTime);
        }

        if (leftArm.localRotation.z > maxAngle)
        {
            armFlappingUp = false;
        }
        else if (leftArm.localRotation.z < minAngle)
        {
            armFlappingUp = true;
        }
    }

}
