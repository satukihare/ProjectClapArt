using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerationTarget : MonoBehaviour {

    [SerializeField] GameObject pop_trgt_obj = null;

    //reismマネージャ
    reismMng reism_mng = null;

    // Start is called before the first frame update
    void Start() {
        //reismマネージャを取得
        reism_mng = this.GetComponent<reismMng>();
    }

    // Update is called once per frame
    void Update() {
        foreach (notesDateClass nots_date in reism_mng.target_date) {

            if (reism_mng.GameInTime == nots_date.getTrgtPopTimming()) {
                if (!nots_date.getGeneFlg()) {
                    //生成
                    GameObject trgt_inst = Instantiate(pop_trgt_obj, nots_date.getPosition(), Quaternion.identity);
                    nots_date.setTrgtInstance(trgt_inst);
                    nots_date.tragtGeneFlg();
                }
            }
        }
    }
}
