using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class notesDateClass {

    //notesの種類定義
    public enum NOTES_TYPE {
        FLICK = 0,
        TAP,
        UNKNOWN = -1,
    }

    //出現タイミング
    private int trgt_pop_timming = 0;

    //notesの出現座標
    private Vector2 position = new Vector2(0, 0);

    //notesの判定
    private NOTES_TYPE notes_type = NOTES_TYPE.UNKNOWN;

    //notesの押す？タイミング
    private int trgt_nts_clk_timing = 0;

    //Target生成フラグ
    bool gene_flg = false;

    //popするTargetのobj
    private GameObject pop_obj = null;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public notesDateClass() { }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="clk_timing"></param>
    public notesDateClass(int clk_timing) {
        trgt_nts_clk_timing = clk_timing;
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="set_trgt_pop_timming"></param>
    /// <param name="set_position"></param>
    /// <param name="set_notes_type"></param>
    /// <param name="set_trgt_nts_clk_timing"></param>
    public notesDateClass(
        int set_trgt_pop_timming ,
        Vector2 set_position ,
        NOTES_TYPE set_notes_type ,
        int set_trgt_nts_clk_timing) {
        trgt_pop_timming = set_trgt_pop_timming;
        position = set_position;
        notes_type = set_notes_type;
        trgt_nts_clk_timing = set_trgt_nts_clk_timing;
    }

    /// <summary>
    /// 初期化
    /// </summary>
    void Start() {}

    /// <summary>
    /// 更新
    /// </summary>
    void Update() {}

    /// <summary>
    /// trgt_pop_timming アクセサ
    /// </summary>
    /// <param name="set_timming_num"></param>
    public void setTrgtPopTimming(int set_timming_num) {
        this.trgt_pop_timming = set_timming_num;
    }

    /// <summary>
    /// position アクセサ
    /// </summary>
    /// <param name="set_position"></param>
    public void setPosition(Vector2 set_position) {
        this.position = set_position;
    }
    /// <summary>
    /// notes_typeアクセサ
    /// </summary>
    /// <param name="set_notes_type"></param>
    public void setNotesType(NOTES_TYPE set_notes_type) {
        this.notes_type = set_notes_type;
    }

    /// <summary>
    /// trgt_nts_clk_timing アクセサ
    /// </summary>
    /// <param name="set_trgt_notsClkTiming"></param>
    public void setTrgtNotsClkTiming(int set_trgt_notsClkTiming) {
        this.trgt_nts_clk_timing = set_trgt_notsClkTiming;
    }

    /// <summary>
    /// trgt_pop_timming getアクセサ
    /// </summary>
    /// <returns></returns>
    public int getTrgtPopTimming() {
        return trgt_pop_timming;
    }

    /// <summary>
    /// position getアクセサ
    /// </summary>
    /// <returns></returns>
    public Vector2 getPosition() {
        return position;
    }

    /// <summary>
    /// notes_typeアクセサ
    /// </summary>
    /// <returns></returns>
    public NOTES_TYPE getNotesType() {
        return notes_type;
    }

    /// <summary>
    /// trgt_nts_clk_timing アクセサ
    /// </summary>
    /// <returns></returns>
    public int getTrgtNotsClkTiming() {
        return trgt_nts_clk_timing;
    }

    /// <summary>
    /// gene_flgをtrueにする
    /// </summary>
    public void tragtGeneFlg() {
        this.gene_flg = true;
    }

    /// <summary>
    /// gene_flg アクセサ
    /// </summary>
    /// <returns></returns>
    public bool getGeneFlg() {
        return gene_flg;
    }

    /// <summary>
    /// instanceをセット
    /// </summary>
    /// <param name="set_pop_obj"></param>
    public void setTrgtInstance( GameObject set_pop_obj ) {
        pop_obj = set_pop_obj;
    }

    /// <summary>
    /// instanceをゲット
    /// </summary>
    /// <returns></returns>
    public GameObject getTrgtInstance() {
        return pop_obj;
    }
}
