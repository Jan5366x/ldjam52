using System;
using UnityEngine;

public class AnimationHelper
{
    
    public const String ANIM_IDLE = "Idle";
    public const String ANIM_WALK = "Walk";
    public const String ANIM_JUMP = "Jump";
    public const String ANIM_FALLING = "Falling";
    public const String ANIM_LANDING = "Landing";
    
    public static bool HasParameter(Animator animator, String parameter)
    {
        if (!animator)
        {
            return false;
        }

        foreach (var animatorControllerParameter in animator.parameters)
        {
            if (animatorControllerParameter.name.Equals(parameter))
            {
                return true;
            }
        }

        return false;
    }

    public static void SetParameter(Animator animator, String parameter, bool value)
    {
        if (!animator)
        {
            return;
        }

        if (!HasParameter(animator, parameter)) return;

        Debug.Log(parameter + value);

        animator.SetBool(parameter, value);
    }

    public static void SetParameter(Animator animator, String parameter, float value)
    {
        if (!animator)
        {
            return;
        }

        if (!HasParameter(animator, parameter)) return;

        animator.SetFloat(parameter, value);
    }

    public static void SetParameter(Animator animator, String parameter, int value, bool forceReset = true)
    {
        if (!animator)
        {
            return;
        }

        if (!HasParameter(animator, parameter)) return;

        if (animator.GetInteger(parameter) != value)
        {
            if (forceReset)
            {
                animator.enabled = false;
            }
            animator.SetInteger(parameter, value);
            if (forceReset)
            {
                animator.enabled = true;
            }
        }
    }
    
    public static void Trigger(Animator animator, String parameter)
    {
        if (!animator)
        {
            return;
        }

        if (!HasParameter(animator, parameter)) return;
        
        animator.ResetTrigger(ANIM_IDLE);
        animator.ResetTrigger(ANIM_WALK);
        animator.ResetTrigger(ANIM_JUMP);
        animator.ResetTrigger(ANIM_FALLING);
        animator.ResetTrigger(ANIM_LANDING);

        animator.SetTrigger(parameter);
    }
}