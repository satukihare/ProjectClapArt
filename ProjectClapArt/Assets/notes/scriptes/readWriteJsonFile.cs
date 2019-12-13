using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Text;

public class readWriteJsonFile : MonoBehaviour {
    /// <summary>
    /// JSONに読み書きするためだけのNoteクラス
    /// 外部では使わないでください
    /// </summary>
    [System.Serializable]
    public class JsonNote {

            /// <summary>
            /// 座標
            /// </summary>
        public Vector2 position = new Vector2(0.0f, 0.0f);

        /// <summary>
        ///スポーンタイミング
        /// </summary>
        public int spawn_time = 10;

        /// <summary>
        /// 押下タイミング
        /// </summary>
        public int press_time = 0;

        /// <summary>
        /// notesの種類
        /// </summary>
        public int note_type = 0;

        /// <summary>
        /// デフォルトコンストラクタ
        /// </summary>
        public JsonNote() { }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="set_position">座標</param>
        /// <param name="set_spawn_time">スポーンする時間</param>
        /// <param name="set_press_time">押下するタイミング</param>
        /// <param name="set_note_type">notesのタイプ</param>
        public JsonNote(Vector2 set_position, int set_spawn_time, int set_press_time, int set_note_type) {
            position = set_position;
            spawn_time = set_spawn_time;
            press_time = set_press_time;
            note_type = set_note_type;
        }
    }

    /// <summary>
    /// JSON に読み書きするためだけのBarクラス
    /// 外部では使わないでください
    /// スタートタイムと長さは現在使いみちがないです
    /// </summary>
    [System.Serializable]
    public class JsonBar {
        //開始時間
        public int startTime;
        //１小節の時間的長さ
        public int length;
        //notesのリスト
        public List<JsonNote> json_notes = new List<JsonNote>();
    }

    /// <summary>
    /// JSONにListとして書き込めないため抽象化するためのクラス
    /// 外部では使わないので触らないでください
    /// </summary>
    [System.Serializable]
    public class JsonBarList {
        public List<JsonBar> json_bars = new List<JsonBar>();
    }

    //譜面データの相対パス
    [SerializeField] string notes_folder_path = @"NotesFileDate\";

    /// <summary>
    /// 初期化
    /// </summary>
    void Start() {}

    /// <summary>
    /// 更新
    /// </summary>
    void Update() { }

    /// <summary>
    /// ファイルへの譜面のJSON形式での書き込み
    /// </summary>
    /// <param name="set_write_file_name">書き込むファイル名</param>
    /// <param name="set_write_bar">書き込む譜面データ</param>
    /// <returns>成功したかどうか</returns>
    public bool writeNotesFileDate(string set_write_file_name, List<Bar> set_write_bar_list) {

        bool write_seccses = false;

        string file_path = Application.dataPath + "/" + notes_folder_path + "/"+set_write_file_name;

        //書き込むBarのリスト
        List<JsonBar> write_bar_list = new List<JsonBar>(set_write_bar_list.Count);
        //JsonBarのリストを抽象化するために最終的にこれに書き込む
        JsonBarList json_bar_list = new JsonBarList();

        //Barのリストから情報を抜いていく
        for (int bar_cnt = 0; bar_cnt < set_write_bar_list.Count; bar_cnt++)
        {
            JsonBar json_bar = new JsonBar();
            int NOTE_LEHGTH = set_write_bar_list[bar_cnt].Notes.Count;

            //BarのNoteから情報を抜いていく
            for (int note_cnt = 0; note_cnt < NOTE_LEHGTH; note_cnt++)
            {
                JsonNote json_note = new JsonNote();
                //座標
                json_note.position = set_write_bar_list[bar_cnt].Notes[note_cnt].Pos;
                json_note.spawn_time = set_write_bar_list[bar_cnt].Notes[note_cnt].SpawnTime;
                json_note.press_time = set_write_bar_list[bar_cnt].Notes[note_cnt].PressTime;

                //Noteの種類（ENUMなので数値に変換している）
                if (set_write_bar_list[bar_cnt].Notes[note_cnt].Type == Note.NOTE_TYPE.FLICK)
                    json_note.note_type = 0;
                else if (set_write_bar_list[bar_cnt].Notes[note_cnt].Type == Note.NOTE_TYPE.TOUCH)
                    json_note.note_type = 1;
                else
                    json_note.note_type = -1;
                //JsonBarに追加
                json_bar.json_notes.Add(json_note);
            }

            json_bar_list.json_bars = write_bar_list;

            //書き込み用リストに追加
            write_bar_list.Add(json_bar);
        }
        Debug.Log("Barの変換完了");

        // UTF-8のテキスト用
        using (System.IO.StreamWriter sw = new System.IO.StreamWriter(file_path))
        {
            // ファイルへテキストデータを出力する
            Debug.Log(JsonUtility.ToJson(json_bar_list));
            //書き込んでる
            sw.Write(JsonUtility.ToJson(json_bar_list));
            write_seccses = true;
        }
        return write_seccses;
    }


