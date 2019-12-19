using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class TutorialDialogManager : MonoBehaviour
{
    static readonly string[] codes =
    {
        "[Sound]",
        "[Speech]",
        "[Pause]",
        "[End]"
    };

    enum CodeID
    {
        Sound = 0,
        Speech,
        Pause,
        End
    };
    const float secsPerChar = 0.06f;
    TextAsset textAsset;
    string[] lines;

    [SerializeField]
    Text textObject;
    [SerializeField]
    Image[] speechBubble;
    string text;

    int currentLine;
    int showLength;
    int maxLength;
    float charTime;
    AudioSource audioSource;
    bool advance = false;

    [SerializeField]
    tutorialGameMnger manager = null;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        LoadTextFile(ScenarioData.text_filename);
        currentLine = 0;
        showLength = 0;
        charTime = secsPerChar;
        AdvanceText();
    }

    // Update is called once per frame
    void Update()
    {
        if (showLength < maxLength)
        {
            charTime -= Time.deltaTime;
            if (charTime <= 0)
            {
                charTime += secsPerChar;
                ++showLength;
                textObject.text = text.Substring(0, showLength);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                textObject.text = text;
                showLength = maxLength;
            }
        }
        else
        {
            if (!audioSource.isPlaying)
            {
                advance = true;
                Invoke("AdvanceText", 1.0f);
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                advance = true;
                AdvanceText();
            }
        }
    }

    void AdvanceText()
    {
        if (!advance) return;
        advance = false;
        while (true)
        {
            Debug.Log("parsing line: " + lines[currentLine]);
            if (lines[currentLine] == codes[(int)CodeID.Sound])
            {
                PlaySound();
            }
            else if (lines[currentLine] == codes[(int)CodeID.Speech])
            {
                ChangeText();
                break;
            }
            else if (lines[currentLine] == codes[(int)CodeID.Pause])
            {
                Pause();
                break;
            }
            else if (lines[currentLine] == codes[(int)CodeID.End])
            {
                End();
                break;
            }
            ++currentLine;
        }
    }

    public void LoadTextFile(string FilePath)
    {
        TextAsset loadAsset = Resources.Load<TextAsset>(FilePath);
        Debug.Log("FilePath " + FilePath);
        //新しいテキストファイルが読み込まれたら更新
        if (loadAsset == null)
        {
            Debug.Log("TextloadFailed");
        }
        else
        {
            if (textAsset != loadAsset)
            {
                textAsset = loadAsset;
                string[] split = { "\r\n" };
                lines = textAsset.text.Split(split, System.StringSplitOptions.RemoveEmptyEntries);
            }
        }
    }

    void PlaySound()
    {
        string filename;
        filename = lines[++currentLine];
        AudioClip sound = Resources.Load<AudioClip>("Audio/" + filename);
        audioSource.clip = sound;
        audioSource.Play();
        Debug.Log("play sound " + filename);
    }

    void ChangeText()
    {
        int chara;
        chara = int.Parse(lines[++currentLine]);

        speechBubble[chara].gameObject.SetActive(true);
        speechBubble[chara ^ 1].gameObject.SetActive(false);


        text = "";
        while (lines[++currentLine][0] != '[')
        {
            text = text + lines[currentLine] + '\n';
        }

        showLength = 0;
        maxLength = text.Length;

        Debug.Log("change text\n" + text);
    }


    void Pause()
    {
        manager.resume();
        gameObject.SetActive(false);
    }

    void End()
    {
        //game.active = true;
        //gameObject.SetActive(false);
        string scene;
        scene = lines[++currentLine];
        Transition.instance.ChangeScene(scene);
    }
}
