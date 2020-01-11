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
        
    }

    // Update is called once per frame
    void Update()
    {


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
        TitleBGM.time = 0;
        Invoke("PlayBGM", 0.8f);
    }

    void PlayBGM()
    {
        TitleBGM.Play();
    }
}
