using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// GameMnger用のスーパークラス
/// </summary>
public class GameSystem : MonoBehaviour {

    /// <summary>
    /// ゲームの遷移状態
    /// </summary>
    protected enum GAME_MODE {
        //待機状態
        GAME_WAIT = 0,
        //notes出現状態
        NOTE_SPAWN,
        //notesをtouch
        NOTE_TOUCH,
        //次のBarに移行するタイミング
        NEXT_BAR,
        //選択モード
        GAME_CHOSE,
        //ゲーム終了
        GAME_END,
        //未定義
        UNKNOWN = -1,
    }

    [SerializeField] protected float BPM = 0;

    [SerializeField] protected float NOTE = 0;

    //音符の長さ
    [SerializeField] protected int whole_note;

    //ゲームの状態
    protected GAME_MODE game_state = GAME_MODE.GAME_WAIT;

    //ゲーム開始時の時間(ms)
    protected float start_game_time = 0;
    //ゲーム中のフラグ
    protected bool game_flg = false;

    //オーディオマネージャ
    [SerializeField] protected AudioSource music = null;

    //ポップするときのSE
    [SerializeField] protected AudioClip pop_se = null;

    //スポーンさせるnotesオブジェクト
    [SerializeField] protected GameObject spawn_note_object = null;

    //notesを入れるリスト
    protected List<Bar> bars = null;

    //現在読んでいるBarの番号
    protected int bar_counter = 0;

    //現在読んでいるnoteの番号
    protected int note_counter = 0;

    //音楽の再生位置を1000倍したものが基準
    protected int music_time_num = 0;

    //タップで許容できる誤差
    [SerializeField] protected int more_diff_num = 100;

    //goodタイミング
    [SerializeField] protected int good_diff_time_num = 10;

    //入力Mnger
    [SerializeField] protected InputManager track_pad_input = null;

    //切り捨てる数値量
    [SerializeField] protected int round_digits = 20;

    //JSON形式の譜面データを読み込む
    [SerializeField] protected readWriteJsonFile read_write_json_file = null;

//--プロパティ--
    public float WholeNote {
        get { return whole_note; }
    }
    public float Bpm {
        get { return BPM; }
    }

    /// <summary>
    /// 初期化
    /// </summary>
    protected void Start() {

        //音符の長さを計測
        whole_note = (int)(60.0f / BPM * NOTE * 1000.0f);

        music = this.GetComponent<AudioSource>();

        //譜面の読み込み
        if (read_write_json_file == null) Debug.Log("readWriteJsonFile nullptr !!");

        bars = read_write_json_file.readNotesFileDate("test.json");
        ResultData.total_notes = 0;
        ResultData.hit_notes = 0;
        foreach (Bar bar in bars)
        {
            ResultData.total_notes += bar.Notes.Count;
        }
    }

    /// <summary>
    /// 更新
    /// 抽象メソッド
    /// </summary>
    protected virtual void Update() {}

    /// <summary>
    /// 待機状態
    /// </summary>
    protected void gameWait() {
        if (track_pad_input.Tap) {
            gameStart();
        }
    }

    /// <summary>
    /// ゲーム開始
    /// </summary>
    protected void gameStart() {
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
    /// スポーン状態
    /// </summary>
    protected void gameNoteSpawn() {

        //描画の不要な桁の切り捨て
        int msc_time_rud_dgts = 0;
        msc_time_rud_dgts = music_time_num / round_digits;
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
    protected void gameNoteTouch() {
        //スポーンした小節リスト
        List<Note> notes = bars[(bar_counter <= 0 ? 0 : bar_counter)].Notes;

        //notesをチェックする
        notesTimingCheck(notes);

        //全てのノードがクリックされているなら選択へ遷移する
        if (checkNoteAllClick(notes))
            return;

        //トラックパッドがtouchされればTrue
        bool flick_flg = track_pad_input.Tap | track_pad_input.Flick | track_pad_input.FlickStart | track_pad_input.FlickEnd;

        //トラックパッドを使用したときのみ処理
        if (!flick_flg)
            return;

        //押した時間
        int press_time = music_time_num;

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
    /// 読んでいるBarのNotesが全てクリックされているなら
    /// 次の遷移へ行く
    /// !!仮想メソッド!!
    /// </summary>
    /// <param name="notes">NoteのList</param>
    /// <returns>全てクリックされているならTrue</returns>
    protected virtual bool checkNoteAllClick(List<Note> notes) { return false; }

    /// <summary>
    /// notesのタイミングをチェックしタイミングが通り過ぎたものにtrueを入れる
    /// </summary>
    /// <param name="notes">NoteのList</param>
    protected void notesTimingCheck(List<Note> notes) {
        foreach (Note note in notes) {
            if (note.ClikFlg)
                continue;

            //タイミングが過ぎているのでTrueをいれる
            if (note.PressTime + 1000 < music_time_num) {
                note.ClikFlg = true;

                //今はタイミングが間違ってもnotesを消している
                //nullならなにもしない
                if (note.NoteInstance != null)
                    Destroy(note.NoteInstance);
                else
                    Debug.Log("Note Instance is NullPtr ! ! ! ");
            }
        }
    }

    /// <summary>
    /// 差分を基に判定を行う
    /// </summary>
    /// <param name="set_diff">誤差</param>
    /// <param name="set_target_note">判定する対象のNote</param>
    protected void judgeTouchTimming(int set_diff, Note set_target_note) {

        //ベストタイミング
        if (set_diff < good_diff_time_num) {
            //フリックフラグを発火
            set_target_note.ClikFlg = true;
            Destroy(set_target_note.NoteInstance);
            Debug.Log("good timming");
            ResultData.hit_notes += 1;
        }
        //ちょっと惜しいとき
        else if (set_diff < more_diff_num) {
            //フリックフラグを発火
            set_target_note.ClikFlg = true;
            Destroy(set_target_note.NoteInstance);
            Debug.Log("miss timming");
            ResultData.hit_notes += 0.7f;
        }
        //完全にタイミングを外した場合
        else {
            Debug.Log("out");
            //break;
        }
    }

    /// <summary>
    /// 差分を取り絶対値を返す
    /// </summary>
    /// <param name="set_press_time">押下した時間</param>
    /// <param name="set_note_press_time">noteのtouch時間</param>
    /// <returns>Abs（押下ーnoteのtouch時間）</returns>
    public static int touchAbsDiffCal(int set_press_time, int set_note_press_time) {

        //差分を作成
        int diff = set_note_press_time - set_press_time;

        //絶対値をとる
        diff = Mathf.Abs(diff);

        return diff;
    }
}
