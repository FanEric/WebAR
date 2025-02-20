using System;
using UnityEngine;

public class AnimatorCallbacks : MonoBehaviour
{
    public Action<string> OnAnimationComplete;
    public Action<string> OnAnimationStart;
    Animator mAnimator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mAnimator = GetComponent<Animator>();
    }

    public void AnimationStartHandler(string name)
    {
        Debug.Log($"{name} animation start.");
        mAnimator.SetInteger(AssemblyPanel.mAnimArgName, 0);
        OnAnimationStart?.Invoke(name);
    }
    public void AnimationCompleteHandler(string name)
    {
        mAnimator.SetInteger(AssemblyPanel.mAnimArgName, 0);
        Debug.Log($"{name} animation complete.");
        OnAnimationComplete?.Invoke(name);
    }
}
