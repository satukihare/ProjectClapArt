using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> Exsample = null;
    [SerializeField] private List<GameObject> @object = null;
    [SerializeField] private GameObject Parent = null;
    private NoteObject EditObj = null;
    public int Type { set; get; }
    public int num { set; get; }

    // Start is called before the first frame update
    void Start()
    {
       num = 0;
        EditObj = @object[num].GetComponent<NoteObject>();
        Debug.Log(@object.Count);
    }

    // Update is called once per frame
    void Update()
    {
        EditNotes();
    }

    private void EditNotes()
    {

        if (Input.GetKey(KeyCode.D))
        {
            EditObj.MoveRightPos(0.001f);
            //@object[num].MoveRightPos(0.001f);
        }

        if (Input.GetKey(KeyCode.A))
        {
            EditObj.MoveLeftPos(0.001f);
            //@object[num].MoveLeftPos(0.001f);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            if (num < @object.Count-1)
            {
                num++;
                EditObj = @object[num].GetComponent<NoteObject>();
                Debug.Log(num);
            }
            
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            if (num > 0)
            {
                num--;
                EditObj = @object[num].GetComponent<NoteObject>();
                Debug.Log(num);
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            @object.Add(Instantiate(@object[num]));
            num = @object.Count-1;
            @object[num].transform.SetParent(Parent.transform, false);
            EditObj = @object[num].GetComponent<NoteObject>();
        }
    }

    public void EditEnd()
    {
        @object[num].SetActive(false);
    }

    public void EditStart()
    {
        @object[num].SetActive(true);
    }

}
