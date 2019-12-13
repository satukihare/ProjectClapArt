using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotesPage : MonoBehaviour
{
    [SerializeField] private Text text = null;
    [SerializeField] private NotesPage Maxpage = null;
    public int nowPage { set; get; }
    private static int MinPage = 1;
    private void Start()
    {
        nowPage = int.Parse(text.text);
    }

    public void NextPage()
    {
        if (nowPage < Maxpage.nowPage)
        {
            nowPage++;
            text.text = nowPage.ToString();
        }
    }

    public void BeforePage()
    {
        if (nowPage > MinPage)
        {
            nowPage--;
            text.text = nowPage.ToString();
        }
        
    }

    public void setPage(int num)
    {
        nowPage = num;
        text.text = nowPage.ToString();
    }
}
