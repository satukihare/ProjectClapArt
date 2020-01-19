using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// チュートリアル用ゲームマネージャ
/// </summary>
public class tutorialGameMnger : GameSystem {

    [SerializeField]
    TutorialDialogManager dialog = null;

    //一時停止中に音楽の再生位置を決めるもの
    [SerializeField] float music_play_back_pos = 0.0f;

    /// <summary>
    /// 更新
    /// </summary>
    override protected void gameUpdate() {
        //音のタイミング
        music_time_num = (int)(music.time * 1000.0f);

        //待機状態
        if (game_state == GAME_MODE.GAME_WAIT) {
            // gameWait();
            game_state = GAME_MODE.GAME_DIALOG;
            gameDialog();
        }
        //notesスポーン
        else if (game_state == GAME_MODE.NOTE_SPAWN) {
            gameNoteSpawn();

        } //touch状態
        else if (game_state == GAME_MODE.NOTE_TOUCH) {
            gameNoteTouch();
        }
        //選択モード
        else if (game_state == GAME_MODE.GAME_CHOSE) {
            gameChose();
        }
        //ゲームでのシナリオ再生
        else if (game_state == GAME_MODE.GAME_DIALOG) {
            gameDialog();
        }
        else if (game_state == GAME_MODE.NON_NOTE){

        }
        //イレギュラー値
        else Debug.Log("Unknown State");
    }

    /// <summary>
    /// 読んでいるBarのNotesが全てクリックされているなら
    /// 次の遷移へ行く
    /// </summary>
    /// <param name="notes">NoteのList</param>
    /// <returns>全てクリックされているならTrue</returns>
    protected override bool checkNoteAllClick(List<Note> notes) {
        bool note_click_ch = true;
        //全てのノードがクリックされているか確認
        foreach (Note note in notes) {
            note_click_ch = note_click_ch & note.ClikFlg;
        }

        //一回でもくりっくされているなら選択へ遷移しない
        if (note_click_ch) {
            game_state = GAME_MODE.GAME_CHOSE;
        }

        return note_click_ch;
    }

    /// <summary>
    /// 選択状態
    /// </summary>
    private void gameChose() {
        gameStop();

        //左を選択
        if (track_pad_input.Tap)
        {
            //バーを先にすすめる
            //次に行く
            bar_counter++;

            //スポーン状態へ
            game_state = GAME_MODE.GAME_DIALOG;
        }
        //右を選択
        else if (track_pad_input.Flick)
        {
            //もう一回だどん
            List<Note> notes = bars[bar_counter].Notes;

            foreach (Note note in notes) {
                //クリックフラグを消す
                note.ClikFlg = false;
                Destroy(note.NoteInstance);
            }
            //スポーン状態へ
            game_state = GAME_MODE.NOTE_SPAWN;
            //音楽を指定の再生位置へ
            music_play_back_pos = bars[bar_counter].StartTime;
            gamePlay();
        }
    }

    /// <summary>
    /// ゲーム開始
    /// </summary>
    protected void gamePlay() {

        //音楽を再生
        music.time = this.music_play_back_pos ;
        music.Play();
        //Live2Dを再生
        this.li2dAnimatorPlay();
    }

    /// <summary>
    /// ゲームを停止
    /// </summary>
    protected void gameStop() {

        //Live2Dのアニメーションを停止
        this.live2dAnimatorStop();
        //再生位置を保存
       this.music_play_back_pos = music.time;

        //音楽を止める
        music.Stop();
    }

    /// <summary>
    /// Dialog関連
    /// </summary>
    protected override void gameDialog()
    {
        dialog.gameObject.SetActive(true);
    }

    /// <summary>
    /// 
    /// </summary>
    public void resume()
    {
        if (game_state == GAME_MODE.GAME_DIALOG)
        {
            game_state = GAME_MODE.NOTE_SPAWN;
            gamePlay();
        }
    }
}