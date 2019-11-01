using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    static readonly string[] codes =
    {
        "[Expression]",
        "[Face]",
        "[Speech]",
        "[End]"
    };
    const float secsPerChar = 0.06f;
    TextAsset textAsset;
    string[] lines;

    [SerializeField]
    Text showText;
    string text;

    int currentLine;
    int showLength;
    int maxLength;
    float charTime;

    // Start is called before the first frame update
    void Start()
    {
        LoadTextFile("Text/test");
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
                showText.text = text.Substring(0, showLength);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                showText.text = text;
                showLength = maxLength;
            }
        }
        else
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
            //Debug.Log("parsing line: " + lines[currentLine]);
            if (lines[currentLine] == codes[0])
            {
                ChangeExpression();
            }
            else if (lines[currentLine] == codes[1])
            {
                ChangeFace();
            }
            else if (lines[currentLine] == codes[2])
            {
                ChangeText();
                break;
            }
            else if (lines[currentLine] == codes[3])
            {
                End();
                break;
            }
            ++currentLine;
        }
    }

    public void LoadTextFile(string FilePath)
    {
        TextAsset loadAsset = (Resources.Load(FilePath, typeof(TextAsset)) as TextAsset);
        //Debug.Log("FilePath " + FilePath);
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
    void ChangeExpression()
    {
        int chara;
        int exp;
        chara = int.Parse(lines[++currentLine]);
        exp = int.Parse(lines[++currentLine]);
        //TODO
        //Debug.Log("change expression " + chara.ToString() + ' ' + exp.ToString());
    }

    void ChangeFace()
    {
        int chara;
        int face;
        chara = int.Parse(lines[++currentLine]);
        face = int.Parse(lines[++currentLine]);
        //TODO
        //Debug.Log("change face " + chara.ToString() + ' ' + face.ToString());
    }
    
    void ChangeText()
    {
        int chara;
        chara = int.Parse(lines[++currentLine]);

        text = "";
        while (lines[++currentLine][0] != '[')
        {
            text = text + lines[currentLine] + '\n';
        }

        showLength = 0;
        maxLength = text.Length;

        //Debug.Log("change text\n" + text);
    }

    void End()
    {
        //game.active = true;
        gameObject.SetActive(false);
    }
}
