﻿using System.Collections;
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

    static float PageTime = 4;

    public void OnclickNextPage()
    {
        if (notesPage.nowPage < MaxPage.nowPage)
        {
            PageTime = toolManager.BarTime;

            notesPage.NextPage();
            timer.addTime(PageTime);
            toolManager.MusicTimer += PageTime;
            editManager.SaveList();
            editManager.Listindex++;
            toolManager.index++;
            editManager.DataRestart(false);
            line.ResetPos();
        }
    }

    public void OnclickBeforePage()
    {
        if(line.StartPosflug == false && notesPage.nowPage > 1)
        {
            PageTime = toolManager.BarTime;

            notesPage.BeforePage();

            timer.addTime(-PageTime);
            toolManager.MusicTimer -= PageTime;
            editManager.SaveList();
            editManager.Listindex--;
            toolManager.index--;
            editManager.DataRestart(false);
            if (toolManager.MusicTimer <= 0)
            {
                toolManager.MusicTimer = 0.0f;
                line.ResetPos();
            }
        }
        else
        {
          if (notesPage.nowPage == 1)
          {
              timer.SetPageTime(0);
          }
          else
          {
              timer.SetPageTime(toolManager.PageEndMusicTimer[toolManager.index]);
          }

            toolManager.MusicTimer = toolManager.PageEndMusicTimer[editManager.Listindex];
            toolManager.NotesTimer = 0;
            line.ResetPos();
        }
        
    }
}
