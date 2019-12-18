using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class NoteObject : MonoBehaviour
{

    private Vector2 Pos;
    private Vector2 StartMax, StartMin;
    public Anchor anchors = new Anchor();
    //private int ID = 1;
    [SerializeField] public Image obj = null;
    //[SerializeField] public Text tex = null;
    public Note.NOTE_TYPE NOTE_TYPE = Note.NOTE_TYPE.UNKNOWN;

    // Start is called before the first frame update
    void Start()
    {
        Pos.x = 0.0f;
        Pos.y = 0.0f;
        StartMin = obj.rectTransform.anchorMin;
        StartMax = obj.rectTransform.anchorMax;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void MoveRightPos(float num)
    {
        Pos.x = num;

        obj.rectTransform.anchorMin += Pos;
        obj.rectTransform.anchorMax += Pos;

        anchors.Min = obj.rectTransform.anchorMin;
        anchors.Max = obj.rectTransform.anchorMax;
    }

    public void MoveLeftPos(float num)
    {
        Pos.x = num;

        obj.rectTransform.anchorMin -= Pos;
        obj.rectTransform.anchorMax -= Pos;

        anchors.Min = obj.rectTransform.anchorMin;
        anchors.Max = obj.rectTransform.anchorMax;

    }

    public void ChangeColor()
    {
        switch (NOTE_TYPE)
        {
            case Note.NOTE_TYPE.UNKNOWN:

                break;

            case Note.NOTE_TYPE.FLICK:
                obj.color = new Color(0, 214, 255);
                break;

            case Note.NOTE_TYPE.TOUCH:
                obj.color = new Color(255, 130, 0);
                break;
        }
    }

   // public void AddNumber()
    //{
    //    ID += 1;
    //    tex.text = ID.ToString();
    //}

    public void ResetPos()
    {
        //obj.rectTransform.localPosition = Pos;
        obj.rectTransform.anchorMin = StartMin;
        obj.rectTransform.anchorMax = StartMax;
    }

    public void CreateInit()
    {
        //obj.rectTransform.localPosition = Pos;
        obj.rectTransform.anchorMin = anchors.Min;
        obj.rectTransform.anchorMax = anchors.Max;
    }
}
