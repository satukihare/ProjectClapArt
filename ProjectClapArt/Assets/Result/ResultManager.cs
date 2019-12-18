using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class ResultManager : MonoBehaviour
{

    [SerializeField]
    PlayableDirector Director;

    bool ScoreVoice = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Director.state == PlayState.Paused)
        {
            if (!ScoreVoice)
            {
                ScoreVoice = true;



            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (Director.state != PlayState.Paused)
            {
                Director.time = Director.duration;
            }
            else
            {
                Transition.instance.ChangeScene("TitleScene");
            }
        }
    }
}
