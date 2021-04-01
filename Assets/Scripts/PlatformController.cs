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
    
    private void Awake()
    {
        Controller = this;
    }

    private void Start()
    {
        StartCoroutine(Raise());
    }

    private IEnumerator Raise()
    {
        var ground = grounds[Random.Range(0, grounds.Count)];
        var rb = ground.GetComponent<Rigidbody>();
        ground.isActive = true;
        yield return new WaitForSeconds(raiseDuration);
        ground.isActive = false;
    }
    
}
