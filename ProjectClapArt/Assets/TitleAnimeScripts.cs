using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleAnimeScripts : MonoBehaviour
{
    [SerializeField]private AudioClip GroupLogo_Announce;
    
    [SerializeField] private AudioClip Title_Announce;

    AudioSource audioSource;
    [SerializeField] private AudioSource TitleBGM;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    //void GroupLogoAnnounce()
    //{
    //   audioSource.PlayOneShot(GroupLogo_Announce);

    //  audioSource.clip = Title_Announce;
    //  audioSource.PlayDelayed(2.5f);
    //  audioSource.Play();

 
    //}

   
}
