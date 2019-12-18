using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveableEffect : MonoBehaviour, IDragHandler
{
    private static Vector3 default_pos;
    public bool flug { set; get; } 

    private void Start()
    {
        flug = false;
        default_pos.x = default_pos.y = default_pos.z = 0.0f;
        //default_pos = transform.position;
        //transform.position = default_pos;
    }

    public void ResetPos()
    {
        transform.position = default_pos;
    }

    //[SerializeField] private Camera camera = null;
    public void OnDrag(PointerEventData data)
    {
        if(flug == false)
        {
            Vector3 TargetPos = Camera.main.ScreenToWorldPoint(data.position);
            TargetPos.z = 0;
            transform.position = TargetPos;
        }
        
    }
}
