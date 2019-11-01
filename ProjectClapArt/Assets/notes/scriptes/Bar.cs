using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bar {
    //開始時間
    float startTime;
    //１小節の時間的長さ
    float length;
    //notesのリスト
    List<Note> notes;

    public float StartTime {
        get { return startTime; }
        set { startTime = value; }
    }

    public float Lingth {
        get { return length; }
        set { length = value; }
    }

    public List<Note> Notes {
        get { return notes; }
        set { notes = value; }
    }
}