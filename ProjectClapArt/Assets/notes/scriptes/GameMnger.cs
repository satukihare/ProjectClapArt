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

    //現在読んでいるnotesのバーの番号
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

    //
    [SerializeField] GameObject good_obj = null;
    [SerializeField] GameObject bad_obj = null;

    [SerializeField] SpriteRenderer debugTap;
    [SerializeField] bool Flag;

    [SerializeField] int debugPCM;
    [SerializeField] float debugTIME;

    [SerializeField] int ElapsedTime = 0;
    [SerializeField] float ElapsedTime_org = 0;
    [SerializeField] int musictime;

    public float WholeNote {
        get { return whole_note; }
    }

    /// <summary>
    /// 初期化
    /// </summary>
    void Start() {
        Application.targetFrameRate = 60;

        //音符の長さを計測
        whole_note = (int)(60.0f / BPM * NOTE * 1000.0f);

        music = this.GetComponent<AudioSource>();

      //  music.velocityUpdateMode = AudioVelocityUpdateMode.Fixed;

        Bar test_bar = new Bar();

        List<Note> notes = new List<Note>(4);

        notes.Add(new Note(new Vector2(-1,-1),2300,4290,Note.NOTE_TYPE.FLICK ));
        notes.Add(new Note(new Vector2(1,-1) ,2800,4790,Note.NOTE_TYPE.FLICK ));
        notes.Add(new Note(new Vector2(-1,1) ,3300,5290,Note.NOTE_TYPE.FLICK ));
        notes.Add(new Note(new Vector2(1,1)  ,3800,5790,Note.NOTE_TYPE.FLICK ));
        
        test_bar.Notes = notes;

        List<Bar> test_bars = new List<Bar>();
        test_bars.Add(test_bar);

        bars = test_bars;

        
    }

    /// <summary>
    /// 更新
    /// </summary>
    void Update() {

        //音のタイミング
        this.music_time_num = (int)(music.time * 1000.0f);
        //不要な桁の切り捨て

        //デバッグ用
        debugPCM = music.timeSamples;
        debugTIME = debugPCM / (Time.time - start_game_time);

        //一秒あたりのPMC
        if (debugPCM != 0)
            ElapsedTime = (int)(debugPCM / 44.1f) ;
        
        ElapsedTime_org = (Time.time - start_game_time);
        musictime = (int)(music.time * 1000.0f);

        //music_time_num /= 20;
        //music_time_num *= 20;

        music_time_num = ElapsedTime;


        music_time_num /= 100;
        music_time_num *= 100;

       
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

        //Debug.Log("music.time : " + music.time.ToString());
        //Debug.Log("music_time_num : " + music_time_num.ToString());
        //Debug.Log("Time.time : " + Time.time.ToString());

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
    /// 判定検出（未使用）
    /// </summary>
    void InputTimmingJudge() {

        //予め多めの誤差にしておく
        int more_dff = 2 * 2;

        //int juge_time = (int)(music.timeSamples * 1000.0f);

        int juge_time = ElapsedTime;
        
        //不要な桁の切り捨て
        juge_time /= 10;
        juge_time *= 10;

        int diff = juge_time % whole_note;

        diff = Mathf.Abs(diff);
        Debug.Log(diff);

        //判定が誤差範囲内なら
        //if ( diff <= 100) {
        //    GameObject good_ = new GameObject
        //    //good_obj.SetActive(true);
        //}
        //else if (diff > 100 && diff <= 300) {
        //    //bad_obj.SetActive(true);
        //}
    }

    /// <summary>
    /// 待機状態
    /// </summary>
    void gameWait() {
        //ゲーム開始
        if (track_pad_input.Tap ) {
            gameStart();
        }

    }

    /// <summary>
    /// スポーン状態
    /// </summary>
    void gameNoteSpawn() {

        //スポーンする小節
        List<Note> notes = bars[bar_counter].Notes;

 
        //スポーンする
        foreach (Note note in notes) {
            //対象のnoteまでスキップ
            if (notes[note_counter] != note)
                continue;

            //スポーンするnotesがあればだす
            if (music_time_num  == note.SpawnTime ) {
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

        //スペースが押されたのであれば判定する
        //if (!Input.GetKey(KeyCode.Space))
        //    return;


        //  Debug.Log("Flick : " + track_pad_input.Flick.ToString());

        //トラックパッドを使用したとき
       // if (!track_pad_input.Tap)
         Flag = track_pad_input.Tap;
      //  Flag = Input.GetKeyDown(KeyCode.Space);
            //  return;
       // Debug.Log("Flick : " + track_pad_input.Flick.ToString());

        //押した時間
        int press_time = music_time_num;

        //スポーンする小節リスト
        List<Note> notes = bars[bar_counter - 1].Notes;

        if (debugTap.color != Color.white)
        {
            debugTap.color = Color.white;
        }
        //判定
        foreach (Note note in notes) {
            //クリックされていたら判定しない
            if (note.ClikFlg)
                continue;

            //差分を作成
            int diff = note.PressTime - press_time;

            //絶対値
            diff = Mathf.Abs(diff);

            //誤差から判定する
            //ベストタイミング
            if (diff < good_diff_time_num) {
                //クリックフラグを発火
                //デバッグ
                debugTap.color = Color.green;

                if (!Flag)
                    continue;

                note.ClikFlg = true;
                Destroy(note.NoteInstance);
                Debug.Log("good");

                break;

            }
            //ちょっと惜しいとき
            else if (diff < this.more_diff_num){
                //クリックフラグを発火
                debugTap.color = Color.red;
                if (!Flag)
                    continue;

                note.ClikFlg = true;
                Destroy(note.NoteInstance);
                Debug.Log("miss");
                break;
            }


            //完全にタイミングを外した場合
            //else if(more_diff_num < diff ) {
            //    Debug.Log("out");
            //    Destroy(note.NoteInstance);
            //    break;
            //}
        }

        
    }
}
