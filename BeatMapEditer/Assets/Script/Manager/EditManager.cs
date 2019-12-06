using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditManager : MonoBehaviour
{
    [SerializeField] private ToolManager ToolManager = null;
    [SerializeField] private List<GameObject> PlayObj = null;
    [SerializeField] private List<GameObject> NoteObject = null;
    [SerializeField] private GameObject Parent = null;
    [SerializeField] private Dropdown Dropdown = null;
    private NoteObject EditObj = null;
    private NoteObject FollowObj = null;
    private Bar NoteData;
    public Note.NOTE_TYPE NOTE_TYPE = Note.NOTE_TYPE.UNKNOWN;
    public int Type { set; get; }
    public int num { set; get; }
    public bool EditMode { set; get; }



    // Start is called before the first frame update
    void Start()
    {
       num = 0;
        EditMode = true;
        NoteData = new Bar();
        EditObj = NoteObject[num].GetComponent<NoteObject>();
        FollowObj = PlayObj[num].GetComponent<NoteObject>();
        Debug.Log(NoteObject.Count);
    }

    // Update is called once per frame
    void Update()
    {
        if (EditMode == true)
        {
            EditNotes();
        }
        else
        {
            DeleteNotes();
        }
    }

    private void EditNotes()
    {

        if (Input.GetKey(KeyCode.D))
        {
            if (EditObj.obj.rectTransform.anchorMax.x < 0.5f)
            {
                EditObj.MoveRightPos(0.001f);
                FollowObj.MoveRightPos(0.001f);
            }
            //@object[num].MoveRightPos(0.001f);
        }

        if (Input.GetKey(KeyCode.A))
        {
            if (EditObj.obj.rectTransform.anchorMin.x > 0)
            {
                EditObj.MoveLeftPos(0.001f);
                FollowObj.MoveLeftPos(0.001f);
            }
            //@object[num].MoveLeftPos(0.001f);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            if (num < NoteObject.Count-1)
            {
                num++;
                EditObj = NoteObject[num].GetComponent<NoteObject>();
                FollowObj = PlayObj[num].GetComponent<NoteObject>();
                Debug.Log(num);
            }
            
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            if (num > 0)
            {
                num--;
                EditObj = NoteObject[num].GetComponent<NoteObject>();
                FollowObj = PlayObj[num].GetComponent<NoteObject>();
                Debug.Log(num);
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            AddNote();
        }
    }

    private void DeleteNotes()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (num < NoteObject.Count - 1)
            {
                num++;
                EditObj = NoteObject[num].GetComponent<NoteObject>();
                FollowObj = PlayObj[num].GetComponent<NoteObject>();
                Debug.Log(num);
            }

        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            if (num > 0)
            {
                num--;
                EditObj = NoteObject[num].GetComponent<NoteObject>();
                FollowObj = PlayObj[num].GetComponent<NoteObject>();
                Debug.Log(num);
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            NoteObject[num].SetActive(false);
            PlayObj[num].SetActive(false);

            PlayObj.RemoveAt(num);
            NoteObject.RemoveAt(num);
        }
    }

    private void AddNote()
    {
        SetNoteData();
        //ノーツを複製してリストに追加、操作対象を新しいものに
        
        NoteObject.Add(Instantiate(NoteObject[num]));
        PlayObj.Add(Instantiate(PlayObj[num]));

        num = NoteObject.Count - 1;

        NoteObject[num].transform.SetParent(Parent.transform, false);
        PlayObj[num].transform.SetParent(Parent.transform, false);

        EditObj = NoteObject[num].GetComponent<NoteObject>();
        FollowObj = PlayObj[num].GetComponent<NoteObject>();
    }

    private void SetNoteData()
    {
        //譜面データの作成
        TypeSetter();
        var Spown = ToolManager.Bar * EditObj.obj.rectTransform.anchorMin.x;
        var Press = ToolManager.Bar * FollowObj.obj.rectTransform.anchorMin.x;
        //作成したデータをリストに追加
        NoteData.Lingth = ToolManager.Bar;

        //NoteData.Notes.Add(new Note(/*2Dスプライトの座標*/, (int)(Spown * 1000), (int)(Press * 1000), NOTE_TYPE));
    }

    private void TypeSetter()
    {
        switch (Type)
        {
            case 0:
                NOTE_TYPE = Note.NOTE_TYPE.FLICK;
                break;
            case 1:
                NOTE_TYPE = Note.NOTE_TYPE.TOUCH;
                break;
        }
    }

    public void EditEnd()
    {
        NoteObject[num].SetActive(false);
    }

    public void EditStart()
    {
        NoteObject[num].SetActive(true);
    }

}
