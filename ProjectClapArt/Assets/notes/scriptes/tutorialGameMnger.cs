using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// チュートリアル用ゲームマネージャ
/// </summary>
public class tutorialGameMnger : MonoBehaviour {

    //ゲームモード
    enum GAME_MODE {
        //待機状態
        GAME_WAIT = 0,
        //notes出現状態
        NOTE_SPAWN,
        //notesをtouch
        NOTE_TOUCH,
        //未定義
        UNKNOWN = -1,
    }

    //ゲームの状態
    GAME_MODE game_state = GAME_MODE.UNKNOWN;

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
    [SerializeField] int round_digits = 20;

    //JSON読み込み用のクラス
    [SerializeField] readWriteJsonFile read_write_json = null;

    /// <summary>
    /// 初期化
    /// </summary>
    void Start() {

    }
    /// <summary>
    /// 更新
    /// </summary>
    void Update() {

    }
}
