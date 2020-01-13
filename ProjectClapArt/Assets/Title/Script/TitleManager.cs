using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TitleManager : MonoBehaviour
{
    [SerializeField]
    PlayableDirector Director;

    [SerializeField]
    AudioSource TitleBGM;
    // Start is called before the first frame update
    void Start()
    {
        ResetBGM();
    }

    // Update is called once per frame
    void Update()
    {
        if (Director.state == PlayState.Paused)
        {
            ResetBGM();
            Director.time = 0.0f;
            Director.Play();
        }

        if (Input.GetMouseButtonDown(0))
        {
           if (Director.time <= 3.2)
           {
               Director.time = 3.2;
           }
           else
           {
                Transition.instance.ChangeScene("DialogScene");
           }
        }

        if (Input.GetKey(KeyCode.P))
        {
            ResetBGM();
        }


    }

    void ResetBGM()
    {
        TitleBGM.Stop();
        TitleBGM.time = 0;
        Invoke("PlayBGM", 1.8f);
    }

    void PlayBGM()
    {
        TitleBGM.Play();
    }
}
