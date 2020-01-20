﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
        //ゲーム中
        GAME_NOW,
        //選択モード
        GAME_CHOSE,
        //ゲーム終了
        GAME_END,
        //シナリオ再生
        GAME_DIALOG,
        //ノーツ無しプレイ
        NON_NOTE,
        //未定義
        UNKNOWN = -1,
    }

    /// <summary>
    /// キャラ選択
    /// </summary>
    protected enum CHAR_MDOEL_ENUM{
        NAGI = 0,
        KAI,
        UNKNOWN = -1,
    }

    [SerializeField] protected float BPM = 0;

    [SerializeField] protected float NOTE = 0;

    //音符の長さ
    [SerializeField] protected int whole_note;

    //ゲームの状態
    [SerializeField] protected GAME_MODE game_state = GAME_MODE.GAME_WAIT;

    //ゲーム開始時の時間(ms)
    protected float start_game_time = 0;
    //ゲーム中のフラグ
    protected bool game_flg = false;

    //オーディオマネージャ
    protected AudioSource music = null;

    //スポーンさせるnotesオブジェクト
    [SerializeField] protected GameObject spawn_note_object = null;

    //barを入れるリスト
    protected List<Bar> bars = null;

    //ポップしているノーツ
    [SerializeField] List<Note> popd_notes = new List<Note>(32);

    //現在読んでいるBarの番号
    [SerializeField]protected int bar_counter = 0;

    //現在読んでいるnoteの番号
    protected int note_counter = 0;

    //音楽の再生位置を1000倍したものが基準
    [SerializeField]protected int music_time_num = 0;

    //ポップするときに許容できる誤差
    [SerializeField] protected int more_pop_dif_num = 0;

    //ノーツを消す時間を調整する
    [SerializeField] protected int note_disappear_time = 1000;

    //押した時間を調整する
    [SerializeField] protected int note_jude_custom_num = 0;

    //タップで許容できる誤差
    [SerializeField] protected int more_diff_num = 150;

    //goodタイミング
    [SerializeField] protected int good_diff_time_num = 80;

    //入力Mnger
    [SerializeField] protected InputManager track_pad_input = null;

    //サイリウムMnger
    [SerializeField] protected PenlightManager penlight_mng = null;

    //切り捨てる数値量
    [SerializeField] protected int round_digits = 20;

    //JSON形式の譜面データを読み込む
    protected readWriteJsonFile read_write_json_file = null;

    //スコアデータ
    protected int score = 0;

    //スコア用イメージ？
    [SerializeField] protected Image score_image = null;

    //読み込むJSONファイル名
    [SerializeField] public string load_json_note_file = "";

    //Live2Dのモデルデータ
    [SerializeField] protected GameObject[] live_2d_models = new GameObject[2];

    //使用している使っているキャラのEnum
    [SerializeField] protected CHAR_MDOEL_ENUM use_model_obj = CHAR_MDOEL_ENUM.UNKNOWN;

    //Particleのオブジェクト
    [SerializeField] protected GameObject free_particle;
    [SerializeField] protected GameObject good_effect;
    [SerializeField] protected GameObject perfect_effect;

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

        if (penlight_mng)
            penlight_mng.SetBPM(BPM);

        music = this.GetComponent<AudioSource>();

        read_write_json_file = GetComponent<readWriteJsonFile>();
        //譜面の読み込み
        if (read_write_json_file == null) Debug.Log("readWriteJsonFile nullptr !!");

        //Live2Dでキャラ選択
        live2dActive(use_model_obj);
        //ファイルのロード
        bars = read_write_json_file.readNotesFileDate(load_json_note_file);
        ResultData.total_notes = 0;
        ResultData.hit_notes = 0;
        ResultData.bonus_score = 0;
        foreach (Bar bar in bars)
        {
            ResultData.total_notes += bar.Notes.Count;
        }
    }

    /// <summary>
    /// 更新
    /// </summary>
    protected void Update() {

        //ゲームモードのときのみ
        if (game_state == GAME_MODE.GAME_NOW) {
            //ノーツSpawn
            gameNoteSpawn();

            //ノーツを削除
            gameDestroyNote();

            //タッチ
            gameNoteTouch();
        }
        //更新
        gameUpdate();

        //フリーアピール
        freeTouchDetection();

        //ゲーム終了か確認
        notesEndCheck(bars, bar_counter+1);

        //ゲーム終了時の場合
        if(game_state == GAME_MODE.GAME_END) {
            gameEnd();
        }
    }

    /// <summary>
    /// 更新
    /// 抽象メソッド
    /// </summary>
    protected virtual void gameUpdate() {}

    /// <summary>
    /// 待機状態
    /// </summary>
    virtual protected void gameWait() {
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

        this.li2dAnimatorPlay();

        //ゲーム状態へ
        game_state = GAME_MODE.GAME_NOW;
    }

    /// <summary>
    /// スポーン状態
    /// </summary>
    protected void gameNoteSpawn() {

        int msc_time_rud_dgts = music_time_num;

        Bar now_bar = searchNowTimeBar();
        if (now_bar == null) return;

        //スポーンする小節
        List<Note> notes = now_bar.Notes;
        if (notes == null) return;

        //スポーンする
        foreach (Note note in notes) {
            //対象のnoteまでスキップ
            //if (notes[note_counter] != note)
            //    continue;
            if (note.PopFlg) continue;

            //スポーンするnotesがあればだす
            if (( note.SpawnTime + now_bar.StartTime - this.more_pop_dif_num <= msc_time_rud_dgts) &&
                ( note.SpawnTime + now_bar.StartTime + this.more_pop_dif_num >= msc_time_rud_dgts)) {

                //オブジェクトを生成している
                GameObject pop_note = Instantiate(spawn_note_object, note.Pos, Quaternion.identity);

                //クリックするタイミングを入れている
                note.PressMusicTime = note.PressTime + now_bar.StartTime;
                note.PopFlg = true;

                //ポップしているノーツに追加
                popd_notes.Add(note);

                //notesにinstanceをセット
                note.NoteInstance = pop_note;
                //カウント
                note_counter++;
                //最後のnotesか判定
                if (note == notes[notes.Count - 1]) {

                    //Counterをゼロに
                    note_counter = 0;
                    //次のBarへ
                    bar_counter++;
                }
                break;
            }
        }
    }

    /// <summary>
    /// その時間のBarを探す
    /// </summary>
    /// <returns></returns>
    protected Bar searchNowTimeBar() {

        //今のBarを探す
        foreach(Bar bar in bars) {
            if(bar.StartTime < this.music_time_num && bar.StartTime + bar.Lingth > music_time_num) {
                return bar;
            }
        }
        return null;
    }

    /// <summary>
    /// 時間が過ぎたノーツを消す
    /// </summary>
    protected void gameDestroyNote() {

        for(int cnt = 0;cnt < popd_notes.Count; cnt++) {
            Note note = popd_notes[cnt];

            if (note == null) continue;

            //押す時間が過ぎていたら消す
            if (note.PressMusicTime + note_disappear_time < music_time_num) {
                missNoteDestory(note);
            }

        }
    }

    /// <summary>
    /// touch状態
    /// </summary>
    protected void gameNoteTouch() {

        //トラックパッドがtouchされればTrue
        bool flick_flg = track_pad_input.Tap | track_pad_input.Flick | track_pad_input.FlickStart | track_pad_input.FlickEnd;

        //トラックパッドを使用したときのみ処理
        if (!flick_flg) return;

        //スポーンした小節リスト
        List<Note> notes = popd_notes;

        //何が押されたのかを検出する
        Note.NOTE_TYPE input_type = this.getInputType();
        //Unknownなら何もしない
        if (input_type == Note.NOTE_TYPE.UNKNOWN) return;

        //押した時間 = 押した時間 + 調整用時間
        int press_time = music_time_num + note_jude_custom_num;

        //判定
        foreach (Note note in notes) {
            //クリックされていたNoteは判定しない
            //inputのタイプが一致しなければ飛ばす
            if ((note.ClikFlg) ||(note.Type != input_type) )
                continue;

            //差分を取る
            int diff = touchAbsDiffCal(press_time, note.PressMusicTime);

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
    /// 差分を基に判定を行う
    /// </summary>
    /// <param name="set_diff">誤差</param>
    /// <param name="set_target_note">判定する対象のNote</param>
    protected void judgeTouchTimming(int set_diff, Note set_target_note) {

        //ベストタイミング
        if (set_diff < good_diff_time_num) {
            //フリックフラグを発火
            set_target_note.ClikFlg = true;
            //Destroy(set_target_note.NoteInstance);
            Animator anim = set_target_note.NoteInstance.GetComponent<Animator>();
            anim.SetTrigger("Hit");
            Debug.Log("good timming");
            ResultData.hit_notes += 1;
            ResultData.voltage_score += 5;
            if (ResultData.voltage_score > ResultData.voltage_max)
                ResultData.voltage_score = ResultData.voltage_max;

            score_image.fillAmount = ResultData.score_rate;
            Instantiate(perfect_effect, set_target_note.Pos, Quaternion.identity);
            //score_image.fillAmount = (float)(score) / 16;
            //
        }
        //ちょっと惜しいとき
        else if (set_diff < more_diff_num) {
            //フリックフラグを発火
            set_target_note.ClikFlg = true;
            //Destroy(set_target_note.NoteInstance);
            Animator anim = set_target_note.NoteInstance.GetComponent<Animator>();
            anim.SetTrigger("Hit");
            Debug.Log("miss timming");
            ResultData.hit_notes += 0.7f;
            ResultData.voltage_score += 3;
            if (ResultData.voltage_score > ResultData.voltage_max)
                ResultData.voltage_score = ResultData.voltage_max;

            score_image.fillAmount = ResultData.score_rate;
            Instantiate(good_effect, set_target_note.Pos, Quaternion.identity);
        }
        //完全にタイミングを外した場合
        else {
            Debug.Log("out");
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

    /// <summary>
    /// 何を押したのか検出する
    /// </summary>
    /// <returns></returns>
    protected Note.NOTE_TYPE getInputType() {
        Note.NOTE_TYPE input_type = Note.NOTE_TYPE.UNKNOWN;
        if (track_pad_input.Tap) input_type = Note.NOTE_TYPE.TOUCH;
        else if (track_pad_input.Flick) input_type = Note.NOTE_TYPE.FLICK;

        return input_type;
    }

    /// <summary>
    /// ゲームの終了確認
    /// 譜面が読み終わったか確認する
    /// </summary>
    /// <param name="bars">Barクラスのリスト</param>
    /// <param name="bar_counter">読んでいるBar</param>
    /// <returns>終わったかを返す(Trueなら終わっている)</returns>
    protected  virtual void notesEndCheck(List<Bar> bars , int bar_counter) {

        //bar_counterがBarの個数を同一以上であれば終わり
        //if (bars.Count <= bar_counter)
        if(bars[ bars.Count - 1 ].StartTime + bars[bars.Count -1 ].Lingth < music_time_num + 1000)
            game_state = GAME_MODE.GAME_END;
    }

    /// <summary>
    /// ゲーム終了時演出
    /// </summary>
    protected virtual void gameEnd() {

        if (Input.GetKey(KeyCode.P))
            Transition.instance.ChangeScene("DialogScene");
    }

    /// <summary>
    /// ゲーム内でのDialogモード
    /// </summary>
    protected virtual void gameDialog() {}

    /// <summary>
    /// キャラ選択でアクティブにするキャラを選択する
    /// </summary>
    /// <param name="set_active_char">アクティブにしたいキャラ</param>
    protected void live2dActive(CHAR_MDOEL_ENUM set_active_char) {
        if(live_2d_models[(int)set_active_char])
            live_2d_models[(int)set_active_char].SetActive(true);
    }

    /// <summary>
    /// Live2Dのアニメーションを再生
    /// </summary>
    protected void li2dAnimatorPlay() {
        if (!live_2d_models[(int)use_model_obj])
            return;
            Animator anim = this.live_2d_models[(int)use_model_obj].GetComponent<Animator>();
        anim.SetBool("Pause", false);
    }

    /// <summary>
    /// live2dのアニメーションを停止
    /// </summary>
    protected void live2dAnimatorStop()
    {
        if (!live_2d_models[(int)use_model_obj])
            return;
        Animator anim = this.live_2d_models[(int)use_model_obj].GetComponent<Animator>();
        anim.SetBool("Pause", true);
    }

    /// <summary>
    /// フリーtouch検出
    /// </summary>
    protected void freeTouchDetection() {
        //ゲームが開始されているか
        //if (!game_flg) return;

        //flickかタップされれば実行される
        bool input_flg = track_pad_input.Tap;// | track_pad_input.Flick;
        if (!input_flg) return;

        //加点システムはここに
        if (ResultData.bonus_score < ResultData.bonus_max)
        ResultData.bonus_score++;

        //パーティクルとかの生成もここで
        //Vector2 pos = Camera.main.ScreenToWorldPoint((Vector2)Input.mousePosition);
        Vector2 pos = new Vector2(Random.Range(-2.0f, 2.0f), Random.Range(-2.5f,4.7f));
        Instantiate(free_particle, pos, Quaternion.identity);
    }

    /// <summary>
    /// ミスしたノーツを消してボルテージも減らす？
    /// </summary>
    /// <param name="note"></param>
    protected void missNoteDestory( Note note ) {
        //nullチェック
        if (note == null)
            return;
        try {
            Animator anim = note.NoteInstance.GetComponent<Animator>();
            anim.SetTrigger("Despawn");
            note.ClikFlg = true;
            //安全のためにNullを入れる
            note.NoteInstance = null;
            //ポップしたnoteのリストからけす
            popd_notes.Remove(note);
        } catch { Debug.LogWarning("MissingReferenceException：タイプ 'GameObject'のオブジェクトは破棄されましたが、まだアクセスしようとしています。"); }

        ResultData.voltage_score -= 5;
        if (ResultData.voltage_score < 0)
            ResultData.voltage_score = 0;
    }
}
