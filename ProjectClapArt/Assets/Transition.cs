using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Playables;

public class Transition : MonoBehaviour
{
    /// <summary>
    /// シングルトン
    /// </summary>
    private static Transition Instance;
    public static Transition instance
    {
        get { return Instance; }
    }

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    PlayableDirector pd;
    string nextScene = "";

    // Start is called before the first frame update
    void Start()
    {
        pd = gameObject.GetComponent<PlayableDirector>();   
    }

    // Update is called once per frame
    void Update()
    {
        if (pd.time >= 1.0f)
        {
            if (nextScene != "")
            {
                Debug.Log("scene change");
                SceneManager.LoadScene(nextScene);
                nextScene = "";
            }
        }
    }

    public void ChangeScene (string nextSceneName)
    {
        if (pd.state == PlayState.Paused)
        {
            nextScene = nextSceneName;
            pd.Play();
        }
    }
}
