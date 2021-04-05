using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Basically, eto yung ground niyo in my sample;
public class ColorItem : MonoBehaviour
{
    private SpriteRenderer sr;
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    //just a function that sets the color
    public void ChangeColor(Color c)
    {
        sr.color = c;
    }
}
