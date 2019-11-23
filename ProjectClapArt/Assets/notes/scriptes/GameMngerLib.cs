using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// GameMnger用のstaticのLibrary
/// </summary>
public class GameMngerLib {

    //ゲームモード
    public enum GAME_MODE {
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
