using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD : MonoBehaviour
{
    private Animator animator;

    protected void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void FadetoBlack()
    {
        animator.SetTrigger("Fade");
    }

    public void Unfade()
    {
        animator.SetTrigger("Unfade");
    }
}
