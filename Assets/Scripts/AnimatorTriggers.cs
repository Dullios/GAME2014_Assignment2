using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorTriggers : MonoBehaviour
{
    private Animator m_animator;

    private void Start()
    {
        m_animator = GetComponent<Animator>();
    }

    public void OnFallingTransition()
    {
        m_animator.SetBool("isFalling", true);
    }

    public void OnLandingTrigger()
    {
        m_animator.SetBool("isLanding", false);
    }
}
