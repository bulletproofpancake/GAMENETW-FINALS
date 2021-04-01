using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    public static PlatformController Controller;
    public List<Ground> ground;

    private void Awake()
    {
        Controller = this;
    }

    private void Start()
    {
        ground[Random.Range(0,ground.Count)].ChangeColor(Color.blue);
    }
}
