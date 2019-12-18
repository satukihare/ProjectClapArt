using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    private int minute;
    public float seconds;
    //　前のUpdateの時の秒数
    private float oldSeconds;
    //　タイマー表示用テキスト
    [SerializeField] private Text timerText = null;
    public bool flug { set; get; }

    void Start()
    {
        flug = false;
        minute = 0;
        seconds = 0f;
        oldSeconds = 0f;
        timerText.text = minute.ToString("00") + ":" + ((int)seconds).ToString("00");
    }

    void Update()
    {
        if (flug == true)
        {
            PlayTimer();
        }
    }

    public void addTime(float num)
    {
        seconds += num;

        if (seconds < 0)
        {
            seconds = 60 + seconds;
            if (minute > 0)
            {
                minute--;
            }
            else
            {
                seconds = 0;
            }
        }

        if (seconds >= 60f)
        {
            minute++;
            seconds = seconds - 60;
        }

        
        timerText.text = minute.ToString("00") + ":" + ((int)seconds).ToString("00");
    }

    public void SetPageTime(float num)
    {
        seconds = num;
        if(seconds >= 60f)
        {
            seconds = seconds - 60;
        }
        minute = (int)(num/60);
        timerText.text = minute.ToString("00") + ":" + ((int)seconds).ToString("00");
    }

    private void PlayTimer()
    {
        seconds += Time.deltaTime;
        if (seconds >= 60f)
        {
            minute++;
            seconds = seconds - 60;
        }
        //　値が変わった時だけテキストUIを更新
        if ((int)seconds != (int)oldSeconds)
        {
            timerText.text = minute.ToString("00") + ":" + ((int)seconds).ToString("00");
        }
        oldSeconds = seconds;
    }
}
