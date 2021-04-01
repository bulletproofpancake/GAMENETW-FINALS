using UnityEngine;

public class Ground : MonoBehaviour
{
    private Material _material;
    private Color _colorBase;

    public bool isActive, hasPlayer, isRaised;
    
    private void Awake()
    {
        _material = GetComponent<Renderer>().material;
        _colorBase = _material.color;
    }

    private void Start()
    {
        PlatformController.Controller.grounds.Add(this);
    }
    
    private void Update()
    {
        if (isActive)
            _material.color = hasPlayer ? Color.red : Color.yellow;
        else if (isRaised)
            _material.color = hasPlayer ? Color.red : Color.cyan;
        else
            _material.color = hasPlayer ? Color.green : _colorBase;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
            hasPlayer = true;
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
            hasPlayer = false;
    }
}
