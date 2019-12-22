using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMnger : GameSystem {

    /// <summary>
    /// 更新
    /// </summary>
    /// 

    [SerializeField]
    Animator Animation;

    [SerializeField]
    Image Fade;

    float Fade_alpha = 128;

    protected enum LIGHT_MODE
    {
        NONE,
        LIGHT_UP,
        LIGHT_DOWN
    }

    LIGHT_MODE light_state;

    override protected void gameUpdate() {

        //音のタイミング
        music_time_num = (int)(music.time * 1000.0f);

        Light();

        //待機状態
        if (game_state ==  GAME_MODE.GAME_WAIT) {
            GameStand_By();
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
    /// GameWaitの代わり
    /// Gameが始まるまでのやつ
    /// </summary>
    /// <param name="notes">NoteのList</param>
    /// <returns>全てクリックされているならTrue</returns>
    void GameStand_By()
    {

        gameStart();

        Invoke("StageLightUp",4f);
        Invoke("DanceStart", 8f);

    }

    /// <summary>
    /// Game終了処理
    /// Gameが始まるまでのやつ
    /// </summary>
    /// <param name="notes">NoteのList</param>
    /// <returns>全てクリックされているならTrue</returns>
    protected override void gameEnd()
    {
        light_state = LIGHT_MODE.LIGHT_DOWN;
        Animation.SetBool("MusicEnd", true);

        //ここにInvokeでTransition呼び出し


    }

    /// <summary>
    /// GameWaitの代わり
    /// Gameが始まるまでのやつ
    /// </summary>
    /// <param name="notes">NoteのList</param>
    /// <returns>全てクリックされているならTrue</returns>
    void StageLightUp()
    {
        light_state = LIGHT_MODE.LIGHT_UP;
    }

    /// <summary>
    /// 照明的なヤツ
    /// Gameが始まるまでのやつ
    /// </summary>
    /// <param name="notes">NoteのList</param>
    /// <returns>全てクリックされているならTrue</returns>
    void Light()
    {
        if (light_state == LIGHT_MODE.LIGHT_UP)
        {
            Fade_alpha -= .5f;
            if (Fade_alpha <= 0)
            {
                Fade_alpha = 0f;
                light_state = LIGHT_MODE.NONE;
            }
           Fade.color = new Color(0f,0f,0f, Fade_alpha/ 255.0f); 

        }
        else if (light_state == LIGHT_MODE.LIGHT_DOWN)
        {
            Fade_alpha += .75f;
            if (Fade_alpha >= 128)
            {
                Fade_alpha = 128f;
                light_state = LIGHT_MODE.NONE;

                Transition.instance.ChangeScene("ResultScene");
            }
            Fade.color = new Color(0f, 0f, 0f, Fade_alpha / 255.0f);
        }
       

    }


    /// <summary>
    /// GameWaitの代わり
    /// Gameが始まるまでのやつ
    /// </summary>
    /// <param name="notes">NoteのList</param>
    /// <returns>全てクリックされているならTrue</returns>
    void DanceStart()
    {
        Animation.SetBool("DanceStart", true);
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
