using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

enum GaugeID{
    Voltage = 0,
    Rhythm,
    Bonus
}
public class ResultManager : MonoBehaviour
{

    [SerializeField]
    PlayableDirector Director;

    [SerializeField]
    AudioClip[] voices;

    [SerializeField]
    GameObject[] rankIcons;

    [SerializeField]
    Image[] gauges;

    [SerializeField]
    float fillRate;

    bool ScoreVoice = false;

    // Start is called before the first frame update
    void Start()
    {
        int i = 0;
        int score_rank = ResultData.rank;
        foreach (GameObject icon in rankIcons)
        {
            icon.SetActive(i == score_rank);    
            ++i;
        }
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
                //キャラ |    C B A
                //-------+-------------
                //ナギ   |    0 1 2
                //カイ   |    3 4 5
                source.clip = voices[chara * 3 + score_rank];
                source.Play();
            }
        }
        if (gauges[(int)GaugeID.Voltage].fillAmount < ResultData.voltage_rate)
        {
            gauges[(int)GaugeID.Voltage].fillAmount += fillRate;
            if (gauges[(int)GaugeID.Voltage].fillAmount > ResultData.voltage_rate)
                gauges[(int)GaugeID.Voltage].fillAmount = ResultData.voltage_rate;
        }
        if (gauges[(int)GaugeID.Rhythm].fillAmount < ResultData.score_rate)
        {
            gauges[(int)GaugeID.Rhythm].fillAmount += fillRate;
            if (gauges[(int)GaugeID.Rhythm].fillAmount > ResultData.score_rate)
                gauges[(int)GaugeID.Rhythm].fillAmount = ResultData.score_rate;
        }
        if (gauges[(int)GaugeID.Bonus].fillAmount < ResultData.bonus_rate)
        {
            gauges[(int)GaugeID.Bonus].fillAmount += fillRate;
            if (gauges[(int)GaugeID.Bonus].fillAmount > ResultData.bonus_rate)
                gauges[(int)GaugeID.Bonus].fillAmount = ResultData.bonus_rate;
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
