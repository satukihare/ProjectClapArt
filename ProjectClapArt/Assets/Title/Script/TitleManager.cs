using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TitleManager : MonoBehaviour
{
    [SerializeField]
    PlayableDirector Director;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Director.state != PlayState.Paused)
            {
                Director.time = Director.duration;
            }
            else
            {
                Transition.instance.ChangeScene("DialogScene");
            }
        }
    }
}
