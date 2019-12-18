using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayButton : MonoBehaviour
{
    [SerializeField] private ToolManager musicManager = null;
    [SerializeField] private EditManager editManager = null;
    [SerializeField] private Text text = null;
    [SerializeField] private Timer timer = null;
    private bool flug;

    void Start()
    {
        flug = false;
    }

    public void Onclick()
    {
        if (flug == false)
        {
            flug = true;
            timer.flug = true;
            text.text = "||";
            musicManager.PlayStart();
            editManager.EditEnd();
        }
        else
        {
            flug = false;
            timer.flug = false;
            text.text = "▶";
            musicManager.PlayStop();
            editManager.EditStart();
        }
    }

   


}
