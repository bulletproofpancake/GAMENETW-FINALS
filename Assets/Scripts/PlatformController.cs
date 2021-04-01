using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlatformController : MonoBehaviour
{
    public static PlatformController Controller;
    public List<Ground> grounds;

    [SerializeField] private float raiseSpeed, raiseDuration;
    [SerializeField] private float repeatMin, repeatMax;
    
    private void Awake()
    {
        Controller = this;
    }

    private void Start()
    {
        InvokeRepeating("RaisePlatform",1f,Random.Range(repeatMin,repeatMax));
    }

    private void RaisePlatform()
    {
        StartCoroutine(Raise());
    }

    private IEnumerator Raise()
    {
        var timer = raiseDuration;
        var ground = grounds[Random.Range(0, grounds.Count)];
        var rb = ground.GetComponent<Rigidbody>();

        if(!ground.isRaised){
            while (timer > 0)
            {
                ground.isActive = true;
                rb.MovePosition(rb.position + Vector3.up * (raiseSpeed * Time.fixedDeltaTime));
                yield return new WaitForEndOfFrame();
                timer -= Time.deltaTime;
            }

            rb.velocity = Vector3.zero;
            ground.isActive = false;
            ground.isRaised = true;
        }
        else
        {
            while (timer > 0)
            {
                ground.isActive = true;
                rb.MovePosition(rb.position + Vector3.down * (raiseSpeed * Time.fixedDeltaTime));
                yield return new WaitForEndOfFrame();
                timer -= Time.deltaTime;
            }

            rb.velocity = Vector3.zero;
            ground.isActive = false;
            ground.isRaised = false;
        }
    }
    
}
