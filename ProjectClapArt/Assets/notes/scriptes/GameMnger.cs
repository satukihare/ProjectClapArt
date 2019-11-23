using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMnger : MonoBehaviour {

    //ゲームの状態
    GameMngerLib.GAME_MODE game_state = GameMngerLib.GAME_MODE.GAME_WAIT;

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
        if (game_state ==  GameMngerLib.GAME_MODE.GAME_WAIT) {
            gameWait();
        }
        //スポーン状態
        else if (game_state == GameMngerLib.GAME_MODE.NOTE_SPAWN) {
            gameNoteSpawn();
        }
        //touch状態
        else if (game_state == GameMngerLib.GAME_MODE.NOTE_TOUCH) {
            gameNoteTouch();
        }
        //次のBarへ行く際の準備
        else if(game_state == GameMngerLib.GAME_MODE.NEXT_BAR) {
            gameNextBar();
        }
        //イレギュラー値
        else Debug.Log("UNKNOWN");
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
        game_state = GameMngerLib.GAME_MODE.NOTE_SPAWN;
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
                    //タッチモードへ
                    game_state = GameMngerLib.GAME_MODE.NOTE_TOUCH;

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
        //スポーンした小節リスト
        List<Note> notes = bars[(bar_counter <= 0 ? 0 : bar_counter )].Notes;

        //notesをチェックする
        notesTimingCheck(notes);

        //全てのノードがクリックされているなら選択へ遷移する
        if ( checkNoteAllClick(notes))
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
            int diff = GameMngerLib.touchAbsDiffCal(press_time, note.PressTime);

            //誤差から判定する
            judgeTouchTimming(diff, note);
        }
    }

    /// <summary>
    /// 読んでいるBarのNotesが全てクリックされているなら
    /// 次の遷移へ行く
    /// </summary>
    /// <param name="notes">NoteのList</param>
    /// <returns>全てクリックされているならTrue</returns>
    private bool checkNoteAllClick(List<Note> notes) {
        bool note_click_ch = true;
        //全てのノードがクリックされているか確認
        foreach (Note note in notes) {
            note_click_ch = note_click_ch & note.ClikFlg;
        }

        //一回でもくりっくされているなら選択へ遷移しない
        if (note_click_ch) {
            game_state = GameMngerLib.GAME_MODE.NEXT_BAR;
        }
        return note_click_ch;
    }

    /// <summary>
    /// notesのタイミングをチェックしタイミングが通り過ぎたものにtrueを入れる
    /// </summary>
    /// <param name="notes">NoteのList</param>
    private void notesTimingCheck(List<Note> notes) {
        foreach (Note note in notes) {
            if (note.ClikFlg)
                continue;

            //タイミングが過ぎているのでTrueをいれる
            if (note.PressTime + 1000 < music_time_num) {
                note.ClikFlg = true;
                //
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

    /// <summary>
    /// 次のBarへ移行する準備
    /// </summary>
    void gameNextBar() {
        bar_counter++;
        game_state = GameMngerLib.GAME_MODE.NOTE_SPAWN;
    }
}
