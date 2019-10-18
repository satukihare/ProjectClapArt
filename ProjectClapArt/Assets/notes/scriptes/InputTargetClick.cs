using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputTargetClick : MonoBehaviour {

    //reismマネージャ
    reismMng reism_mng = null;

    [SerializeField] int error_range = 2;

    // Start is called before the first frame update
    void Start() {
        //reismマネージャを取得
        reism_mng = this.GetComponent<reismMng>();
    }

    // Update is called once per frame
    void Update() {

        //スペースキーが押されたときに判定する
        if (Input.GetKey(KeyCode.Space))
            InputTimmingJudge();
    }

    //判定
    private void InputTimmingJudge() {
        List<notesDateClass> trgt_timng_ptr = reism_mng.target_date;

        //予め多めの誤差にしておく
        int more_dff = error_range * 2;

        //検出したnotesのKeyを記録する
        notesDateClass key_nots_date = null;

        foreach (notesDateClass nots_date in trgt_timng_ptr) {
            //タイミングしかみていない
            int diff = reism_mng.GameInTime - nots_date.getTrgtNotsClkTiming();

            if ( diff >= 0  && diff < more_dff) {
                more_dff = diff;
                key_nots_date = nots_date;
            }
        }

        //判定が誤差範囲内なら今は緑にしておく
        if (more_dff < error_range) {
            if (key_nots_date != null) {
                //notesの削除
                Destroy(key_nots_date.getTrgtInstance());
                trgt_timng_ptr.Remove(key_nots_date);
            }
        }
    }
}