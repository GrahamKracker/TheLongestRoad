using System;
using MelonLoader;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TheLongestRoad.MonoBehaviors;

[RegisterTypeInIl2Cpp(false)]
public class TreeBlow : MonoBehaviour
{
    public TreeBlow(IntPtr ptr) : base(ptr) { }

    private readonly float _distX = Random.Range(-3f, 3f);
    private readonly float _distY = Random.Range(-.5f, .5f);
    private readonly float _distZ = Random.Range(-1.5f, 1.5f);

    private readonly float _speedX = Random.Range(-1f, 1f);  
    private readonly float _speedY = Random.Range(-.75f, .75f);
    private readonly float _speedZ = Random.Range(-1f, 1f);

    private Vector3 _startPos;

    private void Start()
    {
        _startPos = transform.position;
    }

    private void Update()
    {
        transform.Translate(Mathf.Lerp(-_distX, _distX, Mathf.PingPong(Time.time, 1)) * _speedX * Time.deltaTime,Mathf.Lerp(-_distY, _distY, Mathf.PingPong(Time.time, 1)) * _speedY * Time.deltaTime,Mathf.Lerp(-_distZ, _distZ, Mathf.PingPong(Time.time, 1)) * _speedZ * Time.deltaTime, Space.World);
        if (Vector3.Distance(_startPos, transform.position) > 5)
        {
            transform.position = _startPos;
        }
    }
}