using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMnger : GameSystem {

    /// <summary>
    /// 更新
    /// </summary>
    override protected void gameUpdate() {

        //音のタイミング
        music_time_num = (int)(music.time * 1000.0f);

        //待機状態
        if (game_state ==  GAME_MODE.GAME_WAIT) {
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
        //次のBarへ行く際の準備
        else if (game_state == GAME_MODE.NEXT_BAR) {
            gameNextBar();
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
            game_state = GAME_MODE.NEXT_BAR;
        }
        return note_click_ch;
    }

    /// <summary>
    /// 次のBarへ移行する準備
    /// </summary>
    void gameNextBar() {
        bar_counter++;
        game_state = GAME_MODE.NOTE_SPAWN;
    }
}
