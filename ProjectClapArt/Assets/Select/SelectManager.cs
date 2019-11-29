using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectManager : MonoBehaviour
{
    [SerializeField]
    GameObject[] ImageSets;
    int selection = 0;
    Vector2 curPos;
    // Start is called before the first frame update
    void Start()
    {
        curPos = (Vector2)Input.mousePosition;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mov = (Vector2)Input.mousePosition - curPos;
        if (!Transition.instance.playing)
        {
            if (mov.x > 10.0f)
            {
                selection = 2;
                ChangeImage();
            }
            else if (mov.x < -10.0f)
            {
                selection = 1;
                ChangeImage();
            }
        }
        curPos = Input.mousePosition;

        if (Input.GetMouseButtonDown(0))
        {
            if (selection != 0)
                Transition.instance.ChangeScene("TutorialScene");
        }
    }

    void ChangeImage ()
    {
        for (int i = 0; i < 3; i++)
        {
            ImageSets[i].SetActive(i == selection);
        }
    }
}
