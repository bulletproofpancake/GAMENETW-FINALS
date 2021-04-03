using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCollision : MonoBehaviour
{
    public bool isStaggered;
    public bool becomeHunter = false;
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
        //This is to check if your player got hit by either the punch or kick
        if (col.gameObject.name == "Kick")
        {
            isStaggered = true;
            Debug.Log("Collision Detected");
        }

        if (col.gameObject.name == "Punch")
        {
            isStaggered = true;
            Debug.Log("Collision Detected");
            becomeHunter = true;
            ps.isHunter = true;
        }
    }
}
