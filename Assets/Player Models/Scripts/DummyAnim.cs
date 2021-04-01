using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyAnim : MonoBehaviour
{
    Animator anim;
    private DummyStagger ds;
    public GameObject capsuleCollide;

    [SerializeField] private float nextStaggeredTime;
    [SerializeField] private float staggerCooldown;

    

    // Start is called before the first frame update
    void Start()
    {
        ds = capsuleCollide.GetComponent<DummyStagger>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ds.isStaggered == true)
        {
            if (Time.time > nextStaggeredTime)
            {
                anim.SetTrigger("isStaggered");

                nextStaggeredTime = Time.time + staggerCooldown;
                //ds.isStaggered = false;
                       
            }
            else
            {
                ds.isStaggered = false;
            }
        }
    }

    
}
