using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Note {

    //Noteのしゅるい
    public enum NOTE_TYPE {
        FLICK = 0,
        TOUCH,
        UNKNOWN = -1,
    }

    //座標
    Vector2 anchors_min;
    Vector2 anchors_max;

    //スポーンタイミング
    int spawnTime;
    //押す下タイミング
    int pressTime;
    //タイプ
    NOTE_TYPE type;
    //クリック判定
    bool clickFlg = false;
    //生成したNoteのinstance
    GameObject note_instance = null;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="set_anchors_min">表示座標_min</param>
    /// <param name="set_anchors_max">表示座標_max</param>
    /// <param name="span_time">スポーンタイム</param>
    /// <param name="press_time">押下タイム</param>
    /// <param name="note_type">noteのタイプ</par
    public Note(Vector2 set_anchors_min, Vector2 set_anchors_max , int span_time , int press_time , NOTE_TYPE note_type) {
        anchors_min = set_anchors_min;
        anchors_max = set_anchors_max;
        spawnTime = span_time;
        pressTime = press_time;
        type = note_type;
    }

    public Vector2 Anchors_max {
        get { return anchors_max; }
    }

    public Vector2 Anchors_min {
        get { return anchors_min; }
    }

    public int SpawnTime {
        get { return spawnTime; }
    }

    public int PressTime {
        get { return pressTime; }
    }

    public NOTE_TYPE Type {
        get { return type; }
    }

    public bool ClikFlg {
        get{ return clickFlg; }
        set { clickFlg = value; }
    }

    public GameObject NoteInstance {
        get { return note_instance; }
        set { note_instance = value; }
    }

}
