// simple script to play sound effect for player states

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Sound : StateMachineBehaviour
{
    public AudioClip clip;
    public float volume = 1f;

    private AudioSource audioSource;
    private float lastNormalizedTime = 0f;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // get AudioSource from parent so it works if Animator is a child
        if (audioSource == null)
            audioSource = animator.GetComponentInParent<AudioSource>();

        if (clip && audioSource)
            audioSource.PlayOneShot(clip, volume);

        lastNormalizedTime = 0f;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (clip && audioSource)
        {
            // check loop restart: normalizedTime goes from 0 -> 1 -> back to 0 each loop
            float currentNormalizedTime = stateInfo.normalizedTime % 1f;

            // loop restarted
            if (currentNormalizedTime < lastNormalizedTime) 
            {
                audioSource.PlayOneShot(clip, volume);
            }

            lastNormalizedTime = currentNormalizedTime;
        }
    }
}
