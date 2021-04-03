using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyStagger : MonoBehaviour
{
    public bool isStaggered;
    public PlayerStatus ps;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name == "Kick")
        {
            isStaggered = true;
            Debug.Log("Collision Detected");
        }

        if (col.gameObject.name == "Punch")
        {
            isStaggered = true;
            Debug.Log("Collision Detected");
            ps.isHunter = true;
        }
    }
}
