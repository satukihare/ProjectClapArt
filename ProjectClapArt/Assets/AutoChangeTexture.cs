using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class AutoChangeTexture : MonoBehaviour
{
   // [SerializeField]
    RawImage CharImage;

    [SerializeField]
    Texture Nagi, Kai;

    // Start is called before the first frame update
    void Start()
    {
        CharImage = GetComponent<RawImage>();

       
        if (SelectData.chara_select == 0)
        {
            CharImage.texture = Nagi;
        }
        else
        {
            CharImage.texture = Kai;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
