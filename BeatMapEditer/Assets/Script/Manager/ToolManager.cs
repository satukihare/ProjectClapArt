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
    public float Bar { set; get; }

    private bool herfflug, barflug;
    private float Correction = 0.0f;
    private static float Min = 60.0f; 

    // Start is called before the first frame update
    void Start()
    {
        flug = herfflug = barflug = false;
        MusicTimer = 0.0f;
        NotesTimer = 0.0f;
        PageEndMusicTimer = 0.0f;
        Bar = Min / BPM * 4;
        Maxpage.setPage((int)audioSource.clip.length/4);
        MaxMusicTime = audioSource.clip.length;

        if ((BPM/Min) % 3.0f == 0)
        {
            Debug.Log("補正オン");
            Correction = 0.08f;
        }
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
        Debug.Log("1Bar Time is" + Bar);
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
        
        var movement = 1 - Min / BPM;
        Line.pos.x = NotesTimer/2 * (movement + Correction);
        //Line.pos.x = (NotesTimer / 2.4f) ;

        if (NotesTimer >= Bar && herfflug == false)
        {
            herfflug = true;
            Line.pos.x = 0.5f;
        }

        if (NotesTimer >= (Bar * 2) && barflug == false)
        {
            barflug = true;
            Line.pos.x = 1.0f;
            //Line.pos.x = 0.99f;
        }

        Line.SetPos();

        if (Line.pos.x >= 1.0f)
        {
            if (audioSource.time < audioSource.clip.length)
            {
                herfflug = barflug = false;
                PageEndMusicTimer = audioSource.time;
                Nowpage.NextPage();
                Line.ResetPos();
                NotesTimer = 0.0f;
                Debug.Log(Nowpage.nowPage + "page time is" + PageEndMusicTimer);
            }
            else
            {
                herfflug = barflug = false;
                PlayStop();
                MusicTimer = 0;
            }
        }
    }

}
