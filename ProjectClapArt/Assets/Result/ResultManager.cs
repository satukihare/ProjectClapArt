using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class ResultManager : MonoBehaviour
{

    [SerializeField]
    PlayableDirector Director;

    [SerializeField]
    AudioClip[] voices;

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

                int chara = SelectData.chara_select;
                int score_rank = ResultData.rank;

                AudioSource source = gameObject.GetComponent<AudioSource>();

                //      ボイスID対照表
                //
                //       | スコアランク
                //キャラ |   C B A S
                //-------+--------------
                //ナギ   |   0 1 2 3
                //カイ   |   4 5 6 7
                source.clip = voices[chara * 4 + score_rank];
                source.Play();
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
                if (ResultData.rank > 1)
                {
                    if (SelectData.chara_select == 0)
                    {
                        //ナギend
                        Transition.instance.ChangeScene("NagiEndDialog");
                    }
                    else
                    {
                        //カイend
                       
                        Transition.instance.ChangeScene("KaiEndDialog");
                    }
                }
                else
                {
                    //バッドエンド
                    
                    Transition.instance.ChangeScene("BadEndDialog");
                }

                //例外処理
                Transition.instance.ChangeScene("TitleScene");
            }
        }
    }
}
