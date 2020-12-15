using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformBehaviours : MonoBehaviour
{
    private Animator m_animator;

    // Start is called before the first frame update
    void Start()
    {
        m_animator = GetComponent<Animator>();
    }

    public void OnBounce()
    {
        m_animator.SetBool("isBouncing", false);
    }
}
