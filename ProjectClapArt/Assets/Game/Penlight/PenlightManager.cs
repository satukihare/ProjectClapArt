using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenlightManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetBPM (float bpm)
    {
        float multi = 100.0f / bpm;
        Debug.Log("multi = " + multi);
        Debug.Log("bpm = " + bpm);
        foreach (Transform child in transform)
        {
            foreach (Transform penlight in child.transform)
            {
                Animator anim = penlight.gameObject.GetComponent<Animator>();
                anim.SetFloat("Speed Multiplier", multi);
            }
        }
    }
}
