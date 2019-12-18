using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LinePos : MonoBehaviour
{
    private static float Max = 1.0f; 
    public  Vector2 pos ;
    public bool StartPosflug { set; get; }
    private Vector2 StartMax, StartMin;
    private NoteObject note = null;

    [SerializeField] private Image Line = null;
    [SerializeField] private AudioClip[] SE  = null;
    [SerializeField] private AudioSource audios = null;
    [SerializeField] private ToolManager toolManager = null;


    // Start is called before the first frame update
    void Start()
    {
        StartPosflug = true;
        pos.x = 0.0f;
        pos.y = 0.0f;
        StartMax = Line.rectTransform.anchorMax ;
        StartMin = Line.rectTransform.anchorMin ;
    }

    private void Update()
    {
        if (pos.x != 0.0f)
        {
            StartPosflug = true;
        }
        else
        {
            StartPosflug = false;
        }
    }

    public void SetPos()
    {
        Line.rectTransform.anchorMax = StartMax + pos;
        Line.rectTransform.anchorMin = StartMin + pos;
    }

    public void ResetPos()
    {
        pos.x = 0.0f;
        pos.y = 0.0f;
        Line.rectTransform.anchorMax = StartMax;
        Line.rectTransform.anchorMin = StartMin;
    }

    public void setMaxPos()
    {
        pos.x = Max;
        pos.y = 0.0f;
        Line.rectTransform.anchorMax = StartMax + pos;
        Line.rectTransform.anchorMin = StartMin + pos;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       note = collision.gameObject.GetComponent<NoteObject>();

        switch (note.NOTE_TYPE)
        {
            case Note.NOTE_TYPE.UNKNOWN:
                //ノーツがでてくるSE
                audios.PlayOneShot(SE[0]);
                break;

            case Note.NOTE_TYPE.FLICK:
                audios.PlayOneShot(SE[1]);
                break;

            case Note.NOTE_TYPE.TOUCH:
                audios.PlayOneShot(SE[2]);
                break;
        }
        Debug.Log("Hit Note time" + toolManager.NowMusicTimer);
    }
}