    /// <summary>
    /// JSONのファイルからBarデータを読み込む
    /// </summary>
    /// <param name="set_notes_file_name">ファイルネーム</param>
    /// <returns>Barのデータ</returns>
    public List<Bar> readNotesFileDate(string set_notes_file_name) {

        string file_path = Application.dataPath + "/" + notes_folder_path +"/"+ set_notes_file_name;
        FileStream fs = null;

        //BarのListを抽象化するためのラッパークラス
        JsonBarList json_bar_list = null;
        JsonBar json_bars = null;

        try
        {
            //読み込みで開く
            fs = File.Open(file_path, FileMode.Open, FileAccess.Read);
            StreamReader reader = new StreamReader(fs, Encoding.UTF8);

            //ファイルを読み込む
            string all_string_date = reader.ReadToEnd();
            //JSONからクラスに復号
            json_bar_list = JsonUtility.FromJson(all_string_date, typeof(JsonBarList)) as JsonBarList;
        }
        catch (Exception e)
        {
            Debug.Log("FileOpen Err");
            Debug.Log(e);
        }
        finally
        {
            if (fs != null)
            {
                try
                {
                    fs.Dispose();
                }
                catch (IOException ioe)
                {
                    Debug.Log(ioe.Message);
                    fs = null;
                }
            }
        }

        //返すBarのinstance
        List<Bar> return_bar_list = new List<Bar>();
        int BAR_LIST_NUM = json_bar_list.json_bars.Count;

        //Barの情報取得
        for (int bar_cnt = 0; bar_cnt < BAR_LIST_NUM; bar_cnt++)
        {
            JsonBar json_bar = json_bar_list.json_bars[bar_cnt];
            Bar return_bar = new Bar();

            //Barの諸情報を読み込み
            return_bar.StartTime = json_bar.startTime;
            return_bar.Lingth = json_bar.length;

            //Noteの情報取得
            int NOTE_LENGTH = json_bar.json_notes.Count;
            for (int note_cnt = 0; note_cnt < NOTE_LENGTH; note_cnt++)
            {
                JsonNote json_note = json_bar.json_notes[note_cnt];
                Note ret_note = null;

                //情報の抜き出し　Noteの種類をifで制御
                if (json_note.note_type == 0)
                {
                    ret_note = new Note(json_note.position, json_note.spawn_time, json_note.press_time, Note.NOTE_TYPE.FLICK);
                }
                else if (json_note.note_type == 1)
                {
                    ret_note = new Note(json_note.position, json_note.spawn_time, json_note.press_time, Note.NOTE_TYPE.TOUCH);
                }
                else
                {
                    ret_note = new Note(json_note.position, json_note.spawn_time, json_note.press_time, Note.NOTE_TYPE.UNKNOWN);
                }
                //初期化しておく
                ret_note.ClikFlg = false;
                //BarにNote情報を書き込む
                return_bar.Notes.Add(ret_note);
            }
            //Barのリストに情報を書き込む
            return_bar_list.Add(return_bar);
        }
        return return_bar_list;
    }
}
