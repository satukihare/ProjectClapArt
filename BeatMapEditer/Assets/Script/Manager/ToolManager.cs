using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource = null;
    [SerializeField] private LinePos Line = null;
    [SerializeField] private NotesPage Nowpage= null;
    [SerializeField] private NotesPage Maxpage = null;
    [SerializeField] private float BPM = 120.0f;
    public bool  flug { set; get; }
    public float MaxMusicTime { set; get; }
    public float PageEndMusicTimer { set; get; }
    public float NowMusicTimer { set; get; }
    public float MusicTimer { set; get; }
    public float NotesTimer {set; get;}

    private static float Min = 60.0f; 

    // Start is called before the first frame update
    void Start()
    {
        flug = false;
        MusicTimer = 0.0f;
        NotesTimer = 0.0f;
        PageEndMusicTimer = 0.0f;
        Maxpage.setPage((int)audioSource.clip.length/4);
        MaxMusicTime = audioSource.clip.length;
    }

    // Update is called once per frame
    void Update()
    {
        if (flug == true)
        {
            ScrollNotes();
        }
    }

    public void PlayStart()
    {
        Debug.Log("Start Time" + MusicTimer);
        flug = true;
        audioSource.Play();
        audioSource.time = MusicTimer;
    }

    public void PlayStop()
    {
        flug = false;
        MusicTimer = audioSource.time;
        audioSource.Stop();
        Debug.Log("Stop Time" + MusicTimer);
    }

    private void ScrollNotes()
    {
        
        NotesTimer += Time.deltaTime;
        NowMusicTimer = audioSource.time;
        //var movement = 1.0f - Min / BPM;
        decimal movement = (decimal)(1 - Min / BPM);
        Line.pos.x = NotesTimer / 2 * (float)movement;
        //Line.pos.x = (NotesTimer / 2.4f) ;
        Line.SetPos();

        if (Line.pos.x >= 1.0f)
        {
            if (audioSource.time < audioSource.clip.length)
            {
                PageEndMusicTimer = audioSource.time;
                Nowpage.NextPage();
                Line.ResetPos();
                NotesTimer = 0.0f;
                Debug.Log(Nowpage.nowPage + "page time is" + PageEndMusicTimer);
            }
            else
            {
                PlayStop();
                MusicTimer = 0;
            }
        }
    }

}
