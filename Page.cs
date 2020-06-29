using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Page : MonoBehaviour
{
    private Animator animator;
    [SerializeField]
    private GameObject background;
    [SerializeField]
    private Text pageText;
    public void Init()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
            return;

        gameObject.SetActive(false);
        animator.enabled = false;
    }

    public void Play()
    {
        gameObject.SetActive(true);
        if (animator != null)
            animator.enabled = true;

        Animator backgroundAnimator = background.GetComponent<Animator>();
        if (backgroundAnimator != null)
            backgroundAnimator.enabled = true;
        if (pageText != null)
            pageText.gameObject.SetActive(true);
    } 

    public void Stop()
    {
        gameObject.SetActive(false);
        if (animator != null)
            animator.enabled = false;

        Animator backgroundAnimator = background.GetComponent<Animator>();
        if (backgroundAnimator != null)
            backgroundAnimator.enabled = false;
        if (pageText != null)
            pageText.gameObject.SetActive(false);
    }
}
