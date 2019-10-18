using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
public class FrameCounter : MonoBehaviour {

    Text score_object = null; // Textオブジェクト

    [SerializeField] reismMng reism_mng = null;

    // Start is called before the first frame update
    void Start() {
        score_object = this.GetComponent<Text>();
    }

    // 更新
    void Update() {

        // オブジェクトからTextコンポーネントを取得
        Text score_text = score_object.GetComponent<Text>();

        string num_text = reism_mng.GameInTime.ToString();//Time.time.ToString();

        for ( ; ; ) {
            if (num_text.Length > 4) {
                break;
            }
            num_text += "0";
        }

        // テキストの表示を入れ替える
        score_text.text = num_text;
    }
}