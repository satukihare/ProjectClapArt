using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Text;

public class readWriteJsonFile : MonoBehaviour {

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

    //ゲームフォルダのパス
    string game_path;

    //譜面データの相対パス
    [SerializeField] string notes_folder_path = @"NotesFileDate\";

    // Start is called before the first frame update
    void Start() {
        //JsonNote note = new JsonNote();
        //note.position.x = 1.0f;
        //note.position.y = 2.0f;
        //note.press_time = 2000;
        //note.spawn_time = 1000;
        //note.note_type = 1;
        //
        //JsonBar bars = new JsonBar();
        //
        //bars.json_notes.Add (new JsonNote(new Vector2(-1, -1), 1500, 3100, 1));
        //bars.json_notes.Add (new JsonNote(new Vector2(1, -1), 1900, 3500,  1));
        //bars.json_notes.Add (new JsonNote(new Vector2(-1, 1), 2300, 3900,  1));


        Bar bars = new Bar();
        
        bars.Notes.Add (new Note(new Vector2(-1, -1), 1500, 3100, Note.NOTE_TYPE.FLICK));
        bars.Notes.Add (new Note(new Vector2(1, -1), 1900, 3500,  Note.NOTE_TYPE.FLICK));
        bars.Notes.Add (new Note(new Vector2(-1, 1), 2300, 3900, Note.NOTE_TYPE.FLICK));

        this.writeNotesFileDate("test.json", bars);

        //Debug.Log(JsonUtility.ToJson(bars));

        //アプリケーションのデータパスを取得
        game_path = Application.dataPath;
    }
    void Update() { }


    /// <summary>
    /// ファイルへの譜面のJSON形式での書き込み
    /// </summary>
    /// <param name="set_write_file_name">書き込むファイル名</param>
    /// <param name="set_write_bar">書き込む譜面データ</param>
    /// <returns>成功したかどうか</returns>
    public bool writeNotesFileDate(string set_write_file_name, Bar set_write_bar) {

        bool write_seccses = false;

        string file_path = game_path + notes_folder_path + set_write_file_name;
        JsonBar json_bar = new JsonBar();

        int LEHGTH = set_write_bar.Notes.Count;

        for (int cnt = 0; cnt < LEHGTH; cnt++) {
            JsonNote json_note = new JsonNote();
            //座標
            json_note.position = set_write_bar.Notes[cnt].Pos;
            json_note.spawn_time = set_write_bar.Notes[cnt].SpawnTime;
            json_note.press_time = set_write_bar.Notes[cnt].PressTime;

            if (set_write_bar.Notes[cnt].Type == Note.NOTE_TYPE.FLICK)
                json_note.note_type = 0;
            else if (set_write_bar.Notes[cnt].Type == Note.NOTE_TYPE.TOUCH)
                json_note.note_type = 1;
            else
                json_note.note_type = -1;

            //書き込み用Noteに追加
            json_bar.json_notes.Add(json_note);
        }
        // UTF-8のテキスト用
        using (System.IO.StreamWriter sw = new System.IO.StreamWriter(file_path)) {
            // ファイルへテキストデータを出力する
            sw.Write(JsonUtility.ToJson(json_bar));
            write_seccses = true;
        }
        return write_seccses;
    }


    /// <summary>
    /// JSONのファイルからBarデータを読み込む
    /// </summary>
    /// <param name="set_notes_file_name">ファイルネーム</param>
    /// <returns>Barのデータ</returns>
    public Bar readNotesFileDate(string set_notes_file_name) {

        string file_path = game_path + notes_folder_path + set_notes_file_name;
        FileStream fs = null;
        JsonBar json_bars = null;

        try {
            //読み込みで開く
            fs = File.Open(file_path, FileMode.Open, FileAccess.Read);
            StreamReader reader = new StreamReader(fs, Encoding.UTF8);

            //ファイルを読み込む
            string all_string_date = reader.ReadToEnd();

            //Debug.Log(JsonUtility.FromJson);
            json_bars = JsonUtility.FromJson(all_string_date, typeof(JsonBar)) as JsonBar;
        }
        catch (Exception e) {
            Debug.Log("FileOpen Err");
            Debug.Log(e);
        }
        finally {
            if (fs != null) {
                try {
                    fs.Dispose();
                }
                catch (IOException ioe) {
                    Debug.Log(ioe.Message);
                    fs = null;
                }
            }
        }

        //返すBarのinstance
        Bar return_bar = new Bar();

        //曲データを収集
        return_bar.StartTime = json_bars.startTime;
        return_bar.Lingth = json_bars.length;

        //Noteデータを取得
        int LENGTH = json_bars.json_notes.Count;

        for (int cnt = 0; cnt < LENGTH; cnt++) {
            JsonNote json_note = json_bars.json_notes[cnt];
            Note ret_note = null;

            if (json_note.note_type == 0) {
                ret_note = new Note(json_note.position, json_note.spawn_time, json_note.press_time, Note.NOTE_TYPE.FLICK);
            }
            else if (json_note.note_type == 1) {
                ret_note = new Note(json_note.position, json_note.spawn_time, json_note.press_time, Note.NOTE_TYPE.TOUCH);
            }
            else {
                ret_note = new Note(json_note.position, json_note.spawn_time, json_note.press_time, Note.NOTE_TYPE.UNKNOWN);
            }
            //リストに追加
            return_bar.Notes.Add(ret_note);
        }

        return return_bar;
    }

}
