using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class readWriteJsonFile : MonoBehaviour{

    //JSONに読み書きするためだけのNoteクラス
    [System.Serializable]
    public class JsonNote {
        public Vector2 position = new Vector2(0.0f, 0.0f);
        //スポーンタイミング
        public int spawn_time = 10;
        //押す下タイミング
        public int press_time = 0;
        //タイプ
        public int note_type = 0;

        //コンストラクタ
        public JsonNote() { }
        public JsonNote(Vector2 set_position, int set_spawn_time, int set_press_time, int set_note_type) {
            position = set_position;
            spawn_time = set_spawn_time;
            press_time = set_press_time;
            note_type = set_note_type;
        }
    }
    //JSON に読み書きするためだけのBarクラス
    [System.Serializable]
    public class JsonBar {
        //開始時間
        public float startTime;
        //１小節の時間的長さ
        public float length;
        //notesのリスト
        public List<JsonNote> json_notes = new List<JsonNote>();

    }

    // Start is called before the first frame update
    void Start() {
        JsonNote note = new JsonNote();
        note.position.x = 1.0f;
        note.position.y = 2.0f;
        note.press_time = 2000;
        note.spawn_time = 1000;
        note.note_type = 1;

        JsonBar bars = new JsonBar();

        bars.json_notes.Add (new JsonNote(new Vector2(-1, -1), 1500, 3100, 1));
        bars.json_notes.Add (new JsonNote(new Vector2(1, -1), 1900, 3500,  1));
        bars.json_notes.Add (new JsonNote(new Vector2(-1, 1), 2300, 3900,  1));

        Debug.Log(JsonUtility.ToJson(bars));
    }

    // Update is called once per frame
    void Update() { 

    }

    public void writeJsonDate() {
        //JSON化
        Debug.Log(JsonUtility.ToJson(this));
    }
}
