using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// GameMnger用のstaticのLibrary
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
