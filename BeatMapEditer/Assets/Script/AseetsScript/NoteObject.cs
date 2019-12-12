using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class NoteObject : MonoBehaviour
{

    private Vector2 Pos;
    private Vector2 StartMax, StartMin;
    [SerializeField] public Image obj = null;
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
       // if (obj.rectTransform.anchorMax.x < 0.5f)
        //{
            obj.rectTransform.anchorMin += Pos;
            obj.rectTransform.anchorMax += Pos;
        //}
    }

    public void MoveLeftPos(float num)
    {
        Pos.x = num;
        //if (obj.rectTransform.anchorMin.x > 0)
        //{
            obj.rectTransform.anchorMin -= Pos;
            obj.rectTransform.anchorMax -= Pos;
        //}
    }

    public void ResetPos()
    {
        obj.rectTransform.localPosition = Pos;
    }
}
