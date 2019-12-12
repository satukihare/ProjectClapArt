﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditManager : MonoBehaviour
{
    [SerializeField] private ToolManager ToolManager = null;
    [SerializeField] private Dropdown Dropdown = null;
    [SerializeField] private GameObject PlayObj = null;
    [SerializeField] private GameObject NoteObject = null;
    [SerializeField] private GameObject gameObj = null;

    [SerializeField] private GameObject ExsampleParent = null;
    [SerializeField] private GameObject PlayParent = null;
    [SerializeField] private GameObject GameParent = null;

    private List<GameObject> PlayObjects = new List<GameObject>();
    private List<GameObject> NoteObjects = new List<GameObject>();
    private List<GameObject> gameObjects = new List<GameObject>();
    public List<Bar> Bars = new List<Bar>();

    public List<List<GameObject>> PlayLists = new List<List<GameObject>>();
    public List<List<GameObject>> NoteLists = new List<List<GameObject>>();
    public List<List<GameObject>> GameLists = new List<List<GameObject>>();

    //private List<Bar> bars = new List<Bar>();

    private NoteObject EditObj = null;
    private NoteObject FollowObj = null;
    private Bar NoteData = null;
    public Note.NOTE_TYPE NOTE_TYPE = Note.NOTE_TYPE.UNKNOWN;

    public int Type { set; get; }
    public int num { set; get; }
    public int Listindex { set; get; }
    public bool EditMode { set; get; }

    private static float limit = 1.0f;
    private static float move = 0.002f;

    // Start is called before the first frame update
    void Start()
    {
        Listindex = 0;
        Init();
    }

    private void Init()
    {
        num = 0;
        EditMode = true;
        NoteData = new Bar();

        PlayObj.SetActive(true);
        NoteObject.SetActive(true);
        gameObj.SetActive(true);

        PlayObjects = new List<GameObject>();
        NoteObjects = new List<GameObject>();
        gameObjects = new List<GameObject>();

        PlayObjects.Add(PlayObj);
        NoteObjects.Add(NoteObject);
        gameObjects.Add(gameObj);
        Bars.Add(NoteData);

        PlayLists.Add(PlayObjects);
        NoteLists.Add(NoteObjects);
        GameLists.Add(gameObjects);

        EditObj = NoteObjects[num].GetComponent<NoteObject>();
        FollowObj = PlayObjects[num].GetComponent<NoteObject>();
    }

    private void UnInit()
    {
        //PlayLists.Clear();
        //NoteLists.Clear();
        //GameLists.Clear();

        for(int num = 0; num < PlayObjects.Count;num++)
        {
            PlayObjects[num].SetActive(false);
            NoteObjects[num].SetActive(false);
            gameObjects[num].SetActive(false);
        }

        //PlayObjects.Clear();
        //NoteObjects.Clear();
        //gameObjects.Clear();
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

    public void DataRestart()
    {
        UnInit();
        if (Listindex == PlayLists.Count)
        {
            Init();
        }
        else
        {
            LoadList();
            for (int num = 0; num < PlayObjects.Count; num++)
            {
                PlayObjects[num].SetActive(true);
                NoteObjects[num].SetActive(true);
                gameObjects[num].SetActive(true);
            }
            num = 0;
            EditObj = NoteObjects[num].GetComponent<NoteObject>();
            FollowObj = PlayObjects[num].GetComponent<NoteObject>();
        }
    }

    private void EditNotes()
    {

        if (Input.GetKey(KeyCode.D))
        {
            if (EditObj.obj.rectTransform.anchorMax.x < limit)
            {
                EditObj.MoveRightPos(move);
                FollowObj.MoveRightPos(move);
            }
            //@object[num].MoveRightPos(0.001f);
        }

        if (Input.GetKey(KeyCode.A))
        {
            if (EditObj.obj.rectTransform.anchorMin.x > 0)
            {
                EditObj.MoveLeftPos(move);
                FollowObj.MoveLeftPos(move);
            }
            //@object[num].MoveLeftPos(0.001f);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            if (num < NoteObjects.Count-1)
            {
                num++;
                SetEdit();
                Debug.Log(num);
            }
            
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            if (num > 0)
            {
                num--;
                SetEdit();
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
            if (num < NoteObjects.Count - 1)
            {
                num++;
                EditObj = NoteObjects[num].GetComponent<NoteObject>();
                FollowObj = PlayObjects[num].GetComponent<NoteObject>();
                Debug.Log(num);
            }

        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            if (num > 0)
            {
                num--;
                EditObj = NoteObjects[num].GetComponent<NoteObject>();
                FollowObj = PlayObjects[num].GetComponent<NoteObject>();
                Debug.Log(num);
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            NoteObjects[num].SetActive(false);
            PlayObjects[num].SetActive(false);
            gameObjects[num].SetActive(false);

            PlayObjects.RemoveAt(num);
            NoteObjects.RemoveAt(num);
            gameObjects.RemoveAt(num);
        }
    }

    private void AddNote()
    {
        SetNoteData();
        //ノーツを複製してリストに追加、操作対象を新しいものに
        
        NoteObjects.Add(Instantiate(NoteObjects[num]));
        PlayObjects.Add(Instantiate(PlayObjects[num]));
        gameObjects.Add(Instantiate(gameObjects[num]));

        num = NoteObjects.Count - 1;

        NoteObjects[num].transform.SetParent(ExsampleParent.transform, false);
        PlayObjects[num].transform.SetParent(PlayParent.transform, false);
        gameObjects[num].transform.SetParent(GameParent.transform, false);

        EditObj = NoteObjects[num].GetComponent<NoteObject>();
        FollowObj = PlayObjects[num].GetComponent<NoteObject>();
    }

    public void SaveList()
    {
      PlayLists[Listindex] = PlayObjects;
      NoteLists[Listindex] = NoteObjects;
      GameLists[Listindex] = gameObjects;
      Bars[Listindex] = NoteData;
    }

    public void LoadList()
    {
        PlayObjects = PlayLists[Listindex];
        NoteObjects = NoteLists[Listindex];
        gameObjects = GameLists[Listindex];
        NoteData = Bars[Listindex];
    }

    private void SetNoteData()
    {
        //譜面データの作成
        TypeSetter();
        var Spown = ToolManager.BarTime * EditObj.obj.rectTransform.anchorMin.x;
        var Press = ToolManager.BarTime * FollowObj.obj.rectTransform.anchorMin.x * 2;
        //作成したデータをリストに追加
        NoteData.Lingth = ToolManager.BarTime;
        NoteData.Notes.Add(new Note(new Vector2 (gameObjects[num].transform.position.x, gameObjects[num].transform.position.y ), (int)(Spown * 1000), (int)(Press * 1000), NOTE_TYPE));
    }

    private void SetEdit()
    {
        EditObj = NoteObjects[num].GetComponent<NoteObject>();
        FollowObj = PlayObjects[num].GetComponent<NoteObject>();
    }

    private void TypeSetter()
    {
        switch (Type)
        {
            case 0:
                NOTE_TYPE = Note.NOTE_TYPE.FLICK;
                FollowObj.NOTE_TYPE = Note.NOTE_TYPE.FLICK;
                break;
            case 1:
                NOTE_TYPE = Note.NOTE_TYPE.TOUCH;
                FollowObj.NOTE_TYPE = Note.NOTE_TYPE.TOUCH;
                break;
        }
    }

    public void EditEnd()
    {
        NoteObjects[num].SetActive(false);
        PlayObjects[num].SetActive(false);
        gameObjects[num].SetActive(false);
    }

    public void EditStart()
    {
        NoteObjects[num].SetActive(true);
        PlayObjects[num].SetActive(true);
        gameObjects[num].SetActive(true);
    }

}
