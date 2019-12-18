using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Move_Notes : MonoBehaviour
{
    [SerializeField] private Scrollbar Scrollbar= null;
    [SerializeField] private ToolManager manager = null;
    [SerializeField] private LinePos line = null;
    private float oldvalue;

    public void Move()
    {
        TimeMove();
        oldvalue = Scrollbar.value;
        line.pos.x = Scrollbar.value;
        line.SetPos();
        
    }

    private void TimeMove()
    {
        float num;
        num = Scrollbar.value - oldvalue;
        
        if (num > 0)
        {
            manager.NotesTimer -= num;
            manager.MusicTimer -= num;
        }
        else
        {
            manager.NotesTimer += num;
            manager.MusicTimer += num;
        }

        Debug.Log("num is" + num);
        Debug.Log("NotesTimer is" + manager.NotesTimer);
        Debug.Log("NowMusicTime is" + manager.MusicTimer);

    }
}
