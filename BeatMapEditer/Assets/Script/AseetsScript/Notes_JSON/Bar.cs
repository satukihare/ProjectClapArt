
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bar
{
    //開始時間
    int startTime;
    //１小節の時間的長さ
    int length;
    //notesのリスト
    List<Note> notes = new List<Note>();

    public int StartTime
    {
        get { return startTime; }
        set { startTime = value; }
    }

    public int Lingth
    {
        get { return length; }
        set { length = value; }
    }

    public List<Note> Notes
    {
        get { return notes; }
        set { notes = value; }
    }
}