using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Marker : MonoBehaviour
{
    [SerializeField] public Image NoteObject = null ;
    //[SerializeField] public Image NoteObject { set; get; }
    [SerializeField] private Image marker = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 Min = new Vector2(NoteObject.rectTransform.anchorMin.x, marker.rectTransform.anchorMin.y);
        Vector2 Max = new Vector2(NoteObject.rectTransform.anchorMax.x, marker.rectTransform.anchorMax.y);
        marker.rectTransform.anchorMin = Min;
        marker.rectTransform.anchorMax = Max;
    }

    public void Active(bool flug)
    {
        var obj = marker.GetComponent<GameObject>();
        obj.SetActive(false);
    }

}
