using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InputManager : MonoBehaviour
{
    const float maxTapTime = 0.5f;
    const float maxFlickTime = 0.75f;
    const float minFlickDist = 50.0f;
    const float flickDeadZone = 10.0f;
    float pressTime;
    Vector2 curPos, flickStartPos;

    private bool tap;
    private bool prevFlick;
    private bool flick;

    [SerializeField]
	private bool debugMode;
    public Toggle debugTap;
    public Toggle debugFlickStart;
    public Toggle debugFlick;
    public Toggle debugFlickEnd;
	
    public bool Tap {
        get {return tap;}
    }
    public bool Flick {
        get {return flick;}
    }
    public bool FlickStart
    {
        get {return (flick && !prevFlick);}
    }
    public bool FlickEnd
    {
		get {return (!flick && prevFlick);}
    }


    // Start is called before the first frame update
    void Start()
    {
        pressTime = 0;
        tap = flick = false;
    }

    // Update is called once per frame
    void Update()
    {
        tap = false;

        if (Input.GetMouseButtonDown(0))
        {
            pressTime = Time.time;

            float heldTime = Time.time - pressTime;
            if (heldTime < maxTapTime)
            {
              
                //Debug.Log("detected as tap");
            }
            tap = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
          
        }

        Vector2 mov = (Vector2)Input.mousePosition - curPos;
        prevFlick = flick;
        if (flick)
        {
            Debug.Log("flicking");
            if (mov.magnitude < flickDeadZone)
            {
                //Debug.Log("flick end");
                flick = false;
            }
        }
        else
        {
            if (mov.magnitude > flickDeadZone)
            {
                //Debug.Log("flick start");
                flick = true;
            }
        }
        curPos = Input.mousePosition;

		if (debugMode)
		{
			ColorBlock cb;

			cb = debugTap.colors;
			cb.disabledColor = tap ? Color.green : Color.red;
			debugTap.colors = cb;

			cb = debugFlickStart.colors;
			cb.disabledColor = FlickStart ? Color.green : Color.red;
			debugFlickStart.colors = cb;

			cb = debugFlick.colors;
			cb.disabledColor = flick ? Color.green : Color.red;
			debugFlick.colors = cb;

			cb = debugFlickEnd.colors;
			cb.disabledColor = FlickEnd ? Color.green : Color.red;
			debugFlickEnd.colors = cb;
		}
    }

}
