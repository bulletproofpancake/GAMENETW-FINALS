using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlatformController : MonoBehaviour
{
    public static PlatformController Instance;
    public List<Ground> grounds,raised,floor;

    [SerializeField] private float raiseSpeed, raiseDuration;
    [SerializeField] private float repeatMin, repeatMax;
    
    private void Awake()
    {
        Instance = this;
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
            raised.Add(ground);
            floor.Remove(ground);
            rb.position = new Vector3(rb.position.x, Mathf.Clamp(rb.position.y, 0, 1), rb.position.z);
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
            raised.Remove(ground);
            floor.Add(ground);
            rb.position = new Vector3(rb.position.x, Mathf.Clamp(rb.position.y, -1, 0), rb.position.z);
        }
    }

    public void RemoveHunter(GameObject player)
    {
        var ground = floor[Random.Range(0, floor.Count)];
        player.transform.position = ground.transform.position;
    }

}
