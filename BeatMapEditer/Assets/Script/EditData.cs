using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditData 
{
    public List<List<GameObject>> PlayLists {set; get;}
    public List<List<GameObject>> NoteLists {set; get;}
    public List<List<GameObject>> GameLists {set; get;}
}

public class Anchor
{
    public Vector2 Min { set; get; }
    public Vector2 Max { set; get; }
}
