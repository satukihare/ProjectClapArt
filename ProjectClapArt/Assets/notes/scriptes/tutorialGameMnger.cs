using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// チュートリアル用ゲームマネージャ
/// </summary>
public class tutorialGameMnger : MonoBehaviour {

    //ゲームの状態
    GameMngerLib.GAME_MODE game_state = GameMngerLib.GAME_MODE.GAME_WAIT;

    //ゲームの開始時間(ms)
    float start_game_time = 0;
    //ゲーム中のフラグ
    bool game_flg = false;

    //オーディオマネージャ
    [SerializeField] AudioSource music = null;

    //ポップするときのSE
    [SerializeField] AudioClip pop_se = null;

    //スポーンさせるnotesオブジェクト
    [SerializeField] GameObject spawn_note_object = null;

    //notesを入れるリスト
    List<Bar> bars = null;

    //現在読んでいるBarの番号
    int bar_counter = 0;

    //現在読んでいるnoteの番号
    int note_counter = 0;

    //音楽の再生位置を1000倍したものが基準
    int music_time_num = 0;

    //タップで許容できる誤差
    [SerializeField] int more_diff_num = 100;

    //goodタイミング
    [SerializeField] int good_diff_time_num = 10;

    //入力マネージャ
    [SerializeField] InputManager track_pad_input_mng = null;

    //切り捨てる数値量
    [SerializeField] int round_digits = 100;

    //JSON読み込み用のクラス
    [SerializeField] readWriteJsonFile read_write_json = null;

    /// <summary>
    /// 初期化
    /// </summary>
    void Start() {
        music = this.GetComponent<AudioSource>();

        //read_write_jsonがあるか確認
        if (read_write_json == null) Debug.Log("readWriteJsonFile nullptr !!");
        //BarをJSONから読み込む
        bars = read_write_json.readNotesFileDate("test.json");

    }
    /// <summary>
    /// 更新
    /// </summary>
    void Update() {
        //音のタイミング
        this.music_time_num = (int)(music.time * 1000.0f);

        //待機状態
        if (game_state == GameMngerLib.GAME_MODE.GAME_WAIT) {
            gameWait();
        }
        //notesスポーン
        else if (game_state == GameMngerLib.GAME_MODE.NOTE_SPAWN) {
            gameNoteSpawn();

        } //touch状態
        else if (game_state == GameMngerLib.GAME_MODE.NOTE_TOUCH) {
            gameNoteTouch();
        }
        //選択モード
        else if (game_state == GameMngerLib.GAME_MODE.GAME_CHOSE) {
            gameChose();
        }
          //イレギュラー値
          else
            Debug.Log("UNKNOWN");
    }

    /// <summary>
    /// 待機状態
    /// </summary>
    void gameWait() {
        if (track_pad_input_mng.Tap) {
            gameStart();
        }
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
    /// スポーン状態
    /// </summary>
    void gameNoteSpawn() {

        //描画の不要な桁の切り捨て
        int msc_time_rud_dgts = 0;
        msc_time_rud_dgts = music_time_num / round_digits;
        msc_time_rud_dgts = msc_time_rud_dgts * round_digits;

        //小説のかきこみタイミングまでスキップ
        if (bars[bar_counter].StartTime > msc_time_rud_dgts)
            return;
        //スポーンする小節
        List<Note> notes = bars[bar_counter].Notes;

        //スポーンするNoteを探す
        foreach (Note note in notes) {
            //スポーン対象までスキップ
            if (notes[note_counter] != note)
                continue;

            //スポーンするnotesがあれば出す
            if (msc_time_rud_dgts == note.SpawnTime) {
                GameObject pop_note = Instantiate(spawn_note_object, note.Pos, Quaternion.identity);
                //notesにinstanceをセット
                note.NoteInstance = pop_note;
                note_counter++;
                //最後のnotesか判定
                if (note == notes[notes.Count - 1]) {
                    //bar_counter++;
                    //タッチモードへ
                    game_state = GameMngerLib.GAME_MODE.NOTE_TOUCH;
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
        //スポーンするリスト
        List<Note> notes = bars[(bar_counter <= 0 ? 0 : bar_counter - 1)].Notes;

        //notesをチェックする
        notesTimingCheck(notes);

        //全てのノードがクリックされているなら選択へ遷移する
        if (checkNoteAllClick( notes))
            return;

        //トラックパッドがtouchされればTrue
        bool flick_flg = track_pad_input_mng.Tap | track_pad_input_mng.Flick | track_pad_input_mng.FlickStart | track_pad_input_mng.FlickEnd;

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
            game_state = GameMngerLib.GAME_MODE.GAME_CHOSE;
        }

        return note_click_ch;
    }

    /// <summary>
    /// notesのタイミングをチェックしタイミングが通り過ぎたものにtrueを入れる
    /// </summary>
    /// <param name="notes"></param>
    private void notesTimingCheck(List<Note> notes) {
        foreach(Note note in notes) {
            if (note.ClikFlg)
                continue;

            //タイミングが過ぎているのでTrueをいれる
            if (note.PressTime + 1000 < music_time_num) {
                note.ClikFlg = true;
                //
                //今はタイミングが間違ってもnotesを消している
                //
                Destroy(note.NoteInstance);
            }
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
        else {
            Debug.Log("out");
            //break;
        }
    }

    /// <summary>
    /// 選択状態
    /// </summary>
    private void gameChose() {
        //再生位置を保存
        float music_playback_pos = music.time;

        //音楽を止める
        music.Stop();

        //左を選択
        if (Input.GetKey(KeyCode.A)) {
            //バーを先にすすめる
            //次に行く
            bar_counter++;

            //スポーン状態へ
            game_state = GameMngerLib.GAME_MODE.NOTE_SPAWN;
            //音楽を再生
            music.time = music_playback_pos;
            music.Play();
        }
        //右を選択
        else if (Input.GetKey(KeyCode.D)) {
            //もう一回だどん
            List<Note> notes = bars[bar_counter].Notes;

            foreach(Note note in notes) {
                //クリックフラグを消す
                note.ClikFlg = false;
                Destroy(note.NoteInstance);
            }
            //スポーン状態へ
            game_state = GameMngerLib.GAME_MODE.NOTE_SPAWN;
            //音楽を指定の再生位置へ
            music.time = bars[bar_counter].StartTime;
            music.Play();
        }
    }
}