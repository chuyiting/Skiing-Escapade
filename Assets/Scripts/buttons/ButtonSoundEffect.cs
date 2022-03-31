using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSoundEffect : MonoBehaviour
{

    private AudioSource audioSource;
    public AudioClip hoverSound;
    public AudioClip clickSound;
    
    private void Awake() {
        audioSource = GetComponent<AudioSource>();
    }
    public void HoverSound()
    {
        audioSource.PlayOneShot(hoverSound);
    }

    public void ClickSound()
    {
        audioSource.PlayOneShot(clickSound);
    }
}
