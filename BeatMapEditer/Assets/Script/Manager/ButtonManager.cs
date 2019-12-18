using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    [SerializeField] private Timer timer = null;
    [SerializeField] private ToolManager toolManager = null;
    [SerializeField] private EditManager editManager = null;
    [SerializeField] private NotesPage notesPage = null;
    [SerializeField] private NotesPage MaxPage = null;
    [SerializeField] private LinePos line = null;

    static int PageTime = 4;

    public void OnclickNextPage()
    {
        if (notesPage.nowPage < MaxPage.nowPage)
        {
            notesPage.NextPage();
            timer.addTime(PageTime);
            toolManager.MusicTimer += PageTime;
            editManager.SaveList();
            editManager.Listindex += 1;
            editManager.DataRestart(false);
            line.ResetPos();
        }
    }

    public void OnclickBeforePage()
    {
        if(line.StartPosflug == false && notesPage.nowPage > 1)
        {
            notesPage.BeforePage();

            timer.addTime(-PageTime);
            toolManager.MusicTimer -= PageTime;
            editManager.SaveList();
            editManager.Listindex -= 1;
            editManager.DataRestart(false);
            if (toolManager.MusicTimer <= 0)
            {
                toolManager.MusicTimer = 0.0f;
                line.ResetPos();
            }
        }
        else
        {
            toolManager.MusicTimer = toolManager.PageEndMusicTimer;
            line.ResetPos();
        }
        
    }
}
