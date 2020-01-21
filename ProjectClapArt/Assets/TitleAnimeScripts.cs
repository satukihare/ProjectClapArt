using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleAnimeScripts : MonoBehaviour
{
    [SerializeField]private AudioClip GroupLogo_Announce_Kai;
    [SerializeField] private AudioClip GroupLogo_Announce_Nagi;

    [SerializeField] private AudioClip Title_Announce_Kai;
    [SerializeField] private AudioClip Title_Announce_Nagu;

    AudioSource audioSource;
    [SerializeField] private AudioSource TitleBGM;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        Random.Range(0f, 2f);
    }

    public void GroupLogoAnnounce()
    {
        float a = Random.value;

        if (Random.value >0.5f )
        {
            audioSource.PlayOneShot(GroupLogo_Announce_Kai);

            audioSource.clip = Title_Announce_Kai;
            audioSource.PlayDelayed(2.5f);
            audioSource.Play();
        }
        else
        {
            audioSource.PlayOneShot(GroupLogo_Announce_Nagi);

            audioSource.clip = Title_Announce_Nagu;
            audioSource.PlayDelayed(2.5f);
            audioSource.Play();
        }
 
    }

   
}
