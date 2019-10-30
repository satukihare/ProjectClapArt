using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bar : MonoBehaviour {
    //開始時間
    float startTime;
    //１小節の時間的長さ
    float length;
    //notesのリスト
    List<Note> notes;

    public float StartTime {
        get { return startTime; }
    }

    public float Lingth {
        get { return length; }
    }

    public List<Note> Notes {
        get { return notes; }
        set { notes = value; }
    }
}