using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMnger : MonoBehaviour {

    //ゲームモード
    enum GAME_MODE {
        //待機状態
        GAME_WAIT = 0,
        //notes出現状態
        NOTE_SPAWN,
        //notesをtouch
        NOTE_TOUCH,
        //ゲーム終了
        GAME_END,
        //未定義
        UNKNOWN = -1,
    }
    //ゲームの状態
    GAME_MODE game_state = GAME_MODE.GAME_WAIT;

    [SerializeField] float BPM = 0;

    [SerializeField] float NOTE = 0;

    //音符の長さ
    [SerializeField] int whole_note;

    //ゲーム開始時の時間(ms)
    float start_game_time = 0;
    //ゲーム中のフラグ
    bool game_flg = false;
    //オーディオマネージャ
    [SerializeField] AudioSource music = null;

    //ポップするときのSE
    [SerializeField] AudioClip pop_se = null;

    //スポーンさせるnotesオブジェクト
    [SerializeField] GameObject spawn_note_object = null;

    //notesを入れる小節のリスト
    List<Bar> bars = null;

    //現在読んでいるのバーの番号
    int bar_counter = 0;

    //現在読んでいるnoteの番号
    int note_counter = 0;

    //音楽の再生位置を1000倍したものが基準
    int music_time_num = 0;

    //タップの許容できる誤差
    [SerializeField] int more_diff_num = 100;

    //goodタイミング
    [SerializeField] int good_diff_time_num = 10;

    //入力Mnger
    [SerializeField] InputManager track_pad_input = null;

    //切り捨てる数値量
    [SerializeField] int round_digits = 20;

    //JSON形式の譜面データを読み込む
    [SerializeField] readWriteJsonFile read_write_json_file = null;


    [SerializeField] GameObject good_obj = null;
    [SerializeField] GameObject bad_obj = null;

    public float WholeNote {
        get { return whole_note; }
    }

    /// <summary>
    /// 初期化
    /// </summary>
    void Start() {

        //音符の長さを計測
        whole_note = (int)(60.0f / BPM * NOTE * 1000.0f);

        music = this.GetComponent<AudioSource>();

        //譜面の読み込み
        if (read_write_json_file == null) Debug.Log("readWriteJsonFile nullptr !!");

        bars = read_write_json_file.readNotesFileDate("test.json");
    }

    /// <summary>
    /// 更新
    /// </summary>
    void Update() {

        //音のタイミング
        this.music_time_num = (int)(music.time * 1000.0f);

        //待機状態
        if (game_state == GAME_MODE.GAME_WAIT) {
            gameWait();
        }
        //スポーン状態
        else if (game_state == GAME_MODE.NOTE_SPAWN) {
            gameNoteSpawn();
        }
        //touch状態
        else if (game_state == GAME_MODE.NOTE_TOUCH) {
            gameNoteTouch();
        }
        //イレギュラー値
        else { }

        if (bars != null)
            if (bar_counter < bars.Count)
                if (music_time_num > bars[bar_counter].StartTime + bars[bar_counter].Lingth)
                    game_state = GAME_MODE.NOTE_SPAWN;
    }

    /// <summary>
    /// ゲーム開始
    /// </summary>
    void gameStart() {
        //ゲーム開始時間を記録
        this.start_game_time = Time.time;
        //ゲーム中に
        game_flg = true;
        //音楽再生
        music.Play();

        //ゲームをスポーン状態へ
        game_state = GAME_MODE.NOTE_SPAWN;
    }

    /// <summary>
    /// 待機状態
    /// </summary>
    void gameWait() {
        //ゲーム開始
        if (track_pad_input.Tap) {
            gameStart();
        }

    }

    /// <summary>
    /// スポーン状態
    /// </summary>
    void gameNoteSpawn() {

        //描画時の不要な桁の切り捨て
        int msc_time_rud_dgts = music_time_num / round_digits;
        msc_time_rud_dgts = msc_time_rud_dgts * round_digits;

        //小節の書き込みタイミングまでスキップ
        if (bars[bar_counter].StartTime > msc_time_rud_dgts)
            return;

        //スポーンする小節
        List<Note> notes = bars[bar_counter].Notes;

        //スポーンする
        foreach (Note note in notes) {
            //対象のnoteまでスキップ
            if (notes[note_counter] != note)
                continue;

            //スポーンするnotesがあればだす
            if (msc_time_rud_dgts == note.SpawnTime) {
                GameObject pop_notes = Instantiate(spawn_note_object, note.Pos, Quaternion.identity);
                //notesにinstanceをセット
                note.NoteInstance = pop_notes;
                //カウント
                note_counter++;
                //最後のnotesか判定
                if (note == notes[notes.Count - 1]) {
                    //次の小節へ
                    bar_counter++;
                    //タッチモードへ
                    game_state = GAME_MODE.NOTE_TOUCH;

                    //Counterをゼロに
                    note_counter = 0;
                }
                break;
            }
        }
    }

    /// <summary>
    /// touch状態
    /// </summary>
    void gameNoteTouch() {

        //トラックパッドがtouchされればTrue
        bool flick_flg = track_pad_input.Tap | track_pad_input.Flick | track_pad_input.FlickStart | track_pad_input.FlickEnd;

        //トラックパッドを使用したときのみ処理
        if (!flick_flg)
            return;

        //押した時間
        int press_time = music_time_num;

        //スポーンする小節リスト
        List<Note> notes = bars[bar_counter - 1].Notes;

        //判定
        foreach (Note note in notes) {
            //クリックされていたNoteは判定しない
            if (note.ClikFlg)
                continue;

            //差分を取る
            int diff = touchAbsDiffCal(press_time, note.PressTime);

            //誤差から判定する
            judgeTouchTimming(diff, note);
        }
    }

    /// <summary>
    /// 差分を取り絶対値を返す
    /// </summary>
    /// <param name="set_press_time">押下した時間</param>
    /// <param name="set_note_press_time">noteのtouch時間</param>
    /// <returns>Abs（押下ーnoteのtouch時間）</returns>
    private int touchAbsDiffCal(int set_press_time, int set_note_press_time) {

        //差分を作成
        int diff = set_note_press_time - set_press_time;

        //絶対値をとる
        diff = Mathf.Abs(diff);

        return diff;
    }

    /// <summary>
    /// 差分を基に判定を行う
    /// </summary>
    /// <param name="set_diff">誤差</param>
    /// <param name="set_target_note">判定する対象のNote</param>
    private void judgeTouchTimming(int set_diff, Note set_target_note) {

        //ベストタイミング
        if (set_diff < good_diff_time_num) {
            //フリックフラグを発火
            set_target_note.ClikFlg = true;
            Destroy(set_target_note.NoteInstance);
            Debug.Log("good timming");

        }
        //ちょっと惜しいとき
        else if (set_diff < more_diff_num) {
            //フリックフラグを発火
            set_target_note.ClikFlg = true;
            Destroy(set_target_note.NoteInstance);
            Debug.Log("miss timming");
        }
        //完全にタイミングを外した場合
        //else if(more_diff_num < diff ) {
        //    Debug.Log("out");
        //    Destroy(note.NoteInstance);
        //    break;
        //}
    }
}
