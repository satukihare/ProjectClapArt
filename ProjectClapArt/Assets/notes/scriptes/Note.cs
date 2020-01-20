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
    Vector2 pos;
    //スポーンタイミング
    int spawnTime;
    //押す下タイミング
    int pressTime;
    //タイプ
    NOTE_TYPE type;
    //popフラグ
    bool popFlg = false;
    //クリック判定
    bool clickFlg = false;
    //生成したNoteのinstance
    GameObject note_instance = null;
    //クリックするタイミング
    int pressMusicTimeNum = 0;

    /// <summary>
    /// パラメータ
    /// </summary>
    /// <param name="set_pos">表示座標</param>
    /// <param name="span_time">スポーンタイム</param>
    /// <param name="press_time">押下タイム</param>
    /// <param name="note_type">noteのタイプ</param>
    public Note(Vector2 set_pos , int span_time , int press_time , NOTE_TYPE note_type) {
        pos = set_pos;
        spawnTime = span_time;
        pressTime = press_time;
        type = note_type;
    }
//--プロパティ--
    public Vector2 Pos {
        get { return pos; }
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

    public int PressMusicTime {
        get { return this.pressMusicTimeNum; }
        set { pressMusicTimeNum = value; }
    }

    public bool PopFlg {
        get { return popFlg; }
        set { popFlg = value; }
    }

}
