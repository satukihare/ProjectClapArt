using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    static readonly string[] codes =
    {
        "[Banner]",
        "[Expression]",
        "[Face]",
        "[Fade]",
        "[Sound]",
        "[Speech]",
        "[End]"
    };

    enum CodeID
    {
        Banner = 0,
        Expression,
        Face,
        Fade,
        Sound,
        Speech,
        End
    };
    const float secsPerChar = 0.06f;
    TextAsset textAsset;
    string[] lines;

    [SerializeField]
    Text textObject;
    [SerializeField]
    Image[] speechBubble;
    [SerializeField]
    Image[] characters;
    [SerializeField]
    GameObject banner;
    [SerializeField]
    GameObject fade;
    string text;

    int currentLine;
    int showLength;
    int maxLength;
    float charTime;
    bool waitForAnim = false;

    // Start is called before the first frame update
    void Start()
    {
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
        else if (!waitForAnim)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                AdvanceText();
            }
        }
    }

    void AdvanceText()
    {
        while (true)
        {
            Debug.Log("parsing line: " + lines[currentLine]);
            if (lines[currentLine] == codes[(int)CodeID.Banner])
            {
                CreateBanner();
                break;
            }
            else if (lines[currentLine] == codes[(int)CodeID.Expression])
            {
                ChangeExpression();
            }
            else if (lines[currentLine] == codes[(int)CodeID.Face])
            {
                ChangeFace();
            }
            else if (lines[currentLine] == codes[(int)CodeID.Fade])
            {
                CreateFade();
                break;
            }
            else if (lines[currentLine] == codes[(int)CodeID.Sound])
            {
                PlaySound();
            }
            else if (lines[currentLine] == codes[(int)CodeID.Speech])
            {
                ChangeText();
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
                string[] split = {"\r\n"};
                lines = textAsset.text.Split(split, System.StringSplitOptions.RemoveEmptyEntries);
            }
        }
    }

    void CreateBanner()
    {
        waitForAnim = true;
        banner.SetActive(true);
        banner.transform.GetChild(1).gameObject.GetComponent<Text>().text = lines[++currentLine];
    }

    void ChangeExpression()
    {
        int chara;
        int exp;
        chara = int.Parse(lines[++currentLine]);
        exp = int.Parse(lines[++currentLine]);
        //TODO
        Debug.Log("change expression " + chara.ToString() + ' ' + exp.ToString());
    }

    void ChangeFace()
    {
        int chara;
        int face;
        chara = int.Parse(lines[++currentLine]);
        face = int.Parse(lines[++currentLine]);
        //TODO
        Debug.Log("change face " + chara.ToString() + ' ' + face.ToString());
    }

    void CreateFade()
    {
        fade.SetActive(true);
        int.Parse(lines[++currentLine]);
        waitForAnim = true;
    }

    void PlaySound()
    {
        string filename;
        filename = lines[++currentLine];
        AudioClip sound = Resources.Load<AudioClip>("Audio/" + filename);
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(sound, 1.0F);
        Debug.Log("play sound " + filename);
    }

    void ChangeText()
    {
        int chara;
        chara = int.Parse(lines[++currentLine]);

        speechBubble[chara  ].gameObject.SetActive(true);
        speechBubble[chara^1].gameObject.SetActive(false);

        characters[chara  ].color = Color.white;
        characters[chara^1].color = Color.gray;


        text = "";
        while (lines[++currentLine][0] != '[')
        {
            text = text + lines[currentLine] + '\n';
        }

        showLength = 0;
        maxLength = text.Length;

        Debug.Log("change text\n" + text);
    }

    void End()
    {
        //game.active = true;
        gameObject.SetActive(false);
    }

    public void AnimFinished()
    {
        if (waitForAnim)
        {
            waitForAnim = false;
            AdvanceText();
        }
    }
}
