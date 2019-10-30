using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMController : MonoBehaviour {
    //音楽再生用オブジェクト
    AudioSource audio_source;

    //Bgmの再生位置を読み込みオンリーで書き込む
    public float BgmPlayPos {
        get { return audio_source.time; }
    }

    //音楽データ格納
    [SerializeField] List<AudioClip> audio_clip_list = new List<AudioClip>();

    /// <summary>
    /// 初期化
    /// </summary>
    void Start() {
        audio_source = GetComponent<AudioSource>();
    }

    /// <summary>
    /// 更新
    /// </summary>
    void Update() {
        //とりあえず再生タイミングはボタン
        if (Input.GetKey(KeyCode.J)) {
            audio_source.Stop();
            audio_source.clip = audio_clip_list[0];
            audio_source.Play();
        }
        //if (Input.GetKey(KeyCode.K)) {
        //    audio_source.Stop();
        //    audio_source.clip = audio_clip_list[1];
        //    audio_source.Play();
        //}
        //if (Input.GetKey(KeyCode.L)) {
        //    audio_source.Stop();
        //    audio_source.clip = audio_clip_list[2];
        //    audio_source.Play();
        //}
    }
}
