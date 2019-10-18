using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class reismMng : MonoBehaviour {

    //targetのタイミング
    public List< notesDateClass> target_date = new List< notesDateClass>();

    //notesの検出倍率
    [SerializeField] int detection_magnification_num = 10;

    //notesの検出倍率を適応した外部向けのゲーム時間
    int game_in_time = 0;

    //game内の時間を渡す
    public int GameInTime {
        get { return game_in_time; }
    }

    /// <summary>
    /// 初期化
    /// </summary>
    void Start() {
        //デフォルトノーズ
        target_date.Add(new notesDateClass(10, new Vector2(-2, 0), notesDateClass.NOTES_TYPE.FLICK, 30));
        target_date.Add(new notesDateClass(15, new Vector2(2, 0), notesDateClass.NOTES_TYPE.FLICK, 35));
        target_date.Add(new notesDateClass(20, new Vector2(-2, -2), notesDateClass.NOTES_TYPE.FLICK, 40));
        target_date.Add(new notesDateClass(25, new Vector2(2, -2), notesDateClass.NOTES_TYPE.FLICK, 45));
    }

    /// <summary>
    /// 更新
    /// </summary>
    void Update() {
        //ゲーム内時間を書き込む
        game_in_time = (int)(Time.time * this.detection_magnification_num);

        Debug.Log(game_in_time.ToString());
    }
}
